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

        protected override bool Execute()
        {
            if (Command.Coffers > Command.Domain.Coffers)
                Command.Coffers = Command.Domain.Coffers;

            if (Command.TargetDomainId == Command.DomainId || Command.Coffers <= 0)
                return false;

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
            eventStoryResult.AddEventOrganization(Command.Domain, enEventOrganizationType.Main, temp1);
            var temp2 = new List<EventParametrChange>
            {
                new EventParametrChange
                {
                    Type = enActionParameter.Coffers,
                    Before = targetCoffers,
                    After = targetNewCoffers,
                }
            };
            eventStoryResult.AddEventOrganization(Command.Target, enEventOrganizationType.Target, temp2);

            EventStory = new EventStory
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = eventStoryResult.ToJson()
            };


            OrganizationEventStories = new List<DomainEventStory>
            {
                new DomainEventStory
                {
                    Domain = Command.Domain,
                    Importance = Command.Coffers,
                    EventStory = EventStory
                },
                new DomainEventStory
                {
                    Domain = Command.Target,
                    Importance = Command.Coffers,
                    EventStory = EventStory
                }
            };

            return true;
        }
    }
}
