using System;
using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class InvestmentsAction : CommandActionBase
    {
        protected int ImportanceBase => 500;

        protected override bool RemoveCommandeAfterUse => true;

        public InvestmentsAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        public override bool CheckValidAction()
        {
            FixCoffersForAction();

            return Command.Type == enCommandType.Investments &&
                Command.Coffers > 0 &&
                Command.Status == enCommandStatus.ReadyToMove;
        }

        protected override bool Execute()
        {
            var coffers = Command.Domain.Coffers;
            var investments = Command.Domain.Investments;

            var spentCoffers = Math.Min(coffers, Command.Coffers);
            var getInvestments = spentCoffers;

            var newCoffers = coffers - spentCoffers;
            var newInvestments = investments + getInvestments;

            Command.Domain.Coffers = newCoffers;
            Command.Domain.Investments = newInvestments;

            var eventStoryResult = new EventStoryResult(enEventResultType.Investments);
            var trmp = new List<EventParametrChange>
            {
                EventParametrChangeHelper.Create(enActionParameter.Investments, investments, newInvestments),
                EventParametrChangeHelper.Create(enActionParameter.Coffers, coffers, newCoffers)
            };
            eventStoryResult.AddEventOrganization(Command.DomainId, enEventOrganizationType.Main, trmp);

            var thresholdImportance = EventStoryHelper.GetThresholdImportance(investments, newInvestments);
            eventStoryResult.EventResultType = GetInvestmentsEventResultType(thresholdImportance);
            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, spentCoffers + thresholdImportance }
            };
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }

        private enEventResultType GetInvestmentsEventResultType(int thresholdImportance)
        {
            if (thresholdImportance < 10000)
                return enEventResultType.Investments;
            else if (thresholdImportance < 30000)
                return enEventResultType.InvestmentsLevelI;
            else if (thresholdImportance < 100000)
                return enEventResultType.InvestmentsLevelII;
            else if (thresholdImportance < 300000)
                return enEventResultType.InvestmentsLevelIII;
            else if (thresholdImportance < 1000000)
                return enEventResultType.InvestmentsLevelIV;
            else
                return enEventResultType.InvestmentsLevelV;
        }
    }
}
