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

        private const double DefaultTax = 0.02;

        private const int ImportanceBase = 13 * 2;

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

            var startVassalPower = organization.Warriors;
            var startSuzerainPower = suzerain.Warriors;

            var realStep = (int)Math.Round((_random.NextDouble() / 2 + 0.75) * startVassalPower * DefaultTax);
            var newVassalPower = startVassalPower - realStep;
            organization.Warriors = newVassalPower;

            var income = startSuzerainPower > 2 * startVassalPower
                ? startVassalPower / startSuzerainPower * realStep
                : realStep;
            var newSuzerainPower = startSuzerainPower + income;
            suzerain.Warriors = newSuzerainPower;

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
                                Type = Enums.enEventParametrChange.Warrior,
                                Before = startVassalPower / 2000,
                                After = newVassalPower / 2000
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
                                Type = Enums.enEventParametrChange.Warrior,
                                Before = startSuzerainPower / 2000,
                                After = newSuzerainPower / 2000
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
                    Importance = ImportanceBase * Math.Abs(newVassalPower - startVassalPower) / 2000,
                    EventStory = EventStory
                },
                new OrganizationEventStory
                {
                    Organization = organization.Suzerain,
                    Importance = ImportanceBase * Math.Abs(newVassalPower - startVassalPower) / 2000,
                    EventStory = EventStory
                }
            };

            // Result = $"Из-за вассального налога вы сокращаете воинов - {(startVassalPower - newVassalPower) / 2000}. Теперь у вас войнов - {newVassalPower / 2000}."
            // Result = $"На налоги от вассаала {organization.Name} вы нанимаете воинов - {(newSuzerainPower - startSuzerainPower) / 2000}.  Теперь у вас войнов - {newSuzerainPower / 2000}"

            return true;
        }

    }
}
