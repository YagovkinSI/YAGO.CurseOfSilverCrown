using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Helpers;
using YAGO.World.Host.Helpers.Map.Routes;
using YAGO.World.Host.Infrastructure.Database;
using YAGO.World.Host.Parameters;

namespace YAGO.World.Host.Helpers.War
{
    public static class WarActionHelper
    {
        public static enWarActionStage CheckWarActionStage(
            Dictionary<enTypeOfWarrior, int> warriorCountByType, enWarActionStage currentWarActionStage)
        {
            var defendersInCastle = warriorCountByType[enTypeOfWarrior.TargetDefense];
            var defendersWithSupport = warriorCountByType.GetAllOnSide(false);
            var warriorsReadyToAttack = warriorCountByType.GetAllOnSide(true);

            if (warriorsReadyToAttack <= 0)
                return enWarActionStage.DefenderWin;
            else if (defendersWithSupport <= 0)
                return enWarActionStage.AgressorWin;
            else if (defendersInCastle == 0 || defendersWithSupport > 1.2 * warriorsReadyToAttack)
                return enWarActionStage.Battle;

            return currentWarActionStage;
        }

        public static int GetAllOnSide(this Dictionary<enTypeOfWarrior, int> warriorCountByType, bool isAgressor)
        {
            return isAgressor
                ? warriorCountByType
                    .Where(p => p.Key == enTypeOfWarrior.Agressor || p.Key == enTypeOfWarrior.AgressorSupport)
                    .Sum(p => p.Value)
                : warriorCountByType
                    .Where(p => p.Key == enTypeOfWarrior.TargetDefense || p.Key == enTypeOfWarrior.TargetSupport)
                    .Sum(p => p.Value);
        }

        public static (int, int) GetWarriorCountBySide(this Dictionary<enTypeOfWarrior, int> warriorCountByType)
        {
            var warriorsReadyToAttack = warriorCountByType.GetAllOnSide(true);
            var warriorsReadyToDefense = warriorCountByType.GetAllOnSide(false);
            return (warriorsReadyToAttack, warriorsReadyToDefense);
        }

        public static IEnumerable<Organization> GetAllDefenderDomains(ApplicationDbContext context, Organization targetDomain)
        {
            var defenders = new List<Organization> { targetDomain };

            if (targetDomain.Suzerain != null)
                defenders.Add(targetDomain.Suzerain);

            var relationDefenseDomains = DomainRelationsHelper.GetRelationDefenseDomains(context, targetDomain.Id);
            defenders.AddRange(relationDefenseDomains);

            return defenders.Distinct();
        }

        //TODO: Big
        public static int CalcRecomendedUnitCount(ApplicationDbContext context, Organization domain)
        {
            var unitsInDomain = domain.UnitsHere.Sum(u => u.Warriors);
            if (unitsInDomain == 0)
                return WarConstants.MinWarrioirsForAtack;

            var fort = domain.Fortifications;
            var unitCountForFirstWeek = Math.Max(unitsInDomain, fort / 2);

            var defenders = GetAllDefenderDomains(context, domain);
            var defenderIds = defenders.Select(d => d.Id);
            var neibourIds = context.GetNeighbors(domain.Id)
                .Select(d => d.Id);
            var unitsInNeibourDomains = context.Units
                .Where(u => defenderIds.Contains(u.DomainId))
                .Where(u => neibourIds.Contains(u.PositionDomainId.Value))
                .Sum(u => u.Warriors);
            var unitCountForSecondWeek = Math.Max(unitsInNeibourDomains + unitsInDomain, fort / 4);

            var allDefendersUnit = context.Units
                .Where(u => defenderIds.Contains(u.DomainId))
                .Sum(u => u.Warriors);
            var unitCountForThirdWeek = unitsInNeibourDomains + unitsInDomain +
                (allDefendersUnit - (unitsInNeibourDomains + unitsInDomain)) / 2;

            var minUnitCount = Math.Min(Math.Min(unitCountForFirstWeek, unitCountForSecondWeek), unitCountForThirdWeek);
            return minUnitCount / 500 * 500 + 500;
        }
    }
}
