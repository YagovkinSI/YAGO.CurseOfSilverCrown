using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class GoldTransferAction : CommandActionBase
    {
        protected int ImportanceBase => 500;

        protected override bool RemoveCommandeAfterUse => true;

        public GoldTransferAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        protected override bool CheckValidAction()
        {
            FixCoffersForAction();

            return Command.Type == enCommandType.GoldTransfer && 
                Command.Coffers > 0 &&
                Command.TargetDomainId != null &&
                Command.TargetDomainId != Command.DomainId &&
                Command.Status == enCommandStatus.ReadyToRun;
        }

        protected override bool Execute()
        {
            var coffers = Command.Domain.Coffers;
            var newCoffers = Command.Domain.Coffers - Command.Coffers;
            Command.Domain.Coffers = newCoffers;

            var targetCoffers = Command.Target.Coffers;
            var targetNewCoffers = Command.Target.Coffers + Command.Coffers;
            Command.Target.Coffers = targetNewCoffers;

            var type = enEventResultType.GoldTransfer;
            var eventStoryResult = new EventStoryResult(type);
            var temp1 = new List<EventParametrChange>
            {
                new EventParametrChange
                {
                    Type = enActionParameter.Coffers,
                    Before = coffers,
                    After = newCoffers,
                }
            };
            eventStoryResult.AddEventOrganization(Command.DomainId, enEventOrganizationType.Main, temp1);
            var temp2 = new List<EventParametrChange>
            {
                new EventParametrChange
                {
                    Type = enActionParameter.Coffers,
                    Before = targetCoffers,
                    After = targetNewCoffers,
                }
            };
            eventStoryResult.AddEventOrganization(Command.TargetDomainId.Value, enEventOrganizationType.Target, temp2);

            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, Command.Coffers },
                { Command.TargetDomainId.Value, Command.Coffers }
            };
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }
    }
}
