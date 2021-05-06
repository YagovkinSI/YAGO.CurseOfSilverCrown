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
using YSI.CurseOfSilverCrown.Core.Database.EF;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal class MutinyAction : ActionBase
    {
        private const int ImportanceBase = 5000;

        public MutinyAction(ApplicationDbContext context, Turn currentTurn, Organization organization)
            : base(context, currentTurn, organization)
        {
        }

        protected override bool Execute()
        {
            var coffers = Organization.Coffers;
            var warrioirs = Organization.Warriors;

            var newCoffers = RandomHelper.AddRandom(CoffersParameters.StartCount, roundRequest: -1);
            var newWarriors = RandomHelper.AddRandom(WarriorParameters.StartCount);
            Organization.Coffers = newCoffers;
            Organization.Warriors = newWarriors;

            var eventStoryResult = new EventStoryResult(enEventResultType.Mutiny);
            var temp = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enActionParameter.Coffers,
                                Before = coffers,
                                After = newCoffers
                            },
                            new EventParametrChange
                            {
                                Type = enActionParameter.Warrior,
                                Before = warrioirs,
                                After = newWarriors
                            }
                        };
            eventStoryResult.AddEventOrganization(Organization, enEventOrganizationType.Main, temp);

            EventStory = new EventStory
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = eventStoryResult.ToJson()
            };

            OrganizationEventStories = new List<OrganizationEventStory>
            {
                new OrganizationEventStory
                {
                    Organization = Organization,
                    Importance = ImportanceBase,
                    EventStory = EventStory
                }
            };

            return true;
        }

    }
}
