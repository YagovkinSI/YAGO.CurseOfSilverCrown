using System.Collections.Generic;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Commands;
using YAGO.World.Infrastructure.Database.Models.Events;
using YAGO.World.Infrastructure.Database.Models.Turns;
using YAGO.World.Infrastructure.Helpers.Events;

namespace YAGO.World.Infrastructure.Helpers.Actions
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
                Command.Gold > 0 &&
                Command.TargetDomainId != null &&
                Command.TargetDomainId != Command.DomainId &&
                Command.Status == CommandStatus.ReadyToMove;
        }

        protected override bool Execute()
        {
            var coffers = Command.Domain.Gold;
            var newCoffers = Command.Domain.Gold - Command.Gold;
            Command.Domain.Gold = newCoffers;

            var targetCoffers = Command.Target.Gold;
            var targetNewCoffers = Command.Target.Gold + Command.Gold;
            Command.Target.Gold = targetNewCoffers;

            var eventStoryResult = new EventJson();
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
                { Domain.Id, Command.Gold },
                { Command.TargetDomainId.Value, Command.Gold }
            };
            CreateEventStory(eventStoryResult, dommainEventStories, EventType.GoldTransfer);

            return true;
        }
    }
}
