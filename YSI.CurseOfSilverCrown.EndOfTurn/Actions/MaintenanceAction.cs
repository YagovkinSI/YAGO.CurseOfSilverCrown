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
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Core.Database.EF;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class MaintenanceAction : DomainActionBase
    {
        public MaintenanceAction(ApplicationDbContext context, Turn currentTurn, Domain organization)
            : base(context, currentTurn, organization)
        {
        }

        protected override bool Execute()
        {
            var coffers = Domain.Coffers;
            var warrioirs = DomainHelper.GetWarriorCount(Context, Domain.Id);

            var spendCoffers = 0;
            spendCoffers += warrioirs * WarriorParameters.Maintenance;
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
            Domain.Coffers = newCoffers;
            DomainHelper.SetWarriorCount(Context, Domain.Id, newWarriors);

            var eventStoryResult = new EventStoryResult(enEventResultType.Maintenance);
            var temp = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enActionParameter.Coffers,
                                Before = coffers,
                                After = newCoffers
                            }
                        };
            eventStoryResult.AddEventOrganization(Domain, enEventOrganizationType.Main, temp);

            if (spendWarriors > 0)
                eventStoryResult.Organizations.First().EventOrganizationChanges.Add(
                    new EventParametrChange
                    {
                        Type = enActionParameter.Warrior,
                        Before = warrioirs,
                        After = newWarriors
                    }
                    );

            EventStory = new EventStory
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = eventStoryResult.ToJson()
            };

            OrganizationEventStories = new List<DomainEventStory>
            {
                new DomainEventStory
                {
                    Domain = Domain,
                    Importance = spendWarriors * 5,
                    EventStory = EventStory
                }
            };

            return true;
        }

    }
}
