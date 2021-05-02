using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Event;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Actions
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
            if (coffers > CoffersParameters.StartCount * 1.1)
            {
                var maxCoffersDecrease = coffers - RandomHelper.AddRandom(CoffersParameters.StartCount, roundRequest: -1);
                var coffersDecrease = corruptionLevel == 100
                    ? maxCoffersDecrease
                    : (int)Math.Round(maxCoffersDecrease * (corruptionLevel / 100.0));
                var newCoffers = coffers - coffersDecrease;
                organization.Coffers = newCoffers;
                var eventParametrChange = new EventParametrChange
                {
                    Type = enEventParametrChange.Coffers,
                    Before = coffers,
                    After = newCoffers
                };
                importance += newCoffers / 3;
                list.Add(eventParametrChange);
            }

            var warriors = organization.Warriors;
            if (warriors > WarriorParameters.StartCount * 1.1)
            {
                var maxWarriorsDecrease = warriors - RandomHelper.AddRandom(WarriorParameters.StartCount);
                var warriorsDecrease = corruptionLevel == 100
                    ? maxWarriorsDecrease
                    : (int)Math.Round(maxWarriorsDecrease * (corruptionLevel / 100.0));
                var newWarriors = warriors - warriorsDecrease;
                organization.Warriors = newWarriors;
                var eventParametrChange = new EventParametrChange
                {
                    Type = enEventParametrChange.Warrior,
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
                    Type = enEventParametrChange.Investments,
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
                EventResultType = enEventResultType.Corruption,
                Organizations = new List<EventOrganization>
                {
                    new EventOrganization
                    {
                        Id = organization.Id,
                        EventOrganizationType = enEventOrganizationType.Main,
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
