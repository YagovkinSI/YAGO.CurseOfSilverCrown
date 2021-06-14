using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Core.Commands;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class RebelionAction : CommandActionBase
    {
        protected override bool RemoveCommandeAfterUse => true;

        public RebelionAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        protected override bool CheckValidAction()
        {
            return Command.Type == enCommandType.Rebellion &&
                Domain.SuzerainId != null &&
                Domain.TurnOfDefeat + RebelionHelper.TurnCountWithoutRebelion < CurrentTurn.Id &&
                Command.Status == enCommandStatus.ReadyToMove;
        }

        protected override bool Execute()
        {
            var domain = Context.Domains.Find(Domain.Id);
            domain.SuzerainId = null;
            domain.TurnOfDefeat = int.MinValue;
            Context.Update(domain);

            var type = enEventResultType.FastRebelionSuccess;
            var eventStoryResult = new EventStoryResult(type);
            eventStoryResult.AddEventOrganization(Domain.Id, enEventOrganizationType.Agressor, new List<EventParametrChange>());
            eventStoryResult.AddEventOrganization(Domain.SuzerainId.Value, enEventOrganizationType.Defender, new List<EventParametrChange>());

            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, 5000 },
                { Domain.SuzerainId.Value, 5000 }
            };
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }
    }
}
