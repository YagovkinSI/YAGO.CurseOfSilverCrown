using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Web.Data;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions
{
    public class TaxAction : BaseAction
    {
        private readonly ApplicationDbContext context;

        protected override int ImportanceBase => 500;

        public TaxAction(Command command, Turn currentTurn, ApplicationDbContext context)
            : base(command, currentTurn)
        {
            this.context = context;
        }

        public static int GetTax(int warriors, int investments, double random)
        {
            var additionalWarriors = warriors;
            var baseTax = Constants.MinTax;
            var randomBaseTax = baseTax * (0.99 + random / 100.0);

            var investmentTax = Constants.GetInvestmentTax(investments);
            var randomInvestmentTax = investmentTax * (0.99 + random / 100.0);

            var additionalTax = Constants.GetAdditionalTax(additionalWarriors, random);

            return (int)Math.Round(randomBaseTax + randomInvestmentTax + additionalTax);
        }

        public override bool Execute()
        {
            var getCoffers = GetTax(_command.Warriors, _command.Organization.Investments, _random.NextDouble());
            var eventOrganizationList = GetEventOrganizationList(context, _command.Organization, getCoffers);

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = enEventResultType.TaxCollection,
                Organizations = eventOrganizationList 
            };

            EventStory = new EventStory
            {
                TurnId = currentTurn.Id,
                EventStoryJson = JsonConvert.SerializeObject(eventStoryResult)
            };

            OrganizationEventStories = eventOrganizationList
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

        private List<EventOrganization> GetEventOrganizationList(ApplicationDbContext context, Organization organization, 
            int allIncome, List<EventOrganization> currentList = null)
        {
            var type = enEventOrganizationType.Suzerain;
            if (currentList == null)
            {
                currentList = new List<EventOrganization>();
                type = enEventOrganizationType.Main;
            }

            var suzerainId = organization.SuzerainId;
            var getCoffers = suzerainId == null
                ? allIncome
                : (int)Math.Round(allIncome * (1 - Constants.BaseVassalTax));

            var eventOrganization = GetEventOrganization(organization, type, getCoffers);
            currentList.Add(eventOrganization);
            organization.Coffers += getCoffers;

            return suzerainId == null
                ? currentList
                : GetEventOrganizationList(context,
                    context.Organizations.Single(o => o.Id == suzerainId),
                    allIncome - getCoffers,
                    currentList);
        }

        private EventOrganization GetEventOrganization(Organization organization, enEventOrganizationType type, int getCoffers)
        {
            return new EventOrganization
            {
                Id = organization.Id,
                EventOrganizationType = type,
                EventOrganizationChanges = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enEventParametrChange.Coffers,
                                Before = organization.Coffers,
                                After = organization.Coffers + getCoffers
                            }
                        }

            };
        }
    }
}
