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
    public class MutinyAction
    {
        private Random _random = new Random();
        private Organization organization;
        private Turn currentTurn;

        private const int ImportanceBase = 5000;

        public EventStory EventStory { get; set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; set; }

        public MutinyAction(Organization organization, Turn currentTurn)
        {
            this.organization = organization;
            this.currentTurn = currentTurn;
        }

        internal bool Execute()
        {
            var coffers = organization.Coffers;
            var warrioirs = organization.Warriors;

            var newCoffers = Constants.AddRandom10(2500, _random.NextDouble());
            var newWarriors = Constants.AddRandom10(500, _random.NextDouble()) / 10;
            organization.Coffers = newCoffers;
            organization.Warriors = newWarriors;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = Enums.enEventResultType.Mutiny,
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
                            },
                            new EventParametrChange
                            {
                                Type = Enums.enEventParametrChange.Warrior,
                                Before = warrioirs,
                                After = newWarriors
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
                    Importance = ImportanceBase,
                    EventStory = EventStory
                }
            };

            return true;
        }

    }
}
