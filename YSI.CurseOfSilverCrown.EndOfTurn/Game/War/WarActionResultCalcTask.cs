using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.EndOfTurn.Actions;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Game.War
{
    internal class WarActionResultCalcTask
    {
        private readonly ApplicationDbContext _context;
        private readonly WarActionParameters _warActionParameters;
        private readonly Turn _currentTurn;

        public WarActionResultCalcTask(ApplicationDbContext context,
            WarActionParameters warActionParameters,
            Turn currentTurn)
        {
            _context = context;
            _warActionParameters = warActionParameters;
            _currentTurn = currentTurn;
        }

        public void Execute()
        {
            if (_warActionParameters.IsVictory)
            {
                var agressorDomain = _context.Domains.Find(_warActionParameters.AgressorUnit.DomainId);
                var king = KingdomHelper.GetKingdomCapital(_context.Domains.ToList(), agressorDomain);
                var targetDomain = _context.Domains.Find(_warActionParameters.TargetDomainId);

                SetNewSuzerain(targetDomain, agressorDomain);
                SetRetreatCommands(targetDomain, king);
                SetAccupation(_warActionParameters.WarActionMembers, targetDomain, king);
            }
            else
            {
                _warActionParameters.AgressorUnit.Status = enCommandStatus.Complited;
                _context.Update(_warActionParameters.AgressorUnit);
            }
        }

        private void SetNewSuzerain(Domain targetDomain, Domain agressorDomain)
        {
            if (targetDomain.SuzerainId == null)
                targetDomain.TurnOfDefeat = _currentTurn.Id;
            targetDomain.SuzerainId = agressorDomain.Id;
            targetDomain.Suzerain = agressorDomain;
            _context.Update(targetDomain);
        }

        private void SetAccupation(List<WarActionMember> warMembers, Domain targetDomain, Domain king)
        {
            var agressors = warMembers
                    .Where(p => p.Type == enTypeOfWarrior.Agressor || p.Type == enTypeOfWarrior.AgressorSupport)
                    .Select(p => p.Unit)
                    .ToList();
            foreach (var unit in agressors)
            {
                if (unit.Status != enCommandStatus.Destroyed &&
                    (KingdomHelper.IsSameKingdoms(_context.Domains, king, unit.Domain) ||
                     DomainRelationsHelper.HasPermissionOfPassage(_context, unit.Id, targetDomain.Id)))
                {
                    unit.PositionDomainId = _warActionParameters.TargetDomainId;
                    unit.TargetDomainId = _warActionParameters.TargetDomainId;
                    unit.Target2DomainId = null;
                    unit.Type = enArmyCommandType.WarSupportDefense;
                    unit.Status = enCommandStatus.Complited;
                    _context.Update(unit);
                }
            }
        }

        private void SetRetreatCommands(Domain targetDomain, Domain king)
        {
            foreach (var unit in targetDomain.UnitsHere)
            {
                if (unit.Status != enCommandStatus.Destroyed &&
                    !(KingdomHelper.IsSameKingdoms(_context.Domains, king, unit.Domain) ||
                      DomainRelationsHelper.HasPermissionOfPassage(_context, unit.Id, targetDomain.Id)))
                {
                    unit.Status = enCommandStatus.Retreat;
                    _context.Update(unit);
                }
            }

            foreach (var unit in targetDomain.Units)
            {
                unit.Type = unit.Type == enArmyCommandType.CollectTax
                    ? enArmyCommandType.CollectTax
                    : enArmyCommandType.WarSupportDefense;
                unit.TargetDomainId = unit.DomainId;
                unit.Target2DomainId = null;
                _context.Update(unit);
            }
        }
    }
}
