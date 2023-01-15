using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;

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
                var targetDomain = Context.Domains.Find(Unit.TargetDomainId);

                SetNewSuzerain(targetDomain, agressorDomain);
                SetRetreatCommands(targetDomain, king);
                SetAccupation(warParticipants, targetDomain, king);
            }
            else
            {
                Unit.Status = enCommandStatus.Complited;
                Context.Update(Unit);
            }
        }

        private void SetAccupation(List<WarParticipant> warParticipants, Domain targetDomain, Domain king)
        {
            var agressors = warParticipants
                    .Where(p => p.Type == enTypeOfWarrior.Agressor || p.Type == enTypeOfWarrior.AgressorSupport)
                    .Select(p => p.Unit)
                    .ToList();
            foreach (var unit in agressors)
            {
                if (unit.Status != enCommandStatus.Destroyed &&
                    (KingdomHelper.IsSameKingdoms(Context.Domains, king, unit.Domain) ||
                     DomainRelationsHelper.HasPermissionOfPassage(Context, unit.Id, targetDomain.Id)))
                {
                    unit.PositionDomainId = Unit.TargetDomainId;
                    unit.TargetDomainId = Unit.TargetDomainId;
                    unit.Target2DomainId = null;
                    unit.Type = enArmyCommandType.WarSupportDefense;
                    unit.Status = enCommandStatus.Complited;
                    Context.Update(unit);
                }
            }
        }

        private void SetRetreatCommands(Domain targetDomain, Domain king)
        {
            foreach (var unit in targetDomain.UnitsHere)
            {
                if (unit.Status != enCommandStatus.Destroyed &&
                    !(KingdomHelper.IsSameKingdoms(Context.Domains, king, unit.Domain) ||
                      DomainRelationsHelper.HasPermissionOfPassage(Context, unit.Id, targetDomain.Id)))
                {
                    unit.Status = enCommandStatus.Retreat;
                    Context.Update(unit);
                }
            }

            foreach (var unit in targetDomain.Units)
            {
                unit.Type = unit.Type == enArmyCommandType.CollectTax
                    ? enArmyCommandType.CollectTax
                    : enArmyCommandType.WarSupportDefense;
                unit.TargetDomainId = unit.DomainId;
                unit.Target2DomainId = null;
                Context.Update(unit);
            }
        }

        private void SetNewSuzerain(Domain targetDomain, Domain agressorDomain)
        {         
            if (targetDomain.SuzerainId == null)
                targetDomain.TurnOfDefeat = CurrentTurn.Id;
            targetDomain.SuzerainId = agressorDomain.Id;
            targetDomain.Suzerain = agressorDomain;
            Context.Update(targetDomain);
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

            var importance = 5000 + warParticipants.Sum(p => p.WarriorLosses) * 50 + (isVictory ? 2000 : 0);

            var dommainEventStories = organizationsParticipants.ToDictionary(
                o => o.Key,
                o => importance);
            CreateEventStory(eventStoryResult, dommainEventStories);
        }
    }
}
