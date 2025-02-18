using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Web.Database;
using YSI.CurseOfSilverCrown.Web.Database.Commands;
using YSI.CurseOfSilverCrown.Web.Database.Domains;
using YSI.CurseOfSilverCrown.Web.Database.Turns;
using YSI.CurseOfSilverCrown.Web.Database.Units;
using YSI.CurseOfSilverCrown.Web.Helpers;
using YSI.CurseOfSilverCrown.Web.Helpers.War;

namespace YSI.CurseOfSilverCrown.Web.Helpers.Actions.War
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
                _warActionParameters.AgressorUnit.Status = CommandStatus.Complited;
                _context.Update(_warActionParameters.AgressorUnit);

                var agressorSupport = _warActionParameters.WarActionMembers
                    .Where(u => u.IsAgressor && u.Unit.Id != _warActionParameters.AgressorUnit.Id);
                foreach (var member in agressorSupport)
                {
                    member.Unit.Type = UnitCommandType.WarSupportAttack;
                    member.Unit.Target2DomainId = _warActionParameters.AgressorUnit.Id;
                    _context.Update(member.Unit);
                }
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
                if (unit.Status != CommandStatus.Destroyed &&
                    (_context.Domains.IsSameKingdoms(king, unit.Domain) ||
                     DomainRelationsHelper.HasPermissionOfPassage(_context, unit.Id, targetDomain.Id)))
                {
                    unit.PositionDomainId = _warActionParameters.TargetDomainId;
                    unit.TargetDomainId = _warActionParameters.TargetDomainId;
                    unit.Target2DomainId = null;
                    unit.Type = UnitCommandType.WarSupportDefense;
                    unit.Status = CommandStatus.Complited;
                    _context.Update(unit);
                }
            }
        }

        private void SetRetreatCommands(Domain targetDomain, Domain king)
        {
            foreach (var unit in targetDomain.UnitsHere)
            {
                if (unit.Status != CommandStatus.Destroyed &&
                    !(_context.Domains.IsSameKingdoms(king, unit.Domain) ||
                      DomainRelationsHelper.HasPermissionOfPassage(_context, unit.Id, targetDomain.Id)))
                {
                    unit.Status = CommandStatus.Retreat;
                    _context.Update(unit);
                }
            }

            foreach (var unit in targetDomain.Units)
            {
                unit.Type = unit.Type == UnitCommandType.CollectTax
                    ? UnitCommandType.CollectTax
                    : UnitCommandType.WarSupportDefense;
                unit.TargetDomainId = unit.DomainId;
                unit.Target2DomainId = null;
                _context.Update(unit);
            }
        }
    }
}
