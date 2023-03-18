using System;
using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Commands;
using YSI.CurseOfSilverCrown.Core.Database.Events;
using YSI.CurseOfSilverCrown.Core.Database.Turns;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Helpers.Events;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Actions
{
    internal class GrowthAction : CommandActionBase
    {
        protected int ImportanceBase => 500;

        protected override bool RemoveCommandeAfterUse => true;

        public GrowthAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        public override bool CheckValidAction()
        {
            FixCoffersForAction();

            return Command.Type == CommandType.Growth &&
                Command.Gold >= WarriorParameters.Price &&
                Command.Status == CommandStatus.ReadyToMove;
        }

        protected override bool Execute()
        {
            var coffers = Command.Domain.Gold;
            var warriors = DomainHelper.GetWarriorCount(Context, Command.Domain.Id);

            var spentCoffers = Math.Min(coffers, Command.Gold);
            var getWarriors = spentCoffers / WarriorParameters.Price;

            var newCoffers = coffers - spentCoffers;
            var newWarriors = warriors + getWarriors;

            Command.Domain.Gold = newCoffers;
            DomainHelper.SetWarriorCount(Context, Command.Domain.Id, newWarriors);

            var eventStoryResult = new EventJson(EventType.Growth);
            var temp = new List<EventParticipantParameterChange>
            {
                EventJsonParametrChangeHelper.Create(EventParticipantParameterType.Warrior, warriors, newWarriors),
                EventJsonParametrChangeHelper.Create(EventParticipantParameterType.Coffers, coffers, newCoffers)
            };
            eventStoryResult.AddEventOrganization(Command.DomainId, EventParticipantType.Main, temp);

            var thresholdImportance = EventHelper.GetThresholdImportance(warriors, newWarriors);
            eventStoryResult.EventResultType = GetGrowthEventResultType(thresholdImportance);
            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, spentCoffers + thresholdImportance * WarriorParameters.Price }
            };
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }

        private EventType GetGrowthEventResultType(int thresholdImportance)
        {
            if (thresholdImportance < 100)
                return EventType.Growth;
            else if (thresholdImportance < 300)
                return EventType.GrowthLevelI;
            else if (thresholdImportance < 1000)
                return EventType.GrowthLevelII;
            else if (thresholdImportance < 3000)
                return EventType.GrowthLevelIII;
            else if (thresholdImportance < 10000)
                return EventType.GrowthLevelIV;
            else
                return EventType.GrowthLevelV;
        }
    }
}
