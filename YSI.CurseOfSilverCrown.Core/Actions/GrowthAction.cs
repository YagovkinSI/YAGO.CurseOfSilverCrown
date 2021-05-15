using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Event;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal class GrowthAction : ActionBase
    {
        protected int ImportanceBase => 500;

        public GrowthAction(ApplicationDbContext context, Turn currentTurn, Command command) 
            : base(context, currentTurn, command)
        {
        }

        protected override bool Execute()
        {
            var coffers = Command.Domain.Coffers;
            var warriors = Command.Domain.Warriors;

            var spentCoffers = Math.Min(coffers, Command.Coffers);
            var getWarriors = spentCoffers / WarriorParameters.Price;

            var newCoffers = coffers - spentCoffers;
            var newWarriors = warriors + getWarriors;

            Command.Domain.Coffers = newCoffers;
            Command.Domain.Warriors = newWarriors;

            var eventStoryResult = new EventStoryResult(enEventResultType.Growth);
            var temp = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enActionParameter.Warrior,
                                Before = warriors,
                                After = newWarriors
                            },
                            new EventParametrChange
                            {
                                Type = enActionParameter.Coffers,
                                Before = coffers,
                                After = newCoffers
                            }
                        };
            eventStoryResult.AddEventOrganization(Command.Domain, enEventOrganizationType.Main, temp);

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
                    Importance = getWarriors * 50,
                    EventStory = EventStory
                }
            };

            return true;
        }
    }
}
