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

        public EventStory EventStory { get; private set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; private set; }

        public FortificationsAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        public override bool Execute()
        {
            var coffers = Command.Organization.Coffers;
            var fortifications = Command.Organization.Fortifications;

            var spentCoffers = Math.Min(coffers, Command.Coffers);
            var getFortifications = spentCoffers;

            var newCoffers = coffers - spentCoffers;
            var newFortifications = fortifications + getFortifications;

            Command.Organization.Coffers = newCoffers;
            Command.Organization.Fortifications = newFortifications;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = enEventResultType.Fortifications,
                Organizations = new List<EventOrganization>
                {
                    new EventOrganization
                    {
                        Id = Command.Organization.Id,
                        EventOrganizationType = enEventOrganizationType.Main,
                        EventOrganizationChanges = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enEventParametrChange.Fortifications,
                                Before = FortificationsHelper.GetDefencePercent(fortifications),
                                After = FortificationsHelper.GetDefencePercent(newFortifications),
                            },
                            new EventParametrChange
                            {
                                Type = enEventParametrChange.Coffers,
                                Before = coffers,
                                After = newCoffers
                            }
                        }

                    }
                }
            };

            EventStory = new EventStory
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = JsonConvert.SerializeObject(eventStoryResult)
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
