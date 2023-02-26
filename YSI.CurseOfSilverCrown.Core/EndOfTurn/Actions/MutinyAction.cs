using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.GameEvent;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class MutinyAction : DomainActionBase
    {
        private const int ImportanceBase = 5000;

        private Domain Domain { get; set; }

        public MutinyAction(ApplicationDbContext context, Turn currentTurn, Domain domain)
            : base(context, currentTurn, domain)
        {
            Domain = Context.Domains.Find(domain.Id);
        }

        public override bool CheckValidAction()
        {
            return Domain.WarriorCount < WarriorParameters.StartCount / 10;
        }

        protected override bool Execute()
        {
            var coffers = Domain.Coffers;
            var warriors = DomainHelper.GetWarriorCount(Context, Domain.Id);

            var newCoffers = RandomHelper.AddRandom(CoffersParameters.StartCount, roundRequest: -1);
            var newWarriors = RandomHelper.AddRandom(WarriorParameters.StartCount);
            Domain.Coffers = newCoffers;
            DomainHelper.SetWarriorCount(Context, Domain.Id, newWarriors);

            var eventStoryResult = new EventStoryResult(enEventType.Mutiny);
            var temp = new List<EventParametrChange>
            {
                EventParametrChangeHelper.Create(enEventParameterType.Coffers, coffers, newCoffers),
                EventParametrChangeHelper.Create(enEventParameterType.Warrior, warriors, newWarriors)
            };
            eventStoryResult.AddEventOrganization(Domain.Id, enEventDomainType.Main, temp);

            var domainStroies = new Dictionary<int, int>
            {
                {
                    Domain.Id,
                    (newCoffers - coffers) * (newCoffers < coffers ? 2 : 1)
                    + (newWarriors - warriors)
                }
            };
            CreateEventStory(eventStoryResult, domainStroies);

            return true;
        }

    }
}
