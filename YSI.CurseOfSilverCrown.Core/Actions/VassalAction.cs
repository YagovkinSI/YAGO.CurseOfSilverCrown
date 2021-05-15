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
using YSI.CurseOfSilverCrown.Core.Event;
using YSI.CurseOfSilverCrown.Core.Database.EF;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal class VassalAction : ActionBase
    {
        public VassalAction(ApplicationDbContext context, Turn currentTurn, Domain organization)
            : base(context, currentTurn, organization)
        {
        }

        protected override bool Execute()
        {
            var suzerain = Organization.Suzerain;

            var startVassalCoffers = Organization.Coffers;
            var startSuzerainCoffers = suzerain.Coffers;

            var realStep = (int)Math.Round(Constants.MinTax * (1 - Constants.BaseVassalTax));
            var newVassalCoffers = startVassalCoffers - realStep;
            var newSuzerainPower = startSuzerainCoffers + realStep;

            Organization.Coffers = newVassalCoffers;
            suzerain.Coffers = newSuzerainPower;

            var eventStoryResult = new EventStoryResult(enEventResultType.VasalTax);
            var temp = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enActionParameter.Coffers,
                                Before = startVassalCoffers,
                                After = newVassalCoffers
                            }
                        };
            eventStoryResult.AddEventOrganization(Organization, enEventOrganizationType.Vasal, temp);
            var temp2 = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enActionParameter.Coffers,
                                Before = startSuzerainCoffers,
                                After = newSuzerainPower
                            }
                        };
            eventStoryResult.AddEventOrganization(Organization.Suzerain, enEventOrganizationType.Suzerain, temp2);

            EventStory = new EventStory
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = eventStoryResult.ToJson()
            };

            OrganizationEventStories = new List<DomainEventStory>
            {
                new DomainEventStory
                {
                    Domain = Organization,
                    Importance = 500,
                    EventStory = EventStory
                },
                new DomainEventStory
                {
                    Domain = Organization.Suzerain,
                    Importance = 500,
                    EventStory = EventStory
                }
            };

            return true;
        }

    }
}
