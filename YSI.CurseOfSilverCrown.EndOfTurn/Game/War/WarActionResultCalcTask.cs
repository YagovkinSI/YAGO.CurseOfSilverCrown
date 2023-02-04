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
        private readonly Unit _agressorUnit;
        private readonly List<WarActionMember> _warMembers;
        private readonly Turn _currentTurn;
        private readonly bool _isVictory;

        public WarActionResultCalcTask(ApplicationDbContext context,
            Unit agressorUnit,
            List<WarActionMember> warMembers,
            Turn currentTurn,
            bool isVictory)
        {
            _context = context;
            _agressorUnit = agressorUnit;
            _warMembers = warMembers;
            _currentTurn = currentTurn;
            _isVictory = isVictory;
        }

        public void Execute()
        {
            if (_isVictory)
            {
                var agressorDomain = _context.Domains.Find(_agressorUnit.DomainId);
                var king = KingdomHelper.GetKingdomCapital(_context.Domains.ToList(), agressorDomain);
                var targetDomain = _context.Domains.Find(_agressorUnit.TargetDomainId);

                SetNewSuzerain(targetDomain, agressorDomain);
                SetRetreatCommands(targetDomain, king);
                SetAccupation(_warMembers, targetDomain, king);
            }
            else
            {
                _agressorUnit.Status = enCommandStatus.Complited;
                _context.Update(_agressorUnit);
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
                    unit.PositionDomainId = _agressorUnit.TargetDomainId;
                    unit.TargetDomainId = _agressorUnit.TargetDomainId;
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
