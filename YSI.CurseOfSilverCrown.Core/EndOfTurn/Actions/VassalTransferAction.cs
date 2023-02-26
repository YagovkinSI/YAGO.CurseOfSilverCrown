using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.GameCommands;
using YSI.CurseOfSilverCrown.Core.MainModels.GameCommands.DomainCommands;
using YSI.CurseOfSilverCrown.Core.MainModels.GameEvent;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
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

            return Command.Type == enDomainCommandType.VassalTransfer &&
                (targetDomain.SuzerainId == Domain.Id ||
                 targetDomain.Id == Domain.Id && Domain.SuzerainId == null) &&
                Command.Target2DomainId != null &&
                Command.Target2DomainId != Domain.Id &&
                Command.Status == enCommandStatus.ReadyToMove;
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

            var type = isLiberation
                    ? enEventType.Liberation
                    : Command.DomainId == vassal.Id
                        ? enEventType.VoluntaryOath
                        : enEventType.ChangeSuzerain;
            var eventStoryResult = new EventStoryResult(type);
            eventStoryResult.AddEventOrganization(Command.DomainId, enEventDomainType.Main, new List<EventParametrChange>());
            eventStoryResult.AddEventOrganization(Command.TargetDomainId.Value, enEventDomainType.Vasal, new List<EventParametrChange>());
            eventStoryResult.AddEventOrganization(Command.Target2DomainId.Value, enEventDomainType.Suzerain, new List<EventParametrChange>());

            var importance = DomainHelper.GetImprotanceDoamin(Context, Domain.Id);
            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, importance }
            };
            if (!dommainEventStories.ContainsKey(Command.TargetDomainId.Value))
                dommainEventStories.Add(Command.TargetDomainId.Value, importance);
            if (!dommainEventStories.ContainsKey(Command.Target2DomainId.Value))
                dommainEventStories.Add(Command.Target2DomainId.Value, importance);
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }
    }
}
