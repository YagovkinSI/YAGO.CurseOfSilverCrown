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
    public class CorruptionAction
    {
        private Random _random = new Random();
        private Organization organization;
        private Turn currentTurn;

        private const int ImportanceBase = 500;

        public EventStory EventStory { get; set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; set; }

        public CorruptionAction(Organization organization, Turn currentTurn)
        {
            this.organization = organization;
            this.currentTurn = currentTurn;
        }

        internal bool Execute()
        {
            var corruptionLevel = Constants.GetCorruptionLevel(organization.User);

            var list = new List<EventParametrChange>();
            var importance = 0;

            var coffers = organization.Coffers;
            if (coffers > Constants.StartCoffers * 1.1)
            {
                var maxCoffersDecrease = coffers - Constants.AddRandom10(Constants.StartCoffers, (new Random()).NextDouble());
                var coffersDecrease = corruptionLevel == 100
                    ? maxCoffersDecrease
                    : (int)Math.Round(maxCoffersDecrease * (corruptionLevel / 100.0));
                var newCoffers = coffers - coffersDecrease;
                organization.Coffers = newCoffers;
                var eventParametrChange = new EventParametrChange
                {
                    Type = Enums.enEventParametrChange.Coffers,
                    Before = coffers,
                    After = newCoffers
                };
                importance += newCoffers / 3;
                list.Add(eventParametrChange);
            }

            var warriors = organization.Warriors;
            if (warriors > Constants.StartWarriors * 1.1)
            {
                var maxWarriorsDecrease = warriors - Constants.AddRandom10(Constants.StartWarriors * 10, (new Random()).NextDouble()) / 10;
                var warriorsDecrease = corruptionLevel == 100
                    ? maxWarriorsDecrease
                    : (int)Math.Round(maxWarriorsDecrease * (corruptionLevel / 100.0));
                var newWarriors = warriors - warriorsDecrease;
                organization.Warriors = newWarriors;
                var eventParametrChange = new EventParametrChange
                {
                    Type = Enums.enEventParametrChange.Warrior,
                    Before = warriors,
                    After = newWarriors
                };
                importance += warriorsDecrease * 10;
                list.Add(eventParametrChange);
            }

            var investments = organization.Investments;
            if (investments > 0)
            {
                var investmentsDecrease = corruptionLevel == 100
                    ? investments
                    : (int)Math.Round(investments * (corruptionLevel / 100.0));
                var newInvestments = investments - investmentsDecrease;
                organization.Investments = newInvestments;
                var eventParametrChange = new EventParametrChange
                {
                    Type = Enums.enEventParametrChange.Investments,
                    Before = investments,
                    After = newInvestments
                };
                importance += investmentsDecrease / 6;
                list.Add(eventParametrChange);
            }

            if (list.Count == 0)
                return false;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = Enums.enEventResultType.Corruption,
                Organizations = new List<EventOrganization>
                {
                    new EventOrganization
                    {
                        Id = organization.Id,
                        EventOrganizationType = Enums.enEventOrganizationType.Main,
                        EventOrganizationChanges = list
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
                    Importance = importance,
                    EventStory = EventStory
                }
            };

            return true;
        }

    }
}
