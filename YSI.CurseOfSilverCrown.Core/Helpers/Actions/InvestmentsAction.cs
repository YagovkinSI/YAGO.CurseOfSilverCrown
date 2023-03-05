using System;
using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Commands;
using YSI.CurseOfSilverCrown.Core.Database.EventDomains;
using YSI.CurseOfSilverCrown.Core.Database.Events;
using YSI.CurseOfSilverCrown.Core.Database.Turns;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Actions
{
    internal class InvestmentsAction : CommandActionBase
    {
        protected int ImportanceBase => 500;

        protected override bool RemoveCommandeAfterUse => true;

        public InvestmentsAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        public override bool CheckValidAction()
        {
            FixCoffersForAction();

            return Command.Type == enDomainCommandType.Investments &&
                Command.Coffers > 0 &&
                Command.Status == enCommandStatus.ReadyToMove;
        }

        protected override bool Execute()
        {
            var coffers = Command.Domain.Coffers;
            var investments = Command.Domain.Investments;

            var spentCoffers = Math.Min(coffers, Command.Coffers);
            var getInvestments = spentCoffers;

            var newCoffers = coffers - spentCoffers;
            var newInvestments = investments + getInvestments;

            Command.Domain.Coffers = newCoffers;
            Command.Domain.Investments = newInvestments;

            var eventStoryResult = new EventJson(enEventType.Investments);
            var trmp = new List<EventJsonParametrChange>
            {
                EventJsonParametrChangeHelper.Create(enEventParameterType.Investments, investments, newInvestments),
                EventJsonParametrChangeHelper.Create(enEventParameterType.Coffers, coffers, newCoffers)
            };
            eventStoryResult.AddEventOrganization(Command.DomainId, enEventDomainType.Main, trmp);

            var thresholdImportance = EventHelper.GetThresholdImportance(investments, newInvestments);
            eventStoryResult.EventResultType = GetInvestmentsEventResultType(thresholdImportance);
            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, spentCoffers + thresholdImportance }
            };
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }

        private enEventType GetInvestmentsEventResultType(int thresholdImportance)
        {
            if (thresholdImportance < 60000)
                return enEventType.Investments;
            else if (thresholdImportance < 140000)
                return enEventType.InvestmentsLevelI;
            else if (thresholdImportance < 350000)
                return enEventType.InvestmentsLevelII;
            else if (thresholdImportance < 700000)
                return enEventType.InvestmentsLevelIII;
            else if (thresholdImportance < 1500000)
                return enEventType.InvestmentsLevelIV;
            else
                return enEventType.InvestmentsLevelV;
        }
    }
}
