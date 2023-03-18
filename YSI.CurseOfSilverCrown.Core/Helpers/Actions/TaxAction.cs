using System;
using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Commands;
using YSI.CurseOfSilverCrown.Core.Database.Domains;
using YSI.CurseOfSilverCrown.Core.Database.Events;
using YSI.CurseOfSilverCrown.Core.Database.Turns;
using YSI.CurseOfSilverCrown.Core.Database.Units;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Helpers.Events;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Actions
{
    internal class TaxAction : DomainActionBase
    {
        private readonly ApplicationDbContext context;

        protected int ImportanceBase => 500;

        public TaxAction(ApplicationDbContext context, Turn currentTurn, Domain domain)
            : base(context, currentTurn, domain)
        {
            this.context = context;
        }

        public override bool CheckValidAction()
        {
            return true;
        }

        public static int GetTax(int warriors, int investments)
        {
            var additionalWarriors = warriors;

            var investmentTax = InvestmentsHelper.GetInvestmentTax(investments);

            var additionalTax = Constants.GetAdditionalTax(additionalWarriors);

            return investmentTax + additionalTax;
        }

        protected override bool Execute()
        {
            var additionalTaxWarrioirs = Context.Units
                .Where(c => c.Status == CommandStatus.Complited &&
                            c.DomainId == Domain.Id &&
                            c.PositionDomainId == Domain.Id &&
                            c.Type == UnitCommandType.CollectTax)
                .Sum(c => c.Warriors);
            var getCoffers = GetTax(additionalTaxWarrioirs, Domain.Investments);

            var eventStoryResult = new EventJson(EventType.TaxCollection);
            FillEventOrganizationList(eventStoryResult, context, Domain, getCoffers);

            var dommainEventStories = eventStoryResult.Organizations.ToDictionary(
                o => o.Id,
                o => getCoffers / 100);
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }

        private void FillEventOrganizationList(EventJson eventStoryResult, ApplicationDbContext context, Domain organization,
            int allIncome, bool isMain = true)
        {
            var type = isMain
                ? EventParticipantType.Main
                : EventParticipantType.Suzerain;

            var suzerainId = organization.SuzerainId;
            var getCoffers = suzerainId == null
                ? allIncome
                : (int)Math.Round(allIncome * (1 - Constants.BaseVassalTax));

            var temp = new List<EventParticipantParameterChange>
            {
                EventJsonParametrChangeHelper.Create(
                    EventParticipantParameterType.Coffers, organization.Gold, organization.Gold + getCoffers
                )
            };
            eventStoryResult.AddEventOrganization(organization.Id, type, temp);

            organization.Gold += getCoffers;
            if (suzerainId == null)
                return;

            FillEventOrganizationList(eventStoryResult, context,
                    context.Domains.Single(o => o.Id == suzerainId),
                    allIncome - getCoffers,
                    false);
        }
    }
}
