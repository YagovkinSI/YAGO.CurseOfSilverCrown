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
        public WarAction(ApplicationDbContext context, Turn currentTurn, int unitId)
            : base(context, currentTurn, unitId)
        {
        }

        protected override bool CheckValidAction()
        {
            return Unit.Type == enArmyCommandType.War &&
                Unit.TargetDomainId != null &&
                Unit.PositionDomainId == Unit.TargetDomainId &&
                Unit.Status == enCommandStatus.ReadyToRun &&
                !KingdomHelper.IsSameKingdoms(Context.Domains, Unit.Domain, Unit.Target);
            //TODO: Про своё королевство отдельная новость
        }

        protected override void SetFinalOfWar(List<WarParticipant> warParticipants, bool isVictory)
        {
            if (isVictory)
            {
                var domain = Context.Domains.Find(Unit.DomainId);
                var king = KingdomHelper.GetKingdomCapital(Context.Domains.ToList(), domain);
                
                Unit.Target.SuzerainId = king.Id;
                Unit.Target.Suzerain = king;
                Unit.Target.TurnOfDefeat = CurrentTurn.Id;

                var unitsForCancelSupportDefense = warParticipants
                    .Where(p => p.Type == enTypeOfWarrior.TargetSupport)
                    .Select(p => p.Unit)
                    .ToList();
                foreach (var unit in unitsForCancelSupportDefense)
                {
                    if (unit.TargetDomainId != unit.DomainId)
                        unit.Status = enCommandStatus.ReadyToRun;
                    unit.TargetDomainId = unit.DomainId;
                }

                var agressors = warParticipants
                    .Where(p => p.Type == enTypeOfWarrior.Agressor || p.Type == enTypeOfWarrior.AgressorSupport)
                    .Select(p => p.Unit)
                    .ToList();
                foreach (var unit in agressors)
                {
                    unit.Target2DomainId = null;
                    unit.Type = enArmyCommandType.WarSupportDefense;
                }
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

            var importance = warParticipants.Sum(p => p.WarriorLosses) * 50 + (isVictory ? 5000 : 0);

            var dommainEventStories = organizationsParticipants.ToDictionary(
                o => o.Key,
                o => importance);
            CreateEventStory(eventStoryResult, dommainEventStories);
        }
    }
}
