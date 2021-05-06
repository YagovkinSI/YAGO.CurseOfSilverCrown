using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Event;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal class InvestmentsAction : ActionBase
    {
        protected int ImportanceBase => 500;

        public InvestmentsAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        protected override bool Execute()
        {
            var coffers = Command.Organization.Coffers;
            var investments = Command.Organization.Investments;

            var spentCoffers = Math.Min(coffers, Command.Coffers);
            var getInvestments = spentCoffers;

            var newCoffers = coffers - spentCoffers;
            var newInvestments = investments + getInvestments;

            Command.Organization.Coffers = newCoffers;
            Command.Organization.Investments = newInvestments;

            var eventStoryResult = new EventStoryResult(enEventResultType.Investments);
            var trmp = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enActionParameter.Investments,
                                Before = investments,
                                After = newInvestments
                            },
                            new EventParametrChange
                            {
                                Type = enActionParameter.Coffers,
                                Before = coffers,
                                After = newCoffers
                            }
                        };
            eventStoryResult.AddEventOrganization(Command.Organization, enEventOrganizationType.Main, trmp);

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
