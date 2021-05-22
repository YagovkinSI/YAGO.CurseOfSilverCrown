using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Core.Commands;

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

        protected override bool CheckValidAction()
        {
            FixCoffersForAction();

            return Command.Type == enCommandType.Investments &&
                Command.Coffers > 0 &&
                Command.Status == enCommandStatus.ReadyToRun;
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
                            new EventParametrChange
                            {
                                Type = enActionParameter.Investments,
                                Before = investments + InvestmentsHelper.IlusionInvestment,
                                After = newInvestments + InvestmentsHelper.IlusionInvestment
                            },
                            new EventParametrChange
                            {
                                Type = enActionParameter.Coffers,
                                Before = coffers,
                                After = newCoffers
                            }
                        };
            eventStoryResult.AddEventOrganization(Command.Domain, enEventOrganizationType.Main, trmp);

            EventStory = new EventStory
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = eventStoryResult.ToJson()
            };

            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, spentCoffers / 4 }
            };
            CreateOrganizationEventStories(dommainEventStories);

            return true;
        }
    }
}
