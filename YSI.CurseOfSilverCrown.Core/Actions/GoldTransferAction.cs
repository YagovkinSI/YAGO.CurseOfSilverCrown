using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.Commands;
using YSI.CurseOfSilverCrown.Core.MainModels.Commands.DomainCommands;
using YSI.CurseOfSilverCrown.Core.MainModels.EventDomains;
using YSI.CurseOfSilverCrown.Core.MainModels.Events;
using YSI.CurseOfSilverCrown.Core.MainModels.Turns;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal class GoldTransferAction : CommandActionBase
    {
        protected int ImportanceBase => 500;

        protected override bool RemoveCommandeAfterUse => true;

        public GoldTransferAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        public override bool CheckValidAction()
        {
            FixCoffersForAction();

            return Command.Type == enDomainCommandType.GoldTransfer &&
                Command.Coffers > 0 &&
                Command.TargetDomainId != null &&
                Command.TargetDomainId != Command.DomainId &&
                Command.Status == enCommandStatus.ReadyToMove;
        }

        protected override bool Execute()
        {
            var coffers = Command.Domain.Coffers;
            var newCoffers = Command.Domain.Coffers - Command.Coffers;
            Command.Domain.Coffers = newCoffers;

            var targetCoffers = Command.Target.Coffers;
            var targetNewCoffers = Command.Target.Coffers + Command.Coffers;
            Command.Target.Coffers = targetNewCoffers;

            var type = enEventType.GoldTransfer;
            var eventStoryResult = new EventJson(type);
            var temp1 = new List<EventJsonParametrChange>
            {
                EventParametrChangeHelper.Create(enEventParameterType.Coffers, coffers, newCoffers)
            };
            eventStoryResult.AddEventOrganization(Command.DomainId, enEventDomainType.Main, temp1);
            var temp2 = new List<EventJsonParametrChange>
            {
                EventParametrChangeHelper.Create(enEventParameterType.Coffers, targetCoffers, targetNewCoffers)
            };
            eventStoryResult.AddEventOrganization(Command.TargetDomainId.Value, enEventDomainType.Target, temp2);

            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, Command.Coffers },
                { Command.TargetDomainId.Value, Command.Coffers }
            };
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }
    }
}
