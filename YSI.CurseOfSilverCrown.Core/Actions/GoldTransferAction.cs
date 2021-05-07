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
using YSI.CurseOfSilverCrown.Core.Event;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal class GoldTransferAction : ActionBase
    {
        protected int ImportanceBase => 500;

        public GoldTransferAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        protected override bool Execute()
        {
            if (Command.Coffers > Command.Organization.Coffers)
                Command.Coffers = Command.Organization.Coffers;

            if (Command.TargetOrganizationId == Command.OrganizationId || Command.Coffers <= 0)
                return false;

            var coffers = Command.Organization.Coffers;
            var newCoffers = Command.Organization.Coffers - Command.Coffers;
            Command.Organization.Coffers = newCoffers;

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
            eventStoryResult.AddEventOrganization(Command.Organization, enEventOrganizationType.Main, temp1);
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


            OrganizationEventStories = new List<OrganizationEventStory>
            {
                new OrganizationEventStory
                {
                    Organization = Command.Organization,
                    Importance = Command.Coffers,
                    EventStory = EventStory
                },
                new OrganizationEventStory
                {
                    Organization = Command.Target,
                    Importance = Command.Coffers,
                    EventStory = EventStory
                }
            };

            return true;
        }
    }
}
