using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.BL.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.Core.Constants;
using YSI.CurseOfSilverCrown.Core.Actions;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Event;

namespace YSI.CurseOfSilverCrown.Core.BL.EndOfTurn.Actions
{
    public class IdlenessAction : ActionBase
    {
        protected int ImportanceBase => 500;

        public EventStory EventStory { get; private set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; private set; }

        public IdlenessAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        public override bool Execute()
        {
            var coffers = Command.Organization.Coffers;
            var spendCoffers = Command.Coffers;
            var newCoffers = coffers - spendCoffers;
            Command.Organization.Coffers = newCoffers;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = enEventResultType.Idleness,
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
                    Importance = spendCoffers / 10,
                    EventStory = EventStory
                }
            };                

            return true;
        }

        public static int GetOptimizedCoffers()
        {
            return RandomHelper.AddRandom(Constants.MinIdleness, roundRequest: -1);
        }

        public static bool IsOptimized(int coffers)
        {
            return coffers <= 3500;
        }
    }
}
