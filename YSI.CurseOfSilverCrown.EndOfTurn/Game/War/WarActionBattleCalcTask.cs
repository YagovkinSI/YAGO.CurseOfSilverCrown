using System;
using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.EndOfTurn.Actions;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Game.War
{
    internal class WarActionBattleCalcTask
    {
        private readonly ApplicationDbContext _context;
        private readonly Unit _agressorUnit;
        private readonly List<WarActionMember> _warMembers;

        public WarActionBattleCalcTask(ApplicationDbContext context, Unit agressorUnit, List<WarActionMember> warMembers)
        {
            _context = context;
            _agressorUnit = agressorUnit;
            _warMembers = warMembers;
        }

        public (bool, bool) Execute()
        {
            var breached = CalcBreach(_warMembers, out var siegeLosses);
            var isVictory = breached && CalcVictory(_warMembers, siegeLosses);

            CalcLossesInSiege(_warMembers, siegeLosses);
            if (breached)
                CalcLossesInCombats(_warMembers, isVictory);

            return (breached, isVictory);
        }

        private bool CalcBreach(List<WarActionMember> warMembers, out int losses)
        {
            var fortification = _agressorUnit.Target.Fortifications;
            var warrioirsInCastle = warMembers
                .Where(p => p.Type == enTypeOfWarrior.TargetDefense)
                .Sum(p => p.WarriorsOnStart);
            var besiegers = warMembers
                .Where(p => p.IsAgressor)
                .Sum(p => p.WarriorsOnStart);

            losses = 0;
            if (fortification < 1 || warrioirsInCastle < 1)
                return true;

            var breached = false;
            var garrisonDamage = FortificationsHelper.GetGarrisonDamage(fortification, warrioirsInCastle);
            var chanceToNotBreach = 0.8;
            while (!breached && losses < besiegers / 10)
            {
                var random = new Random().NextDouble();
                breached = random > chanceToNotBreach;
                losses += RandomHelper.AddRandom(garrisonDamage, 20);
                chanceToNotBreach /= 1.1;
            }
            return breached;
        }

        private bool CalcVictory(List<WarActionMember> warMembers, int siegeLosses)
        {
            var agressotWarrioirCount = warMembers
                .Where(p => p.IsAgressor)
                .Sum(p => p.WarriorsOnStart)
                - siegeLosses;
            var targetWarrioirCount = warMembers
                .Where(p => !p.IsAgressor)
                .Sum(p => p.WarriorsOnStart)
                - siegeLosses / 20;

            var agressotPowerResult = RandomHelper.AddRandom(agressotWarrioirCount, 20);
            var targetPowerResult = RandomHelper.AddRandom(targetWarrioirCount * 1.1, 20);

            return agressotPowerResult >= targetPowerResult;
        }

        private void CalcLossesInSiege(List<WarActionMember> warMembers, int siegeLosses)
        {
            var agressotWarriorsCount = warMembers
                   .Where(p => p.IsAgressor)
                   .Sum(p => p.WarriorsOnStart);
            var targetWarriorsCount = warMembers
                .Where(p => p.Type == enTypeOfWarrior.TargetDefense)
                .Sum(p => p.WarriorsOnStart);

            var siegeAgressorLossesPercent = (double)siegeLosses / agressotWarriorsCount;
            var siegeTargetLossesPercent = targetWarriorsCount == 0
                ? 0
                : RandomHelper.AddRandom((double)siegeLosses / 20 / targetWarriorsCount, 20);

            warMembers
                .Where(p => p.Type != enTypeOfWarrior.TargetSupport)
                .ToList()
                .ForEach(p => p.SetLost(p.IsAgressor ? siegeAgressorLossesPercent : siegeTargetLossesPercent));

            _context.UpdateRange(warMembers.Select(p => p.Unit));
        }

        private void CalcLossesInCombats(List<WarActionMember> warMembers, bool isVictory)
        {
            var agressotWarriorsCount = warMembers
                .Where(p => p.IsAgressor)
                .Sum(p => p.WarriorsOnStart - p.WarriorLosses);
            var targetWarriorsCount = warMembers
                .Where(p => !p.IsAgressor)
                .Sum(p => p.WarriorsOnStart - p.WarriorLosses);

            var random = new Random();
            var agressorLossesPercentDefault = WarConstants.AgressorLost +
                random.NextDouble() / 20 +
                (isVictory ? 0 : 0.05 + random.NextDouble() / 20);
            var targetLossesPercentDefault = WarConstants.TargetLost +
                random.NextDouble() / 20 +
                (!isVictory ? 0 : 0.05 + random.NextDouble() / 20);
            var agressorLossesPercent = agressotWarriorsCount <= targetWarriorsCount
                ? agressorLossesPercentDefault
                : agressorLossesPercentDefault * ((double)targetWarriorsCount / agressotWarriorsCount);
            var targetLossesPercent = agressotWarriorsCount >= targetWarriorsCount
                ? targetLossesPercentDefault
                : targetLossesPercentDefault * ((double)agressotWarriorsCount / targetWarriorsCount);

            warMembers.ForEach(p => p.SetLost(p.IsAgressor ? agressorLossesPercent : targetLossesPercent));

            _context.UpdateRange(warMembers.Select(p => p.Unit));
        }
    }
}
