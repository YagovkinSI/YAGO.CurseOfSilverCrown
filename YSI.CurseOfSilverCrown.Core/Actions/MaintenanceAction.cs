using System;
using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.Domains;
using YSI.CurseOfSilverCrown.Core.MainModels.EventDomains;
using YSI.CurseOfSilverCrown.Core.MainModels.Events;
using YSI.CurseOfSilverCrown.Core.MainModels.Turns;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Actions
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

            var eventStoryResult = new EventJson(enEventType.Maintenance);
            var temp = new List<EventJsonParametrChange>
            {
                EventJsonParametrChangeHelper.Create(enEventParameterType.Coffers, coffers, newCoffers)
            };
            eventStoryResult.AddEventOrganization(Domain.Id, enEventDomainType.Main, temp);

            if (spendWarriors > 0)
            {
                eventStoryResult.Organizations.First().EventOrganizationChanges.Add(
                    EventJsonParametrChangeHelper.Create(enEventParameterType.Warrior, warrioirs, newWarriors)
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
