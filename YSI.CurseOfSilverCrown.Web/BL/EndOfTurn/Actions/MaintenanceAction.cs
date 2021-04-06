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
    public class MaintenanceAction
    {
        private Random _random = new Random();
        private Organization organization;
        private Turn currentTurn;

        private const int DefaultMaintenance = 5000;

        private const int ImportanceBase = 500;

        public EventStory EventStory { get; set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; set; }

        public MaintenanceAction(Organization organization, Turn currentTurn)
        {
            this.organization = organization;
            this.currentTurn = currentTurn;
        }

        internal bool Execute()
        {
            var coffers = organization.Coffers;
            var warrioirs = organization.Warriors;

            var spendCoffers = DefaultMaintenance - 500 + (int)Math.Round(_random.NextDouble() * 100) * 10;
            spendCoffers += organization.Warriors * 20;
            var spendWarriors = 0;

            if (spendCoffers > coffers)
            {
                spendWarriors = (int)Math.Ceiling((coffers - spendCoffers) / 20.0);
                if (spendWarriors > warrioirs)
                    spendWarriors = warrioirs;
                spendCoffers -= spendWarriors * 20;
            }

            var newCoffers = coffers - spendCoffers;
            var newWarriors = warrioirs - spendWarriors;
            organization.Coffers = newCoffers;
            organization.Warriors = newWarriors;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = Enums.enEventResultType.Maintenance,
                Organizations = new List<EventOrganization>
                {
                    new EventOrganization
                    {
                        Id = organization.Id,
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

            if (spendWarriors > 0)
                eventStoryResult.Organizations.First().EventOrganizationChanges.Add(
                    new EventParametrChange
                    {
                        Type = Enums.enEventParametrChange.Warrior,
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
                    Importance = spendCoffers / 10,
                    EventStory = EventStory
                }
            };

            return true;
        }

    }
}
