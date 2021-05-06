using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Event;
using YSI.CurseOfSilverCrown.Core.Database.EF;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal class VassalAction : ActionBase
    {
        public EventStory EventStory { get; set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; set; }

        public VassalAction(ApplicationDbContext context, Turn currentTurn, Organization organization)
            : base(context, currentTurn, organization)
        {
        }

        public override bool Execute()
        {
            var suzerain = Organization.Suzerain;

            var startVassalCoffers = Organization.Coffers;
            var startSuzerainCoffers = suzerain.Coffers;

            var realStep = (int)Math.Round(Constants.MinTax * (1 - Constants.BaseVassalTax));
            var newVassalCoffers = startVassalCoffers - realStep;
            var newSuzerainPower = startSuzerainCoffers + realStep;

            Organization.Coffers = newVassalCoffers;
            suzerain.Coffers = newSuzerainPower;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = enEventResultType.VasalTax,
                Organizations = new List<EventOrganization>
                {
                    new EventOrganization
                    {
                        Id = Organization.Id,
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
                        Id = Organization.Suzerain.Id,
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
                TurnId = CurrentTurn.Id,
                EventStoryJson = JsonConvert.SerializeObject(eventStoryResult)
            };

            OrganizationEventStories = new List<OrganizationEventStory>
            {
                new OrganizationEventStory
                {
                    Organization = Organization,
                    Importance = 500,
                    EventStory = EventStory
                },
                new OrganizationEventStory
                {
                    Organization = Organization.Suzerain,
                    Importance = 500,
                    EventStory = EventStory
                }
            };

            return true;
        }

    }
}
