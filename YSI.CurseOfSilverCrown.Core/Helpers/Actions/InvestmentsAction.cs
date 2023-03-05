using System;
using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Commands;
using YSI.CurseOfSilverCrown.Core.Database.Events;
using YSI.CurseOfSilverCrown.Core.Database.Turns;
using YSI.CurseOfSilverCrown.Core.Helpers.Events;

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

            return Command.Type == CommandType.Investments &&
                Command.Coffers > 0 &&
                Command.Status == CommandStatus.ReadyToMove;
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

            var eventStoryResult = new EventJson(EventType.Investments);
            var trmp = new List<EventParticipantParameterChange>
            {
                EventJsonParametrChangeHelper.Create(EventParticipantParameterType.Investments, investments, newInvestments),
                EventJsonParametrChangeHelper.Create(EventParticipantParameterType.Coffers, coffers, newCoffers)
            };
            eventStoryResult.AddEventOrganization(Command.DomainId, EventParticipantType.Main, trmp);

            var thresholdImportance = EventHelper.GetThresholdImportance(investments, newInvestments);
            eventStoryResult.EventResultType = GetInvestmentsEventResultType(thresholdImportance);
            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, spentCoffers + thresholdImportance }
            };
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }

        private EventType GetInvestmentsEventResultType(int thresholdImportance)
        {
            if (thresholdImportance < 60000)
                return EventType.Investments;
            else if (thresholdImportance < 140000)
                return EventType.InvestmentsLevelI;
            else if (thresholdImportance < 350000)
                return EventType.InvestmentsLevelII;
            else if (thresholdImportance < 700000)
                return EventType.InvestmentsLevelIII;
            else if (thresholdImportance < 1500000)
                return EventType.InvestmentsLevelIV;
            else
                return EventType.InvestmentsLevelV;
        }
    }
}
