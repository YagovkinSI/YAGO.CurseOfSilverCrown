using System;
using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.Commands;
using YSI.CurseOfSilverCrown.Core.MainModels.Commands.DomainCommands;
using YSI.CurseOfSilverCrown.Core.MainModels.EventDomains;
using YSI.CurseOfSilverCrown.Core.MainModels.Events;
using YSI.CurseOfSilverCrown.Core.MainModels.Turns;
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

            return Command.Type == enDomainCommandType.Investments &&
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

            var eventStoryResult = new EventStoryResult(enEventType.Investments);
            var trmp = new List<EventParametrChange>
            {
                EventParametrChangeHelper.Create(enEventParameterType.Investments, investments, newInvestments),
                EventParametrChangeHelper.Create(enEventParameterType.Coffers, coffers, newCoffers)
            };
            eventStoryResult.AddEventOrganization(Command.DomainId, enEventDomainType.Main, trmp);

            var thresholdImportance = EventStoryHelper.GetThresholdImportance(investments, newInvestments);
            eventStoryResult.EventResultType = GetInvestmentsEventResultType(thresholdImportance);
            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, spentCoffers + thresholdImportance }
            };
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }

        private enEventType GetInvestmentsEventResultType(int thresholdImportance)
        {
            if (thresholdImportance < 10000)
                return enEventType.Investments;
            else if (thresholdImportance < 30000)
                return enEventType.InvestmentsLevelI;
            else if (thresholdImportance < 100000)
                return enEventType.InvestmentsLevelII;
            else if (thresholdImportance < 300000)
                return enEventType.InvestmentsLevelIII;
            else if (thresholdImportance < 1000000)
                return enEventType.InvestmentsLevelIV;
            else
                return enEventType.InvestmentsLevelV;
        }
    }
}
