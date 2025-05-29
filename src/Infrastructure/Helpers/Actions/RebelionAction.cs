using System.Collections.Generic;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Commands;
using YAGO.World.Infrastructure.Database.Models.Events;
using YAGO.World.Infrastructure.Database.Models.Turns;
using YAGO.World.Infrastructure.Helpers;

namespace YAGO.World.Infrastructure.Helpers.Actions
{
    internal class RebelionAction : CommandActionBase
    {
        protected override bool RemoveCommandeAfterUse => true;

        public RebelionAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        public override bool CheckValidAction()
        {
            return Command.Type == CommandType.Rebellion &&
                Domain.SuzerainId != null &&
                Domain.TurnOfDefeat + RebelionHelper.TurnCountWithoutRebelion < CurrentTurn.Id &&
                Command.Status == CommandStatus.ReadyToMove;
        }

        protected override bool Execute()
        {
            var domain = Context.Domains.Find(Domain.Id);
            var suzerainId = domain.SuzerainId.Value;
            domain.SuzerainId = null;
            domain.TurnOfDefeat = int.MinValue;
            Context.Update(domain);

            var eventStoryResult = new EventJson();
            eventStoryResult.AddEventOrganization(Domain.Id, EventParticipantType.Agressor, new List<EventParticipantParameterChange>());
            eventStoryResult.AddEventOrganization(suzerainId, EventParticipantType.Defender, new List<EventParticipantParameterChange>());

            var importance = DomainHelper.GetImprotanceDoamin(Context, Domain.Id);
            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, importance },
                { suzerainId, importance * 2 }
            };
            CreateEventStory(eventStoryResult, dommainEventStories, EventType.FastRebelionSuccess);

            return true;
        }
    }
}
