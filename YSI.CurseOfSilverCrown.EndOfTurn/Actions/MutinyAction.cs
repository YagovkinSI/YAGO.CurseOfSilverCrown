using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.BL.Models.Main;
using YSI.CurseOfSilverCrown.Core.BL.Models;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class MutinyAction : DomainActionBase
    {
        private const int ImportanceBase = 5000;

        private DomainMain DomainMain { get; set; }

        public MutinyAction(ApplicationDbContext context, Turn currentTurn, Domain domain)
            : base(context, currentTurn, domain)
        {
            DomainMain = Context.GetDomainMain(domain.Id).Result;
        }

        protected override bool CheckValidAction()
        {
            return DomainMain.Warriors < 40;
        }

        protected override bool Execute()
        {
            var coffers = Domain.Coffers;
            var warriors = DomainHelper.GetWarriorCount(Context, Domain.Id);

            var newCoffers = RandomHelper.AddRandom(CoffersParameters.StartCount, roundRequest: -1);
            var newWarriors = RandomHelper.AddRandom(WarriorParameters.StartCount);
            Domain.Coffers = newCoffers;
            DomainHelper.SetWarriorCount(Context, Domain.Id, newWarriors);

            var eventStoryResult = new EventStoryResult(enEventResultType.Mutiny);
            var temp = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enActionParameter.Coffers,
                                Before = coffers,
                                After = newCoffers
                            },
                            new EventParametrChange
                            {
                                Type = enActionParameter.Warrior,
                                Before = warriors,
                                After = newWarriors
                            }
                        };
            eventStoryResult.AddEventOrganization(Domain, enEventOrganizationType.Main, temp);

            EventStory = new EventStory
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = eventStoryResult.ToJson()
            };

            OrganizationEventStories = new List<DomainEventStory>
            {
                new DomainEventStory
                {
                    Domain = Domain,
                    Importance = ImportanceBase,
                    EventStory = EventStory
                }
            };

            return true;
        }

    }
}
