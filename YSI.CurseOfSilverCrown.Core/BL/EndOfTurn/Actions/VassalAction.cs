using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.BL.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Constants;
using YSI.CurseOfSilverCrown.Core.Event;

namespace YSI.CurseOfSilverCrown.Core.BL.EndOfTurn.Actions
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

            var realStep = (int)Math.Round(Constants.MinTax * (1 - Constants.BaseVassalTax));
            var newVassalCoffers = startVassalCoffers - realStep;
            var newSuzerainPower = startSuzerainCoffers + realStep;

            organization.Coffers = newVassalCoffers;
            suzerain.Coffers = newSuzerainPower;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = enEventResultType.VasalTax,
                Organizations = new List<EventOrganization>
                {
                    new EventOrganization
                    {
                        Id = organization.Id,
                        EventOrganizationType = enEventOrganizationType.Vasal,
                        EventOrganizationChanges = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enEventParametrChange.Coffers,
                                Before = startVassalCoffers,
                                After = newVassalCoffers
                            }
                        }
                    },
                    new EventOrganization
                    {
                        Id = organization.Suzerain.Id,
                        EventOrganizationType = enEventOrganizationType.Suzerain,
                        EventOrganizationChanges = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enEventParametrChange.Coffers,
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
