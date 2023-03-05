using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Commands;
using YSI.CurseOfSilverCrown.Core.Database.Events;
using YSI.CurseOfSilverCrown.Core.Database.Turns;
using YSI.CurseOfSilverCrown.Core.Helpers.Events;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Actions
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

            return Command.Type == CommandType.GoldTransfer &&
                Command.Coffers > 0 &&
                Command.TargetDomainId != null &&
                Command.TargetDomainId != Command.DomainId &&
                Command.Status == CommandStatus.ReadyToMove;
        }

        protected override bool Execute()
        {
            var coffers = Command.Domain.Coffers;
            var newCoffers = Command.Domain.Coffers - Command.Coffers;
            Command.Domain.Coffers = newCoffers;

            var targetCoffers = Command.Target.Coffers;
            var targetNewCoffers = Command.Target.Coffers + Command.Coffers;
            Command.Target.Coffers = targetNewCoffers;

            var type = EventType.GoldTransfer;
            var eventStoryResult = new EventJson(type);
            var temp1 = new List<EventParticipantParameterChange>
            {
                EventJsonParametrChangeHelper.Create(EventParticipantParameterType.Coffers, coffers, newCoffers)
            };
            eventStoryResult.AddEventOrganization(Command.DomainId, EventParticipantType.Main, temp1);
            var temp2 = new List<EventParticipantParameterChange>
            {
                EventJsonParametrChangeHelper.Create(EventParticipantParameterType.Coffers, targetCoffers, targetNewCoffers)
            };
            eventStoryResult.AddEventOrganization(Command.TargetDomainId.Value, EventParticipantType.Target, temp2);

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
