using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Game.Map.Routes;
using YSI.CurseOfSilverCrown.Core.Game.War;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.Commands;
using YSI.CurseOfSilverCrown.Core.MainModels.Commands.DomainCommands;
using YSI.CurseOfSilverCrown.Core.MainModels.Commands.UnitCommands;
using YSI.CurseOfSilverCrown.Core.MainModels.Domains;
using YSI.CurseOfSilverCrown.Core.MainModels.Turns;
using YSI.CurseOfSilverCrown.Core.MainModels.Units;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.Core.ViewModels;

namespace YSI.CurseOfSilverCrown.AI
{
    internal class UserAI
    {
        private ApplicationDbContext Context { get; }

        private Domain Domain { get; }
        private Turn CurrentTurn { get; }

        private double _risky;
        private double _peaceful;
        private double _loyalty;

        public UserAI(ApplicationDbContext context, int personId, Turn currentTurn)
        {
            Context = context;
            CurrentTurn = currentTurn;
            Domain = context.Domains
                .Single(d => d.PersonId == personId);

            _risky = RandomHelper.DependentRandom(personId, 0);
            _peaceful = RandomHelper.DependentRandom(personId, 1);
            _loyalty = RandomHelper.DependentRandom(personId, 2);
        }

        private double CurrentParametr(double staticParametr)
        {
            return (new Random().NextDouble() + staticParametr) / 2;
        }

        public void SetCommands()
        {
            SetParameters();
            SetUnitCommands();
            SetDomainCommands();
            SetRebelionCommand();
        }

        private void SetParameters()
        {
            var grants = Context.Commands
                .Where(c => c.Type == enDomainCommandType.GoldTransfer && c.TargetDomainId == Domain.Id)
                .ToList();

            foreach (var grant in grants)
            {
                if (Context.Domains.IsSameKingdoms(grant.Domain, Domain))
                {
                    _risky -= grant.Coffers / 2000.0;
                    _peaceful += grant.Coffers / 2000.0;
                    _loyalty += grant.Coffers / 2000.0;
                }
                else
                {
                    _risky -= grant.Coffers / 2000.0;
                    _peaceful -= grant.Coffers / 2000.0;
                    _loyalty -= grant.Coffers / 2000.0;
                }
            }
        }

        private void SetUnitCommands()
        {
            var units = PrepareUnit().Result;

            foreach (var unit in units)
                ChooseUnitCommand(unit);

            Context.UpdateRange(units);
            Context.SaveChanges();
        }

        private void SetDomainCommands()
        {
            if (Domain.Coffers < CoffersParameters.StartCount * 0.2)
                return;

            var budget = new Budget(Context, Domain, Domain.PersonId);
            var notSpending = Math.Max(-(budget.Lines.Single(l => l.Type == enLineOfBudgetType.Total).Coffers.ExpectedValue.Value
                - Domain.Coffers), 0);
            var spending = Domain.Coffers - notSpending * 3;
            if (spending < CoffersParameters.StartCount * 0.2)
                return;

            ResetCommands();
            var commanfType = notSpending > 0
                ? enDomainCommandType.Investments
                : ChooseCommandType();
            var command = CreateCommand(commanfType, spending);

            Context.Add(command);
            Context.SaveChanges();
        }

        private void ResetCommands()
        {
            var commandTypesForDelete = new[] {
                enDomainCommandType.Growth,
                enDomainCommandType.Investments,
                enDomainCommandType.Fortifications,
                enDomainCommandType.GoldTransfer
            };
            var commandsForDelete = Domain.Commands
                .Where(c => commandTypesForDelete.Contains(c.Type))
                .ToList();
            Context.RemoveRange(commandsForDelete);
            Context.SaveChanges();
        }

        private enDomainCommandType ChooseCommandType()
        {
            var chooseWar = CurrentParametr(_peaceful) < 0.5;
            var chooseInvestment = CurrentParametr(_risky) > 0.5;

            return chooseInvestment
                ? enDomainCommandType.Investments
                : chooseWar
                    ? enDomainCommandType.Growth
                    : enDomainCommandType.Fortifications;
        }

        private Command CreateCommand(enDomainCommandType commanfType, int spending)
        {
            return new Command
            {
                DomainId = Domain.Id,
                Type = commanfType,
                Coffers = commanfType switch
                {
                    enDomainCommandType.Growth => (spending / 2) / 100 * 100,
                    enDomainCommandType.Investments => spending,
                    enDomainCommandType.Fortifications => spending / 2,
                    _ => spending / 2
                },
                InitiatorPersonId = Domain.PersonId,
                Status = enCommandStatus.ReadyToMove
            };
        }

