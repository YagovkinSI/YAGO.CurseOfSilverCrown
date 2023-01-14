using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Utils;

namespace YSI.CurseOfSilverCrown.EndOfTurn.AI
{
    internal class UserAI
    {
        private ApplicationDbContext Context { get; }

        private Domain Domain { get; }

        private readonly double _risky;
        private readonly double _peaceful;
        private readonly double _loyalty;

        public UserAI(ApplicationDbContext context, int personId)
        {
            Context = context;
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
            SetUnitCommands();
            SetDomainCommands();
        }

        private void SetUnitCommands()
        {
            var units = PrepareUnit().Result;

            var kingdomIds = KingdomHelper.GetAllDomainsIdInKingdoms(Context.Domains, Domain);
            foreach (var unit in units)
                ChooseUnitCommand(unit, kingdomIds);

            Context.UpdateRange(units);
            Context.SaveChanges();
        }

        private void SetDomainCommands()
        {
            ResetCommands();

            if (Domain.Coffers < 100)
                return;

            var chooseWar = CurrentParametr(_peaceful) < 0.5;
            var chooseInvestment = CurrentParametr(_risky) > 0.5;
            var commanfType = chooseInvestment
                ? enCommandType.Investments
                : chooseWar
                    ? enCommandType.Growth
                    : enCommandType.Fortifications;
            var command = new Command
            {
                DomainId = Domain.Id,
                Type = commanfType,
                Coffers = Domain.Coffers / 100 * 100,
                InitiatorPersonId = Domain.PersonId,
                Status = enCommandStatus.ReadyToMove
            };
            Context.Add(command);
            Context.SaveChanges();
        }

        private void ResetCommands()
        {
            var commandTypesForDelete = new[] {
                enCommandType.Growth,
                enCommandType.Investments,
                enCommandType.Fortifications,
                enCommandType.GoldTransfer
            };
            var commandsForDelete = Domain.Commands
                .Where(c => commandTypesForDelete.Contains(c.Type))
                .ToList();
            Context.RemoveRange(commandsForDelete);
            Context.SaveChanges();
        }

        private void ChooseUnitCommand(Unit unit, List<int> kingdomIds)
        {
            var (target, targetPower) = ChooseEnemy(unit, kingdomIds);
            var wishSuperiority = 1.2 * (1.5 - CurrentParametr(_risky));
            var wishAttack = CurrentParametr(_peaceful) < 0.5;
            if (target != null
                && unit.Warriors / targetPower > wishSuperiority
                && wishAttack)
            {
                var success = UnitHelper.TryWar(unit, target.Id, Context).Result;
            }
            else if (unit.PositionDomainId == Domain.Id)
            {
                unit.Type = unit.Domain.SuzerainId != null && !unit.Domain.Vassals.Any()
                    ? enArmyCommandType.CollectTax
                    : enArmyCommandType.WarSupportDefense;
            }
            else
            {
                CommandForDopartedUnit(unit);
            }
        }

        private void CommandForDopartedUnit(Unit unit)
        {
            var returnUnit = unit.Position.SuzerainId != unit.DomainId ||
                WarBaseHelper.GetTargetPower(unit.Position) > 0.9 * WarBaseHelper.GetTargetPower(Domain);
            if (returnUnit)
            {
                var count = (int)(unit.Warriors * (0.05 + new Random().NextDouble() / 5));
                if (count > 100 && unit.Position.SuzerainId == unit.DomainId)
                {
                    var (success, newUnit) = UnitHelper.TrySeparate(unit, count, Context).Result;
                    if (success)
                    {
                        newUnit.TargetDomainId = unit.PositionDomainId;
                        newUnit.Type = enArmyCommandType.WarSupportDefense;
                        Context.Update(newUnit);
                    }
                }
            }
            unit.TargetDomainId = returnUnit
                ? Domain.Id
                : unit.PositionDomainId;
            unit.Type = enArmyCommandType.WarSupportDefense;
        }

        private (Domain, double) ChooseEnemy(Unit unit, List<int> kingdomIds)
        {
            var targets = RouteHelper.GetNeighbors(Context, unit.PositionDomainId.Value)
                    .Where(d => !kingdomIds.Contains(d.Id))
                    .OrderBy(d => WarBaseHelper.GetTargetPower(d))
                    .ToArray();
            var index = 0;
            while (new Random().NextDouble() < 0.25)
                index++;
            return targets.Count() > index
                ? (targets[index], WarBaseHelper.GetTargetPower(targets[index]))
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
                var success = await UnitHelper.TryUnion(unitTo, unitFrom, Context);
                if (!success)
                    throw new Exception("Некорретная подборка юнитов для объединения");
            }
            return unitTo;
        }
    }
}
