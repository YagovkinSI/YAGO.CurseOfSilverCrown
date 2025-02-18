using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Host.Database;
using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Database.Events;
using YAGO.World.Host.Database.Turns;
using YAGO.World.Host.Helpers;
using YAGO.World.Host.Helpers.Events;
using YAGO.World.Host.Parameters;

namespace YAGO.World.Host.Helpers.Actions
{
    internal class MaintenanceAction : DomainActionBase
    {
        public MaintenanceAction(ApplicationDbContext context, Turn currentTurn, Domain organization)
            : base(context, currentTurn, organization)
        {
        }

        public override bool CheckValidAction()
        {
            return true;
        }

        protected override bool Execute()
        {
            var coffers = Domain.Gold;
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
            Domain.Gold = newCoffers;
            DomainHelper.SetWarriorCount(Context, Domain.Id, newWarriors);

            var eventStoryResult = new EventJson();
            var temp = new List<EventParticipantParameterChange>
            {
                EventJsonParametrChangeHelper.Create(EventParticipantParameterType.Coffers, coffers, newCoffers)
            };
            eventStoryResult.AddEventOrganization(Domain.Id, EventParticipantType.Main, temp);

            if (spendWarriors > 0)
            {
                eventStoryResult.Organizations.First().EventOrganizationChanges.Add(
                    EventJsonParametrChangeHelper.Create(EventParticipantParameterType.Warrior, warrioirs, newWarriors)
                );
            }

            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, spendCoffers / 100 + spendWarriors * WarriorParameters.Price * 2 }
            };
            CreateEventStory(eventStoryResult, dommainEventStories, EventType.Maintenance);

            return true;
        }

    }
}
