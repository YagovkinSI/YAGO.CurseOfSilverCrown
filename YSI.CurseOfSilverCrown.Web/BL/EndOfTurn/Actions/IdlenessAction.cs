using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions
{
    public class IdlenessAction : BaseAction
    {
        protected override int ImportanceBase => 500;


        public IdlenessAction(Command command, Turn currentTurn)
            : base(command, currentTurn)
        {
        }

        public override bool Execute()
        {
            var coffers = _command.Organization.Coffers;
            var spendCoffers = _command.Coffers;
            var newCoffers = coffers - spendCoffers;
            _command.Organization.Coffers = newCoffers;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = Enums.enEventResultType.Idleness,
                Organizations = new List<EventOrganization>
                {
                    new EventOrganization
                    {
                        Id = _command.Organization.Id,
                        EventOrganizationType = Enums.enEventOrganizationType.Main,
                        EventOrganizationChanges = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = Enums.enEventParametrChange.Coffers,
                                Before = coffers,
                                After = newCoffers
                            }
                        }

                    }
                }
            };

            EventStory = new EventStory
            {
                TurnId = currentTurn.Id,
                EventStoryJson = JsonConvert.SerializeObject(eventStoryResult)
            };

            OrganizationEventStories = new List<OrganizationEventStory>
            {
                new OrganizationEventStory
                {
                    Organization = _command.Organization,
                    Importance = spendCoffers / 10,
                    EventStory = EventStory
                }
            };                

            return true;
        }

        internal static int GetOptimizedCoffers()
        {
            var random = new Random();
            return random.Next(10) * 100 + 5000;
        }

        internal static bool IsOptimized(int coffers)
        {
            return coffers < 6000;
        }
    }
}
