using System;
using System.Collections.Generic;
using YAGO.World.Host.Database.Commands;
using YAGO.World.Host.Database.Events;
using YAGO.World.Host.Database.Turns;
using YAGO.World.Host.Helpers.Events;
using YAGO.World.Host.Infrastructure.Database;

namespace YAGO.World.Host.Helpers.Actions
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
                Command.Gold > 0 &&
                Command.Status == CommandStatus.ReadyToMove;
        }

        protected override bool Execute()
        {
            var coffers = Command.Domain.Gold;
            var investments = Command.Domain.Investments;

            var spentCoffers = Math.Min(coffers, Command.Gold);
            var getInvestments = spentCoffers;

            var newCoffers = coffers - spentCoffers;
            var newInvestments = investments + getInvestments;

            Command.Domain.Gold = newCoffers;
            Command.Domain.Investments = newInvestments;

            var eventStoryResult = new EventJson();
            var trmp = new List<EventParticipantParameterChange>
            {
                EventJsonParametrChangeHelper.Create(EventParticipantParameterType.Investments, investments, newInvestments),
                EventJsonParametrChangeHelper.Create(EventParticipantParameterType.Coffers, coffers, newCoffers)
            };
            eventStoryResult.AddEventOrganization(Command.DomainId, EventParticipantType.Main, trmp);

            var thresholdImportance = EventHelper.GetThresholdImportance(investments, newInvestments);
            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, spentCoffers + thresholdImportance }
            };

            var eventType = GetInvestmentsEventResultType(thresholdImportance);
            CreateEventStory(eventStoryResult, dommainEventStories, eventType);

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
