using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Event;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.Core.Commands;
using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal class WarAction : WarBaseAction
    {
        public WarAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        protected override bool IsValidAttack()
        {
            return !KingdomHelper.IsSameKingdoms(Context.Organizations, Command.Organization, Command.Target).Result;
        }


        protected override void SetFinalOfWar(List<WarParticipant> warParticipants, bool isVictory)
        {
            if (isVictory)
            {
                Command.Target.SuzerainId = Command.OrganizationId;
                Command.Target.Suzerain = Command.Organization;
                Command.Target.TurnOfDefeat = CurrentTurn.Id;

                var commandForDelete = warParticipants
                    .Where(p => p.Type == enTypeOfWarrior.TargetSupport)
                    .Select(p => p.Command)
                    .ToList();
                commandForDelete.ForEach(c => c.Type = enCommandType.ForDelete);

                Command.Type = enCommandType.WarSupportDefense;
            }
            else
            {
                Command.Type = enCommandType.ForDelete;
            }
        }

        protected override void CreateEvent(List<WarParticipant> warParticipants, bool isVictory)
        {
            var organizationsParticipants = warParticipants
                .GroupBy(p => p.Organization.Id);

            var type = isVictory
                        ? enEventResultType.FastWarSuccess
                        : enEventResultType.FastWarFail;
            var eventStoryResult = new EventStoryResult(type);
            FillEventOrganizationList(eventStoryResult, organizationsParticipants);

            EventStory = new EventStory
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = eventStoryResult.ToJson()
            };

            var importance = warParticipants.Sum(p => p.WarriorLosses) * 50 + (isVictory ? 5000 : 0);
            OrganizationEventStories = new List<OrganizationEventStory>();            
            foreach (var organizationsParticipant in organizationsParticipants)
            {
                var organizationEventStory = new OrganizationEventStory
                {
                    OrganizationId = organizationsParticipant.Key,
                    Importance = importance,
                    EventStory = EventStory
                };
                OrganizationEventStories.Add(organizationEventStory);
            }
        }
    }
}
