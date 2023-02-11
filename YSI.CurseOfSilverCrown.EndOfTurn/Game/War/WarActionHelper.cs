using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.EndOfTurn.Actions;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Game.War
{
    internal static class WarActionHelper
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
            else if(defendersInCastle == 0 || defendersWithSupport > 1.2 * warriorsReadyToAttack)
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
    }
}