        private void ChooseUnitCommand(Unit unit)
        {
            var wishAttack = CurrentParametr(_peaceful) < 0.5 && unit.Warriors > 1000;
            if (wishAttack && unit.PositionDomainId == Domain.Id)
                LeavePartOfWarriorsInGarrison(unit);

            var (target, targetPower) = wishAttack
                ? ChooseEnemy(unit)
                : (null, 0);
            var wishSuperiority = 1.2 * (1.5 - CurrentParametr(_risky));
            if (wishAttack
                && target != null
                && unit.Warriors / targetPower > wishSuperiority)
            {
                var success = unit.TryWar(target.Id, Context).Result;
            }
            else if (unit.PositionDomainId == Domain.Id)
            {
                unit.Type = enUnitCommandType.WarSupportDefense;
            }
            else
            {
                CommandForDopartedUnit(unit);
            }
        }

        private void LeavePartOfWarriorsInGarrison(Unit unit)
        {
            var garrisonPercent = new Random().NextDouble() / 10.0 + 0.1;
            var newUnitCount = (int)(unit.Warriors * garrisonPercent);
            var (success, newUnit) = unit.TrySeparate(newUnitCount, Context).Result;
            if (success)
            {
                newUnit.TargetDomainId = unit.PositionDomainId;
                newUnit.Type = enUnitCommandType.WarSupportDefense;
                Context.Update(newUnit);
            }
        }

        private void CommandForDopartedUnit(Unit unit)
        {
            var maxGarrison = FortificationsHelper.GetFortCoef(unit.Position.Fortifications);
            var returnUnit = unit.Warriors > maxGarrison;
            if (returnUnit)
            {
                var count = maxGarrison;
                var (success, newUnit) = unit.TrySeparate(count, Context).Result;
                if (success)
                {
                    newUnit.TargetDomainId = unit.PositionDomainId;
                    newUnit.Type = enUnitCommandType.WarSupportDefense;
                    Context.Update(newUnit);
                }
            }
            unit.TargetDomainId = returnUnit
                ? Domain.Id
                : unit.PositionDomainId;
            unit.Type = enUnitCommandType.WarSupportDefense;
        }

        //TODO: Big
        private (Domain, double) ChooseEnemy(Unit unit)
        {
            var vassalDomains = Context.Domains.GetAllLevelVassalIds(unit.DomainId);
            var neiborTargets = new List<GameMapRoute>();
            if (vassalDomains.Count == 1)
            {
                var niebors = Context.GetNeighbors(unit.DomainId);
                neiborTargets = niebors
                    .Where(d => !Context.Domains.IsSameKingdoms(d, unit.Domain))
                    .Select(d => new GameMapRoute(d, 1))
                    .ToList();
            }
            else
            {
                var targets = WarBaseHelper
                    .GetAvailableTargets(Context, unit.DomainId, unit, enUnitCommandType.War)
                    .Result;
                neiborTargets = targets
                    .Where(t => vassalDomains.Any(v => RouteHelper.IsNeighbors(Context, t.TargetDomain.Id, v)))
                    .ToList();
            }

            var sortedTargets = neiborTargets
                .ToDictionary(d => d.TargetDomain, d => WarActionHelper.CalcRecomendedUnitCount(Context, d.TargetDomain))
                .OrderBy(p => p.Value)
                .ToArray();

            var index = 0;
            while (new Random().NextDouble() < 0.25)
                index++;
            return sortedTargets.Count() > index
                ? (sortedTargets[index].Key, sortedTargets[index].Value)
                : (null, 0);
        }

        private async Task<IEnumerable<Unit>> PrepareUnit()
        {
            var units = Domain.Units
                .Where(u => u.DomainId == Domain.Id);
            var unitsByLocations = units.GroupBy(u => u.PositionDomainId);
            var result = new List<Unit>();
            foreach (var unitGroup in unitsByLocations)
            {
                var unit = await UnionUnits(unitGroup.ToList());
                result.Add(unit);
            }
            return result;
        }

        private async Task<Unit> UnionUnits(List<Unit> units)
        {
            var unitTo = units[0];
            for (var i = units.Count - 1; i > 0; i--)
            {
                var unitFrom = units[i];
                var success = await unitTo.TryUnion(unitFrom, Context);
                if (!success)
                    throw new Exception("Некорретная подборка юнитов для объединения");
            }
            return unitTo;
        }

        private void SetRebelionCommand()
        {
            if (Domain.SuzerainId == null)
                return;

            var canRebelion = Domain.TurnOfDefeat + RebelionHelper.TurnCountWithoutRebelion < CurrentTurn.Id;
            if (!canRebelion)
                return;

            var wishRebelion = CalcWishRebelion();
            if (wishRebelion)
            {
                var command = new Command
                {
                    DomainId = Domain.Id,
                    Type = enDomainCommandType.Rebellion,
                    Status = enCommandStatus.ReadyToMove
                };
                Context.Add(command);
                Context.SaveChanges();
            }
        }

        private bool CalcWishRebelion()
        {
            var currentLoyality = CurrentParametr(_loyalty);
            if (currentLoyality > 0.7)
                return false;

            var allWarriors = Domain.WarriorCount;
            var powerBalance = allWarriors * 1.1 / Domain.Suzerain.WarriorCount;
            if (powerBalance < 0.8)
                return false;

            return powerBalance - 4 * currentLoyality > 0;
        }
    }
}
