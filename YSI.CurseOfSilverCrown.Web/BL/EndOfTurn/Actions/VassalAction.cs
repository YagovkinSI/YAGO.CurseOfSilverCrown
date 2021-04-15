using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions
{
    public class VassalAction
    {
        private Random _random = new Random();
        private Organization organization;
        private Turn currentTurn;

        private const int ImportanceBase = 500;

        public EventStory EventStory { get; set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; set; }

        public VassalAction(Organization organization, Turn currentTurn)
        {
            this.organization = organization;
            this.currentTurn = currentTurn;
        }

        internal bool Execute()
        {
            var suzerain = organization.Suzerain;

            var startVassalCoffers = organization.Coffers;
            var startSuzerainCoffers = suzerain.Coffers;

            var realStep = Constants.VassalTax;
            var newVassalCoffers = startVassalCoffers - realStep;
            var newSuzerainPower = startSuzerainCoffers + realStep;

            organization.Coffers = newVassalCoffers;
            suzerain.Coffers = newSuzerainPower;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = Enums.enEventResultType.VasalTax,
                Organizations = new List<EventOrganization>
                {
                    new EventOrganization
                    {
                        Id = organization.Id,
                        EventOrganizationType = Enums.enEventOrganizationType.Vasal,
                        EventOrganizationChanges = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = Enums.enEventParametrChange.Coffers,
                                Before = startVassalCoffers,
                                After = newVassalCoffers
                            }
                        }
                    },
                    new EventOrganization
                    {
                        Id = organization.Suzerain.Id,
                        EventOrganizationType = Enums.enEventOrganizationType.Suzerain,
                        EventOrganizationChanges = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = Enums.enEventParametrChange.Coffers,
                                Before = startSuzerainCoffers,
                                After = newSuzerainPower
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
                    Organization = organization,
                    Importance = 500,
                    EventStory = EventStory
                },
                new OrganizationEventStory
                {
                    Organization = organization.Suzerain,
                    Importance = 500,
                    EventStory = EventStory
                }
            };

            return true;
        }

    }
}
