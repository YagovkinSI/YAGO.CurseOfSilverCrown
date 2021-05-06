using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Event;
using YSI.CurseOfSilverCrown.Core.Commands;
using YSI.CurseOfSilverCrown.Core.Actions.Organizations;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal class TaxAction : ActionBase
    {
        private readonly ApplicationDbContext context;

        protected int ImportanceBase => 500;

        public TaxAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
            this.context = context;
        }

        public static int GetTax(int warriors, int investments, double random)
        {
            var additionalWarriors = warriors;
            var baseTax = Constants.MinTax;
            var randomBaseTax = baseTax * (0.99 + random / 100.0);

            var investmentTax = InvestmentsHelper.GetInvestmentTax(investments);
            var randomInvestmentTax = investmentTax * (0.99 + random / 100.0);

            var additionalTax = Constants.GetAdditionalTax(additionalWarriors, random);

            return (int)Math.Round(randomBaseTax + randomInvestmentTax + additionalTax);
        }

        protected override bool Execute()
        {
            var getCoffers = GetTax(Command.Warriors, Command.Organization.Investments, Random.NextDouble());

            var eventStoryResult = new EventStoryResult(enEventResultType.TaxCollection);
            FillEventOrganizationList(eventStoryResult, context, Command.Organization, getCoffers);            

            EventStory = new EventStory
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = eventStoryResult.ToJson()
            };

            OrganizationEventStories = eventStoryResult.Organizations
                .Select(e =>
                    new OrganizationEventStory
                    {
                        OrganizationId = e.Id,
                        Importance = getCoffers / 20,
                        EventStory = EventStory
                    })
                .ToList();

            return true;
        }

        private void FillEventOrganizationList(EventStoryResult eventStoryResult, ApplicationDbContext context, Organization organization, 
            int allIncome, bool isMain = true)
        {
            var type = isMain
                ? enEventOrganizationType.Main
                : enEventOrganizationType.Suzerain;

            var suzerainId = organization.SuzerainId;
            var getCoffers = suzerainId == null
                ? allIncome
                : (int)Math.Round(allIncome * (1 - Constants.BaseVassalTax));

            var temp = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enActionParameter.Coffers,
                                Before = organization.Coffers,
                                After = organization.Coffers + getCoffers
                            }
                        };
            eventStoryResult.AddEventOrganization(organization, type, temp);

            organization.Coffers += getCoffers;
            if (suzerainId == null)
                return;
            
            FillEventOrganizationList(eventStoryResult, context,
                    context.Organizations.Single(o => o.Id == suzerainId),
                    allIncome - getCoffers,
                    false);
        }
    }
}
