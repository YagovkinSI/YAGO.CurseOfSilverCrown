using System.Collections.Generic;
using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Database.Events;
using YAGO.World.Host.Database.Turns;
using YAGO.World.Host.Helpers.Events;
using YAGO.World.Host.Infrastructure.Database;
using YAGO.World.Host.Parameters;

namespace YAGO.World.Host.Helpers.Actions
{
    internal class CastleFireAction : DomainActionBase
    {
        public CastleFireAction(ApplicationDbContext context, Turn currentTurn, Organization domain)
            : base(context, currentTurn, domain)
        {
        }

        public override bool CheckValidAction()
        {
            return Domain.Fortifications > FortificationsParameters.StartCount * 1.2;
        }

        protected override bool Execute()
        {
            var startParametr = Domain.Fortifications;
            var endParametr = (int)(Domain.Fortifications * RandomHelper.AddRandom(0.5, 30));
            if (endParametr < FortificationsParameters.StartCount * 0.9)
                endParametr = RandomHelper.AddRandom(FortificationsParameters.StartCount);
            var deltaParamets = endParametr - startParametr;
            if (deltaParamets > 1)
                return false;
            Domain.Fortifications = endParametr;

            var eventStoryResult = CreateEventStoryResult(startParametr, endParametr);
            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, - deltaParamets * 2 }
            };
            CreateEventStory(eventStoryResult, dommainEventStories, EventType.CastleFire);

            return true;
        }

        private EventJson CreateEventStoryResult(int startParametr, int endParametr)
        {
            var eventStoryResult = new EventJson();
            var temp = new List<EventParticipantParameterChange>
            {
                EventJsonParametrChangeHelper.Create(EventParticipantParameterType.Fortifications, startParametr, endParametr)
            };
            eventStoryResult.AddEventOrganization(Domain.Id, EventParticipantType.Main, temp);
            return eventStoryResult;
        }
    }
}
