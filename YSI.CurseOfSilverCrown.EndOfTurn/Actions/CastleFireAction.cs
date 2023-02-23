using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class CastleFireAction : DomainActionBase
    {
        public CastleFireAction(ApplicationDbContext context, Turn currentTurn, Domain domain)
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
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }

        private EventStoryResult CreateEventStoryResult(int startParametr, int endParametr)
        {
            var eventStoryResult = new EventStoryResult(enEventResultType.CastleFire);
            var temp = new List<EventParametrChange>
            {
                EventParametrChangeHelper.Create(enActionParameter.Fortifications, startParametr, endParametr)
            };
            eventStoryResult.AddEventOrganization(Domain.Id, enEventOrganizationType.Main, temp);
            return eventStoryResult;
        }
    }
}
