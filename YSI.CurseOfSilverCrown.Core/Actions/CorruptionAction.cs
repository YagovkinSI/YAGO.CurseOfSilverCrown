using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Event;
using YSI.CurseOfSilverCrown.Core.Database.EF;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal class CorruptionAction : ActionBase
    {
        public CorruptionAction(ApplicationDbContext context, Turn currentTurn, Domain organization)
            : base(context, currentTurn, organization)
        {
        }

        protected override bool Execute()
        {
            var corruptionLevel = Constants.GetCorruptionLevel(Organization.User);

            var list = new List<EventParametrChange>();
            var importance = 0;

            var coffers = Organization.Coffers;
            if (coffers > CoffersParameters.StartCount * 1.1)
            {
                var maxCoffersDecrease = coffers - RandomHelper.AddRandom(CoffersParameters.StartCount, roundRequest: -1);
                var coffersDecrease = corruptionLevel == 100
                    ? maxCoffersDecrease
                    : (int)Math.Round(maxCoffersDecrease * (corruptionLevel / 100.0));
                var newCoffers = coffers - coffersDecrease;
                Organization.Coffers = newCoffers;
                var eventParametrChange = new EventParametrChange
                {
                    Type = enActionParameter.Coffers,
                    Before = coffers,
                    After = newCoffers
                };
                importance += newCoffers / 5;
                list.Add(eventParametrChange);
            }

            var warriors = Organization.Warriors;
            if (warriors > WarriorParameters.StartCount * 1.1)
            {
                var maxWarriorsDecrease = warriors - RandomHelper.AddRandom(WarriorParameters.StartCount);
                var warriorsDecrease = corruptionLevel == 100
                    ? maxWarriorsDecrease
                    : (int)Math.Round(maxWarriorsDecrease * (corruptionLevel / 100.0));
                var newWarriors = warriors - warriorsDecrease;
                Organization.Warriors = newWarriors;
                var eventParametrChange = new EventParametrChange
                {
                    Type = enActionParameter.Warrior,
                    Before = warriors,
                    After = newWarriors
                };
                importance += warriorsDecrease * 5;
                list.Add(eventParametrChange);
            }

            var investments = Organization.Investments;
            if (investments > 0)
            {
                var investmentsDecrease = corruptionLevel == 100
                    ? investments
                    : (int)Math.Round(investments * (corruptionLevel / 100.0));
                var newInvestments = investments - investmentsDecrease;
                Organization.Investments = newInvestments;
                var eventParametrChange = new EventParametrChange
                {
                    Type = enActionParameter.Investments,
                    Before = investments,
                    After = newInvestments
                };
                importance += investmentsDecrease / 9;
                list.Add(eventParametrChange);
            }



            var fortifications = Organization.Fortifications;
            if (fortifications > FortificationsParameters.StartCount)
            {
                var fortificationsDecrease = corruptionLevel == 100
                    ? fortifications - FortificationsParameters.StartCount
                    : (int)Math.Round((fortifications - FortificationsParameters.StartCount) * (corruptionLevel / 100.0));
                var newFortifications = fortifications - fortificationsDecrease;
                Organization.Fortifications = newFortifications;
                var eventParametrChange = new EventParametrChange
                {
                    Type = enActionParameter.Fortifications,
                    Before = fortifications,
                    After = newFortifications
                };
                importance += fortificationsDecrease / 9;
                list.Add(eventParametrChange);
            }

            if (list.Count == 0)
                return false;

            var eventStoryResult = new EventStoryResult(enEventResultType.Corruption);
            eventStoryResult.AddEventOrganization(Organization, enEventOrganizationType.Main, list);

            EventStory = new EventStory
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = eventStoryResult.ToJson()
            };

            OrganizationEventStories = new List<DomainEventStory>
            {
                new DomainEventStory
                {
                    Domain = Organization,
                    Importance = importance,
                    EventStory = EventStory
                }
            };

            return true;
        }

    }
}
