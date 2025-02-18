using System;
using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Web.Database;
using YSI.CurseOfSilverCrown.Web.Database.Units;
using YSI.CurseOfSilverCrown.Web.Helpers.War;

namespace YSI.CurseOfSilverCrown.Web.Helpers.Actions.War
{
    internal class WarActionParameters
    {
        public Unit AgressorUnit { get; }
        public List<WarActionMember> WarActionMembers { get; }
        public bool IsBreached { get; set; }
        public int TargetDomainId { get; }
        public enWarActionStage WarActionStage { get; set; }
        public double CurrentFortifications { get; set; }
        public int DayOfWar { get; set; }

        public bool WarIsOver => WarActionStage == enWarActionStage.AgressorWin || WarActionStage == enWarActionStage.DefenderWin;
        public bool IsVictory => WarActionStage == enWarActionStage.AgressorWin;

        public WarActionParameters(ApplicationDbContext context, Unit unit)
        {
            AgressorUnit = unit;
            TargetDomainId = unit.TargetDomainId.Value;
            CurrentFortifications = unit.Target.Fortifications;

            var membersFindTask = new WarActionMembersFindTask(context, unit);
            WarActionMembers = membersFindTask.Execute();

            WarActionStage = enWarActionStage.Siege;
            DayOfWar = 0;
        }

        public int GetWarriorCount(params enTypeOfWarrior[] types)
        {
            return WarActionMembers
                .Where(m => types.Contains(m.Type))
                .Where(m => m.IsReadyToBattle(DayOfWar))
                .Sum(m => m.WarriorsOnStart - m.WarriorLosses);
        }

        public Dictionary<enTypeOfWarrior, int> GetWarriorCountByType()
        {
            var dict = new Dictionary<enTypeOfWarrior, int>
            {
                { enTypeOfWarrior.Agressor, GetWarriorCount(enTypeOfWarrior.Agressor) },
                { enTypeOfWarrior.AgressorSupport, GetWarriorCount(enTypeOfWarrior.AgressorSupport) },
                { enTypeOfWarrior.TargetDefense, GetWarriorCount(enTypeOfWarrior.TargetDefense) },
                { enTypeOfWarrior.TargetSupport, GetWarriorCount(enTypeOfWarrior.TargetSupport) }
            };
            return dict;
        }

        public void AddLosses(bool isPercent, int value, params enTypeOfWarrior[] types)
        {
            var members = WarActionMembers
                .Where(m => types.Contains(m.Type))
                .Where(m => m.IsReadyToBattle(DayOfWar));
            if (!members.Any())
                return;
            var percent = isPercent
                ? value / 100.0
                : Math.Min(1, (double)value / members.Sum(m => m.WarriorsOnStart - m.WarriorLosses));
            foreach (var member in members)
            {
                member.SetLost(percent);
            }
        }
    }
}
