using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Event;
using YSI.CurseOfSilverCrown.Core.Commands;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal class FortificationsAction : ActionBase
    {
        protected int ImportanceBase => 500;

        public FortificationsAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        protected override bool Execute()
        {
            var coffers = Command.Organization.Coffers;
            var fortifications = Command.Organization.Fortifications;

            var spentCoffers = Math.Min(coffers, Command.Coffers);
            var getFortifications = spentCoffers;

            var newCoffers = coffers - spentCoffers;
            var newFortifications = fortifications + getFortifications;

            Command.Organization.Coffers = newCoffers;
            Command.Organization.Fortifications = newFortifications;

            var eventStoryResult = new EventStoryResult(enEventResultType.Fortifications);
            var eventOrganizationChanges = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enActionParameter.Fortifications,
                                Before = FortificationsHelper.GetDefencePercent(fortifications),
                                After = FortificationsHelper.GetDefencePercent(newFortifications),
                            },
                            new EventParametrChange
                            {
                                Type = enActionParameter.Coffers,
                                Before = coffers,
                                After = newCoffers
                            }
                        };
            eventStoryResult.AddEventOrganization(Command.Organization, enEventOrganizationType.Main, eventOrganizationChanges);

            EventStory = new EventStory
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = eventStoryResult.ToJson()
            };

            OrganizationEventStories = new List<OrganizationEventStory>
            { 
                new OrganizationEventStory
                {
                    Organization = Command.Organization,
                    Importance = spentCoffers / 4,
                    EventStory = EventStory
                }
            };

            return true;
        }
    }
}
