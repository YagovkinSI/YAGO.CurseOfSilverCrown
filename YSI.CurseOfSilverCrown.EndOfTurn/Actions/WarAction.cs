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
                Unit.Status == enCommandStatus.ReadyToMove &&
                RouteHelper.IsNeighbors(Context, Unit.PositionDomainId.Value, Unit.TargetDomainId.Value) &&
                !KingdomHelper.IsSameKingdoms(Context.Domains, Unit.Domain, Unit.Target);
            //TODO: Про своё королевство отдельная новость
        }

        protected override void SetFinalOfWar(List<WarParticipant> warParticipants, bool isVictory)
        {
            if (isVictory)
            {
                var agressorDomain = Context.Domains.Find(Unit.DomainId);
                var king = KingdomHelper.GetKingdomCapital(Context.Domains.ToList(), agressorDomain);

                var targetDomain = Context.Domains
                    .Include(d => d.UnitsHere)
                    .Single(d => d.Id == Unit.TargetDomainId);
                targetDomain.SuzerainId = king.Id;
                targetDomain.TurnOfDefeat = CurrentTurn.Id;
                Context.Update(targetDomain);

                foreach (var unit in targetDomain.UnitsHere)
                {
                    if (unit.Status != enCommandStatus.Destroyed && 
                        !KingdomHelper.IsSameKingdoms(Context.Domains, king, unit.Domain))
                    {
                        unit.Status = enCommandStatus.Retreat;
                        unit.Type = enArmyCommandType.WarSupportDefense;
                        unit.Target2DomainId = null;
                        Context.Update(unit);
                    }
                }

                var agressors = warParticipants
                    .Where(p => p.Type == enTypeOfWarrior.Agressor || p.Type == enTypeOfWarrior.AgressorSupport)
                    .Select(p => p.Unit)
                    .ToList();
                foreach (var unit in agressors)
                {
                    if (unit.Status != enCommandStatus.Destroyed && 
                        KingdomHelper.IsSameKingdoms(Context.Domains, king, unit.Domain))
                    {
                        unit.PositionDomainId = Unit.TargetDomainId;
                        unit.Target2DomainId = null;
                        unit.Type = enArmyCommandType.WarSupportDefense;
                        unit.Status = enCommandStatus.Complited;
                        Context.Update(unit);
                    }
                }
            }
            else
            {
                Unit.Status = enCommandStatus.Complited;
                Context.Update(Unit);
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

            var importance = warParticipants.Sum(p => p.WarriorLosses) * 50 + (isVictory ? 5000 : 0);

            var dommainEventStories = organizationsParticipants.ToDictionary(
                o => o.Key,
                o => importance);
            CreateEventStory(eventStoryResult, dommainEventStories);
        }
    }
}
