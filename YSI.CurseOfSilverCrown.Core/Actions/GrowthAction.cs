using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Actions;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Event;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    public class GrowthAction : ActionBase
    {
        protected int ImportanceBase => 500;

        public EventStory EventStory { get; private set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; private set; }

        public GrowthAction(ApplicationDbContext context, Turn currentTurn, Command command) 
            : base(context, currentTurn, command)
        {
        }

        public override bool Execute()
        {
            var coffers = Command.Organization.Coffers;
            var warriors = Command.Organization.Warriors;

            var spentCoffers = Math.Min(coffers, Command.Coffers);
            var getWarriors = spentCoffers / WarriorParameters.Price;

            var newCoffers = coffers - spentCoffers;
            var newWarriors = warriors + getWarriors;

            Command.Organization.Coffers = newCoffers;
            Command.Organization.Warriors = newWarriors;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = enEventResultType.Growth,
                Organizations = new List<EventOrganization>
                {
                    new EventOrganization
                    {
                        Id = Command.Organization.Id,
                        EventOrganizationType = enEventOrganizationType.Main,
                        EventOrganizationChanges = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enEventParametrChange.Warrior,
                                Before = warriors,
                                After = newWarriors
                            },
                            new EventParametrChange
                            {
                                Type = enEventParametrChange.Coffers,
                                Before = coffers,
                                After = newCoffers
                            }
                        }

                    }
                }
            };

            EventStory = new EventStory
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = JsonConvert.SerializeObject(eventStoryResult)
            };

            OrganizationEventStories = new List<OrganizationEventStory>
            { 
                new OrganizationEventStory
                {
                    Organization = Command.Organization,
                    Importance = getWarriors * 50,
                    EventStory = EventStory
                }
            };

            return true;
        }
    }
}
