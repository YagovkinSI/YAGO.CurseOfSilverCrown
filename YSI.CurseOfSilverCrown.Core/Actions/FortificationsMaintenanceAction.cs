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

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal class FortificationsMaintenanceAction
    {
        private Organization organization;
        private Turn currentTurn;

        public EventStory EventStory { get; set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; set; }

        public FortificationsMaintenanceAction(Organization organization, Turn currentTurn)
        {
            this.organization = organization;
            this.currentTurn = currentTurn;
        }

        internal bool Execute()
        {
            var coffers = organization.Coffers;
            var warrioirs = organization.Warriors;

            var spendCoffers = 0;
            spendCoffers += (int)Math.Round(organization.Fortifications * FortificationsParameters.MaintenancePercent);
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
            organization.Coffers = newCoffers;
            organization.Warriors = newWarriors;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = enEventResultType.FortificationsMaintenance,
                Organizations = new List<EventOrganization>
                {
                    new EventOrganization
                    {
                        Id = organization.Id,
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
                TurnId = currentTurn.Id,
                EventStoryJson = JsonConvert.SerializeObject(eventStoryResult)
            };

            OrganizationEventStories = new List<OrganizationEventStory>
            {
                new OrganizationEventStory
                {
                    Organization = organization,
                    Importance = spendWarriors * 5,
                    EventStory = EventStory
                }
            };

            return true;
        }

    }
}
