using System.Collections.Generic;
using YAGO.World.Host.Database;
using YAGO.World.Host.Database.Commands;
using YAGO.World.Host.Database.Events;
using YAGO.World.Host.Database.Turns;
using YAGO.World.Host.Helpers;

namespace YAGO.World.Host.Helpers.Actions
{
    internal class VassalTransferAction : CommandActionBase
    {
        protected int ImportanceBase => 500;

        protected override bool RemoveCommandeAfterUse => true;

        public VassalTransferAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        public override bool CheckValidAction()
        {
            if (Command.TargetDomainId == null)
                return false;
            var targetDomain = Context.Domains.Find(Command.TargetDomainId.Value);

            return Command.Type == CommandType.VassalTransfer &&
                (targetDomain.SuzerainId == Domain.Id ||
                 targetDomain.Id == Domain.Id && Domain.SuzerainId == null) &&
                Command.Target2DomainId != null &&
                Command.Target2DomainId != Domain.Id &&
                Command.Status == CommandStatus.ReadyToMove;
        }

        protected override bool Execute()
        {
            if (Command.TargetDomainId == Command.Target2DomainId && Command.TargetDomainId == Command.DomainId)
                return false;

            var vassal = Command.Target;
            var newSuzerain = Command.Target2;

            var isLiberation = vassal.Id == newSuzerain.Id;
            if (isLiberation)
            {
                vassal.Suzerain = null;
                vassal.SuzerainId = null;
                vassal.TurnOfDefeat = int.MinValue;
            }
            else
            {
                vassal.Suzerain = newSuzerain;
                vassal.SuzerainId = newSuzerain.Id;

                var suzerain = newSuzerain;
                while (suzerain.SuzerainId != null)
                {
                    if (suzerain.SuzerainId == vassal.Id)
                    {
                        suzerain.SuzerainId = null;
                        suzerain.Suzerain = null;
                        suzerain.TurnOfDefeat = int.MinValue;
                        Context.Update(suzerain);
                        break;
                    }
                    suzerain = Context.Domains.Find(suzerain.SuzerainId);
                }
            }
            Context.Update(vassal);

            var eventStoryResult = new EventJson();
            eventStoryResult.AddEventOrganization(Command.DomainId, EventParticipantType.Main, new List<EventParticipantParameterChange>());
            eventStoryResult.AddEventOrganization(Command.TargetDomainId.Value, EventParticipantType.Vasal, new List<EventParticipantParameterChange>());
            eventStoryResult.AddEventOrganization(Command.Target2DomainId.Value, EventParticipantType.Suzerain, new List<EventParticipantParameterChange>());

            var importance = DomainHelper.GetImprotanceDoamin(Context, Command.TargetDomainId.Value);
            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, importance * 2 }
            };
            if (!dommainEventStories.ContainsKey(Command.TargetDomainId.Value))
                dommainEventStories.Add(Command.TargetDomainId.Value, importance / 2);
            if (!dommainEventStories.ContainsKey(Command.Target2DomainId.Value))
                dommainEventStories.Add(Command.Target2DomainId.Value, importance);

            var type = isLiberation
                    ? EventType.Liberation
                    : Command.DomainId == vassal.Id
                        ? EventType.VoluntaryOath
                        : EventType.ChangeSuzerain; 
            CreateEventStory(eventStoryResult, dommainEventStories, type);

            return true;
        }
    }
}
