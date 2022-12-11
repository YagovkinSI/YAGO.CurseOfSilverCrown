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

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class FortificationsMaintenanceAction : DomainActionBase
    {
        public FortificationsMaintenanceAction(ApplicationDbContext context, Turn currentTurn, Domain organization)
            : base(context, currentTurn, organization)
        {
        }

        protected override bool CheckValidAction()
        {
            return true;
        }

        protected override bool Execute()
        {
            var coffers = Domain.Coffers;
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
            Domain.Coffers = newCoffers;
            DomainHelper.SetWarriorCount(Context, Domain.Id, newWarriors);

            var eventStoryResult = new EventStoryResult(enEventResultType.FortificationsMaintenance);
            var temp = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enActionParameter.Coffers,
                                Before = coffers,
                                After = newCoffers
                            }
                        };
            eventStoryResult.AddEventOrganization(Domain.Id, enEventOrganizationType.Main, temp);

            if (spendWarriors > 0)
            {
                eventStoryResult.Organizations.First().EventOrganizationChanges.Add(
                    new EventParametrChange
                    {
                        Type = enActionParameter.Warrior,
                        Before = warrioirs,
                        After = newWarriors
                    }
                    );
            }

            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, spendWarriors * 5 }
            };
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }

    }
}
