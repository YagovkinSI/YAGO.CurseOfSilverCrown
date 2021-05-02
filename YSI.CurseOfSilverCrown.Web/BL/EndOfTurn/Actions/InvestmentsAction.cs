using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Actions;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Event;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions
{
    public class InvestmentsAction : ActionBase
    {
        protected int ImportanceBase => 500;

        public EventStory EventStory { get; private set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; private set; }

        public InvestmentsAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        public override bool Execute()
        {
            var coffers = Command.Organization.Coffers;
            var investments = Command.Organization.Investments;

            var spentCoffers = Math.Min(coffers, Command.Coffers);
            var getInvestments = spentCoffers;

            var newCoffers = coffers - spentCoffers;
            var newInvestments = investments + getInvestments;

            Command.Organization.Coffers = newCoffers;
            Command.Organization.Investments = newInvestments;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = enEventResultType.Investments,
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
                                Type = enEventParametrChange.Investments,
                                Before = investments,
                                After = newInvestments
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
