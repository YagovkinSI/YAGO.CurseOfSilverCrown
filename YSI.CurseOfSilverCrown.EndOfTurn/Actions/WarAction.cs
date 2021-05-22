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
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.Core.Commands;
using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class WarAction : WarBaseAction
    {
        public WarAction(ApplicationDbContext context, Turn currentTurn, Unit command)
            : base(context, currentTurn, command)
        {
        }

        protected override bool CheckValidAction()
        {
            return Unit.Type == enArmyCommandType.War &&
                Unit.PositionDomainId == Unit.TargetDomainId &&
                Unit.Status == enCommandStatus.ReadyToRun &&
                !KingdomHelper.IsSameKingdoms(Context.Domains, Unit.Domain, Unit.Target);
            //TODO: Про своё королевство отдельная новость
        }

        protected override void SetFinalOfWar(List<WarParticipant> warParticipants, bool isVictory)
        {
            if (isVictory)
            {
                Unit.Target.SuzerainId = Unit.DomainId;
                Unit.Target.Suzerain = Unit.Domain;
                Unit.Target.TurnOfDefeat = CurrentTurn.Id;

                var unitsForCancelSupportDefense = warParticipants
                    .Where(p => p.Type == enTypeOfWarrior.TargetSupport)
                    .Select(p => p.Unit)
                    .ToList();
                foreach (var unit in unitsForCancelSupportDefense)
                {
                    unit.TargetDomainId = unit.DomainId;
                    unit.Status = enCommandStatus.ReadyToRun;
                }
                Unit.TypeInt = (int)enArmyCommandType.WarSupportDefense;
            }

            Unit.Status = enCommandStatus.Complited;
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
            OrganizationEventStories = new List<DomainEventStory>();            
            foreach (var organizationsParticipant in organizationsParticipants)
            {
                var organizationEventStory = new DomainEventStory
                {
                    DomainId = organizationsParticipant.Key,
                    Importance = importance,
                    EventStory = EventStory
                };
                OrganizationEventStories.Add(organizationEventStory);
            }
        }
    }
}
