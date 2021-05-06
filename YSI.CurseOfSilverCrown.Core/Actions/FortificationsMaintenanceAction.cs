using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Event;
using YSI.CurseOfSilverCrown.Core.Database.EF;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal class FortificationsMaintenanceAction : ActionBase
    {
        public EventStory EventStory { get; set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; set; }

        public FortificationsMaintenanceAction(ApplicationDbContext context, Turn currentTurn, Organization organization)
            : base(context, currentTurn, organization)
        {
        }

        public override bool Execute()
        {
            var coffers = Organization.Coffers;
            var warrioirs = Organization.Warriors;

            var spendCoffers = 0;
            spendCoffers += (int)Math.Round(Organization.Fortifications * FortificationsParameters.MaintenancePercent);
            var spendWarriors = 0;

            if (spendCoffers > coffers)
            {
                spendWarriors = (int)Math.Ceiling((spendCoffers - coffers) / (double)WarriorParameters.Maintenance);
                if (spendWarriors > warrioirs)
                    spendWarriors = warrioirs;
                spendCoffers -= spendWarriors * WarriorParameters.Maintenance;
            }

            var newCoffers = coffers - spendCoffers;
            var newWarriors = warrioirs - spendWarriors;
            Organization.Coffers = newCoffers;
            Organization.Warriors = newWarriors;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = enEventResultType.FortificationsMaintenance,
                Organizations = new List<EventOrganization>
                {
                    new EventOrganization
                    {
                        Id = Organization.Id,
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

            if (spendWarriors > 0)
                eventStoryResult.Organizations.First().EventOrganizationChanges.Add(
                    new EventParametrChange
                    {
                        Type = enEventParametrChange.Warrior,
                        Before = warrioirs,
                        After = newWarriors
                    }
                    );

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
                    Importance = spendWarriors * 5,
                    EventStory = EventStory
                }
            };

            return true;
        }

    }
}
