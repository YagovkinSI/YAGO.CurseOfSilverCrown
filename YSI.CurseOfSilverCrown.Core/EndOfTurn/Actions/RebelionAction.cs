using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
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
            return Command.Type == enCommandType.Rebellion &&
                Domain.SuzerainId != null &&
                Domain.TurnOfDefeat + RebelionHelper.TurnCountWithoutRebelion < CurrentTurn.Id &&
                Command.Status == enCommandStatus.ReadyToMove;
        }

        protected override bool Execute()
        {
            var domain = Context.Domains.Find(Domain.Id);
            var suzerainId = domain.SuzerainId.Value;
            domain.SuzerainId = null;
            domain.TurnOfDefeat = int.MinValue;
            Context.Update(domain);

            var type = enEventResultType.FastRebelionSuccess;
            var eventStoryResult = new EventStoryResult(type);
            eventStoryResult.AddEventOrganization(Domain.Id, enEventOrganizationType.Agressor, new List<EventParametrChange>());
            eventStoryResult.AddEventOrganization(suzerainId, enEventOrganizationType.Defender, new List<EventParametrChange>());

            var importance = DomainHelper.GetImprotanceDoamin(Context, Domain.Id);
            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, importance },
                { suzerainId, importance * 2 }
            };
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }
    }
}
