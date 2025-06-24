using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YAGO.World.Domain.Units.Enums;
using YAGO.World.Infrastructure.APIModels;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Domains;
using YAGO.World.Infrastructure.Database.Models.Units;
using YAGO.World.Infrastructure.Helpers;
using YAGO.World.Infrastructure.Helpers.Map.Routes;
using YAGO.World.Infrastructure.Helpers.War;

namespace YAGO.World.Infrastructure.AI
{
    internal class SetUnitCommandsTask
    {
        public ApplicationDbContext Context { get; }
        public Organization Domain { get; }
        public AIPattern AIPattern { get; }

        internal SetUnitCommandsTask(ApplicationDbContext context, Organization domain, AIPattern aiPattern)
        {
            Context = context;
            Domain = domain;
            AIPattern = aiPattern;
        }

        internal void Execute()
        {
            var units = PrepareUnit().Result;

            foreach (var unit in units)
                ChooseUnitCommand(unit);

            Context.UpdateRange(units);
            _ = Context.SaveChanges();
        }

        private void ChooseUnitCommand(Unit unit)
        {
            var wishAttack = AIPattern.GetPeaceful() < 0.5 && unit.Warriors > 1000;
            if (wishAttack && unit.PositionDomainId == Domain.Id)
                LeavePartOfWarriorsInGarrison(unit);

            var (target, targetPower) = wishAttack
                ? ChooseEnemy(unit)
                : (null, 0);
            var wishSuperiority = 1.2 * (1.5 - AIPattern.GetRisky());
            if (wishAttack
                && target != null
                && unit.Warriors / targetPower > wishSuperiority)
            {
                _ = unit.TryWar(target.Id, Context).Result;
            }
            else if (unit.PositionDomainId == Domain.Id)
            {
                unit.Type = UnitCommandType.WarSupportDefense;
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
                newUnit.Type = UnitCommandType.WarSupportDefense;
                _ = Context.Update(newUnit);
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
                    newUnit.Type = UnitCommandType.WarSupportDefense;
                    _ = Context.Update(newUnit);
                }
            }
            unit.TargetDomainId = returnUnit
                ? Domain.Id
                : unit.PositionDomainId;
            unit.Type = UnitCommandType.WarSupportDefense;
        }

        //TODO: Big
        private (Organization, double) ChooseEnemy(Unit unit)
        {
            var targets = GetTargers(unit);

            var sortedTargets = targets
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

        private List<GameMapRoute> GetTargers(Unit unit)
        {
            var vassalDomainIds = Context.Domains.GetAllLevelVassalIds(unit.DomainId);
            var kingdomDomains = Context.Domains.GetAllDomainsIdInKingdoms(unit.Domain);
            var targets = new List<Organization>();

            foreach (var vassalDomainId in vassalDomainIds)
            {
                var vasalNeighbors = Context.GetNeighbors(vassalDomainId)
                    .Where(d => !kingdomDomains.Contains(d.Id));
                targets.AddRange(vasalNeighbors);
            }

            var routes = new List<GameMapRoute>();
            foreach (var target in targets)
            {
                if (routes.Any(t => t.TargetDomain.Id == target.Id))
                    continue;
                var parameters = new RouteFindParameters(unit, enMovementReason.Atack, target.Id);
                var route = RouteHelper.FindRoute(Context, parameters);
                if (route == null)
                    continue;
                routes.Add(new GameMapRoute(target, route.Count - 1));
            }

            return routes;
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
    }
}
