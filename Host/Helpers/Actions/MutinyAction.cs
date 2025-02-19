using System.Collections.Generic;
using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Database.Events;
using YAGO.World.Host.Database.Turns;
using YAGO.World.Host.Helpers.Events;
using YAGO.World.Host.Infrastructure.Database;
using YAGO.World.Host.Parameters;

namespace YAGO.World.Host.Helpers.Actions
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
            var coffers = Domain.Gold;
            var warriors = DomainHelper.GetWarriorCount(Context, Domain.Id);

            var newCoffers = RandomHelper.AddRandom(CoffersParameters.StartCount, roundRequest: -1);
            var newWarriors = RandomHelper.AddRandom(WarriorParameters.StartCount);
            Domain.Gold = newCoffers;
            DomainHelper.SetWarriorCount(Context, Domain.Id, newWarriors);

            var eventStoryResult = new EventJson();
            var temp = new List<EventParticipantParameterChange>
            {
                EventJsonParametrChangeHelper.Create(EventParticipantParameterType.Coffers, coffers, newCoffers),
                EventJsonParametrChangeHelper.Create(EventParticipantParameterType.Warrior, warriors, newWarriors)
            };
            eventStoryResult.AddEventOrganization(Domain.Id, EventParticipantType.Main, temp);

            var domainStroies = new Dictionary<int, int>
            {
                {
                    Domain.Id,
                    (newCoffers - coffers) * (newCoffers < coffers ? 2 : 1)
                    + (newWarriors - warriors)
                }
            };
            CreateEventStory(eventStoryResult, domainStroies, EventType.Mutiny);

            return true;
        }

    }
}
