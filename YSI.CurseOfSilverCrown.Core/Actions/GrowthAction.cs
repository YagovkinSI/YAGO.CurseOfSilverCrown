using System;
using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.Commands;
using YSI.CurseOfSilverCrown.Core.MainModels.Commands.DomainCommands;
using YSI.CurseOfSilverCrown.Core.MainModels.EventDomains;
using YSI.CurseOfSilverCrown.Core.MainModels.Events;
using YSI.CurseOfSilverCrown.Core.MainModels.Turns;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Actions
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

            return Command.Type == enDomainCommandType.Growth &&
                Command.Coffers >= WarriorParameters.Price &&
                Command.Status == enCommandStatus.ReadyToMove;
        }

        protected override bool Execute()
        {
            var coffers = Command.Domain.Coffers;
            var warriors = DomainHelper.GetWarriorCount(Context, Command.Domain.Id);

            var spentCoffers = Math.Min(coffers, Command.Coffers);
            var getWarriors = spentCoffers / WarriorParameters.Price;

            var newCoffers = coffers - spentCoffers;
            var newWarriors = warriors + getWarriors;

            Command.Domain.Coffers = newCoffers;
            DomainHelper.SetWarriorCount(Context, Command.Domain.Id, newWarriors);

            var eventStoryResult = new EventJson(enEventType.Growth);
            var temp = new List<EventJsonParametrChange>
            {
                EventJsonParametrChangeHelper.Create(enEventParameterType.Warrior, warriors, newWarriors),
                EventJsonParametrChangeHelper.Create(enEventParameterType.Coffers, coffers, newCoffers)
            };
            eventStoryResult.AddEventOrganization(Command.DomainId, enEventDomainType.Main, temp);

            var thresholdImportance = EventHelper.GetThresholdImportance(warriors, newWarriors);
            eventStoryResult.EventResultType = GetGrowthEventResultType(thresholdImportance);
            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, spentCoffers + thresholdImportance * WarriorParameters.Price }
            };
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }

        private enEventType GetGrowthEventResultType(int thresholdImportance)
        {
            if (thresholdImportance < 100)
                return enEventType.Growth;
            else if (thresholdImportance < 300)
                return enEventType.GrowthLevelI;
            else if (thresholdImportance < 1000)
                return enEventType.GrowthLevelII;
            else if (thresholdImportance < 3000)
                return enEventType.GrowthLevelIII;
            else if (thresholdImportance < 10000)
                return enEventType.GrowthLevelIV;
            else
                return enEventType.GrowthLevelV;
        }
    }
}
