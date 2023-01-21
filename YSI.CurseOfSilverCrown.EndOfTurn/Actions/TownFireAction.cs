using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class TownFireAction : DomainActionBase
    {
        public TownFireAction(ApplicationDbContext context, Turn currentTurn, Domain domain)
            : base(context, currentTurn, domain)
        {
        }

        protected override bool CheckValidAction()
        {
            return Domain.Investments > InvestmentsHelper.StartInvestment * 1.2;
        }

        protected override bool Execute()
        {
            var startParametr = Domain.Investments;
            var endParametr = (int)(Domain.Investments * RandomHelper.AddRandom(0.8, 20));
            if (endParametr < InvestmentsHelper.StartInvestment * 0.9)
                endParametr = RandomHelper.AddRandom(InvestmentsHelper.StartInvestment);
            var deltaParamets = endParametr - startParametr;
            if (deltaParamets > 1)
                return false;
            Domain.Investments = endParametr;

            var eventStoryResult = CreateEventStoryResult(startParametr, endParametr);
            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, 5000 - deltaParamets * 1000 / InvestmentsHelper.StartInvestment }
            };
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }

        private EventStoryResult CreateEventStoryResult(int startParametr, int endParametr)
        {
            var eventStoryResult = new EventStoryResult(enEventResultType.TownFire);
            var temp = new List<EventParametrChange>
            {
                new EventParametrChange
                {
                    Type = enActionParameter.Investments,
                    Before = startParametr,
                    After = endParametr
                }
            };
            eventStoryResult.AddEventOrganization(Domain.Id, enEventOrganizationType.Main, temp);
            return eventStoryResult;
        }
    }
}
