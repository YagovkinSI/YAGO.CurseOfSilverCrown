using System;
using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Game.Map.Routes;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.Domains;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Game.War
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

        public static IEnumerable<Domain> GetAllDefenderDomains(ApplicationDbContext context, Domain targetDomain)
        {
            var defenders = new List<Domain> { targetDomain };

            if (targetDomain.Suzerain != null)
                defenders.Add(targetDomain.Suzerain);

            var relationDefenseDomains = DomainRelationsHelper.GetRelationDefenseDomains(context, targetDomain.Id);
            defenders.AddRange(relationDefenseDomains);

            return defenders.Distinct();
        }

        //TODO: Big
        public static int CalcRecomendedUnitCount(ApplicationDbContext context, Domain domain)
        {
            var unitsInDomain = domain.UnitsHere.Sum(u => u.Warriors);
            if (unitsInDomain == 0)
                return WarConstants.MinWarrioirsForAtack;

            var fort = domain.Fortifications;
            var unitCountForFirstWeek = Math.Max(unitsInDomain, fort / 2);

            var defenders = GetAllDefenderDomains(context, domain);
            var defenderIds = defenders.Select(d => d.Id);
            var neibourIds = RouteHelper.GetNeighbors(context, domain.Id)
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
            return (minUnitCount / 500) * 500 + 500;
        }
    }
}
