using System;
using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
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
                EventParametrChangeHelper.Create(enActionParameter.Coffers, coffers, newCoffers)
            };
            eventStoryResult.AddEventOrganization(Domain.Id, enEventOrganizationType.Main, temp);

            if (spendWarriors > 0)
            {
                eventStoryResult.Organizations.First().EventOrganizationChanges.Add(
                    EventParametrChangeHelper.Create(enActionParameter.Warrior, warrioirs, newWarriors)
                );
            }

            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, spendCoffers / 100 + spendWarriors * WarriorParameters.Price * 2 }
            };
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }

    }
}
