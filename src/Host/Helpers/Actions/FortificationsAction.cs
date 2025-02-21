using System;
using System.Collections.Generic;
using YAGO.World.Host.Database.Commands;
using YAGO.World.Host.Database.Events;
using YAGO.World.Host.Database.Turns;
using YAGO.World.Host.Helpers.Events;
using YAGO.World.Host.Infrastructure.Database;

namespace YAGO.World.Host.Helpers.Actions
{
    internal class FortificationsAction : CommandActionBase
    {
        protected int ImportanceBase => 500;

        protected override bool RemoveCommandeAfterUse => true;

        public FortificationsAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        public override bool CheckValidAction()
        {
            FixCoffersForAction();

            return Command.Type == CommandType.Fortifications &&
                Command.Gold > 0 &&
                Command.Status == CommandStatus.ReadyToMove;
        }

        protected override bool Execute()
        {
            var coffers = Command.Domain.Gold;
            var fortifications = Command.Domain.Fortifications;

            var spentCoffers = Math.Min(coffers, Command.Gold);
            var getFortifications = spentCoffers;

            var newCoffers = coffers - spentCoffers;
            var newFortifications = fortifications + getFortifications;

            Command.Domain.Gold = newCoffers;
            Command.Domain.Fortifications = newFortifications;

            var eventStoryResult = new EventJson();
            var eventOrganizationChanges = new List<EventParticipantParameterChange>
            {
                EventJsonParametrChangeHelper.Create(EventParticipantParameterType.Fortifications, fortifications, newFortifications),
                EventJsonParametrChangeHelper.Create(EventParticipantParameterType.Coffers, coffers, newCoffers)
            };
            eventStoryResult.AddEventOrganization(Command.DomainId, EventParticipantType.Main, eventOrganizationChanges);

            var thresholdImportance = EventHelper.GetThresholdImportance(fortifications, newFortifications);

            var dommainEventStories = new Dictionary<int, int>
            {
                { Command.Domain.Id, spentCoffers + thresholdImportance }
            };
            var eventType = GetFortificationsEventResultType(thresholdImportance);
            CreateEventStory(eventStoryResult, dommainEventStories, eventType);

            return true;
        }

        private EventType GetFortificationsEventResultType(int thresholdImportance)
        {
            if (thresholdImportance < 3000)
                return EventType.Fortifications;
            else if (thresholdImportance < 6000)
                return EventType.FortificationsLevelI;
            else if (thresholdImportance < 12000)
                return EventType.FortificationsLevelII;
            else if (thresholdImportance < 35000)
                return EventType.FortificationsLevelIII;
            else if (thresholdImportance < 100000)
                return EventType.FortificationsLevelIV;
            else
                return EventType.FortificationsLevelV;
        }
    }
}
