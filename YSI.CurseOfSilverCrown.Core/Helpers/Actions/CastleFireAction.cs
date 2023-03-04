using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.Domains;
using YSI.CurseOfSilverCrown.Core.MainModels.EventDomains;
using YSI.CurseOfSilverCrown.Core.MainModels.Events;
using YSI.CurseOfSilverCrown.Core.MainModels.Turns;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Utils;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Actions
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

        private EventJson CreateEventStoryResult(int startParametr, int endParametr)
        {
            var eventStoryResult = new EventJson(enEventType.CastleFire);
            var temp = new List<EventJsonParametrChange>
            {
                EventJsonParametrChangeHelper.Create(enEventParameterType.Fortifications, startParametr, endParametr)
            };
            eventStoryResult.AddEventOrganization(Domain.Id, enEventDomainType.Main, temp);
            return eventStoryResult;
        }
    }
}
