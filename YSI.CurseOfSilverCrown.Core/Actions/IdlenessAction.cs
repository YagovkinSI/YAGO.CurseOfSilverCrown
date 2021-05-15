using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Event;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal class IdlenessAction : ActionBase
    {
        protected int ImportanceBase => 500;

        public IdlenessAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
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
            eventStoryResult.AddEventOrganization(Command.Domain, enEventOrganizationType.Main, trmp);

            EventStory = new EventStory
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = eventStoryResult.ToJson()
            };

            OrganizationEventStories = new List<DomainEventStory>
            {
                new DomainEventStory
                {
                    Domain = Command.Domain,
                    Importance = spendCoffers / 10,
                    EventStory = EventStory
                }
            };                

            return true;
        }
    }
}
