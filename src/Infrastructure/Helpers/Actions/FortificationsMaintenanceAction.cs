using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Domains;
using YAGO.World.Infrastructure.Database.Models.Events;
using YAGO.World.Infrastructure.Database.Models.Turns;
using YAGO.World.Infrastructure.Helpers;
using YAGO.World.Infrastructure.Helpers.Events;
using YAGO.World.Infrastructure.Parameters;

namespace YAGO.World.Infrastructure.Helpers.Actions
{
    internal class FortificationsMaintenanceAction : DomainActionBase
    {
        public FortificationsMaintenanceAction(ApplicationDbContext context, Turn currentTurn, Organization organization)
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
            spendCoffers += (int)Math.Round(Domain.Fortifications * FortificationsParameters.MaintenancePercent);
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
            CreateEventStory(eventStoryResult, dommainEventStories, EventType.FortificationsMaintenance);

            return true;
        }

    }
}
