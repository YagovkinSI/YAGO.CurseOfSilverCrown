using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Domains;
using YSI.CurseOfSilverCrown.Core.Database.EventDomains;
using YSI.CurseOfSilverCrown.Core.Database.Events;
using YSI.CurseOfSilverCrown.Core.Database.Turns;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Actions
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

            var eventStoryResult = new EventJson(enEventType.Mutiny);
            var temp = new List<EventJsonParametrChange>
            {
                EventJsonParametrChangeHelper.Create(enEventParameterType.Coffers, coffers, newCoffers),
                EventJsonParametrChangeHelper.Create(enEventParameterType.Warrior, warriors, newWarriors)
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
