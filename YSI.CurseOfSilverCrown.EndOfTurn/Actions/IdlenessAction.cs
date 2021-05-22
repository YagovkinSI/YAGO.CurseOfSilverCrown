using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class IdlenessAction : CommandActionBase
    {
        protected int ImportanceBase => 500;

        protected override bool RemoveCommandeAfterUse => true;

        public IdlenessAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        protected override bool CheckValidAction()
        {
            return Command.Type == enCommandType.Idleness &&
                Command.Coffers > 0 &&
                Command.Status == enCommandStatus.ReadyToRun;
        }

        protected override bool Execute()
        {
            var coffers = Command.Domain.Coffers;
            var spendCoffers = Command.Coffers;
            var newCoffers = coffers - spendCoffers;
            Command.Domain.Coffers = newCoffers;

            var eventStoryResult = new EventStoryResult(enEventResultType.Idleness);
            var trmp = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enActionParameter.Coffers,
                                Before = coffers,
                                After = newCoffers
                            }
                        };
            eventStoryResult.AddEventOrganization(Command.DomainId, enEventOrganizationType.Main, trmp);

            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, spendCoffers / 10 }
            };
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }
    }
}
