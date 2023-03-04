using System;
using YSI.CurseOfSilverCrown.Core.Helpers.War;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Actions.War
{
    internal class WarActionRetreatCheckTask
    {
        private readonly WarActionParameters _warActionParameters;

        public WarActionRetreatCheckTask(WarActionParameters warActionParameters)
        {
            _warActionParameters = warActionParameters;
        }

        public void Execute()
        {
            var warriorCountByType = _warActionParameters.GetWarriorCountByType();
            _warActionParameters.WarActionStage =
                WarActionHelper.CheckWarActionStage(warriorCountByType, _warActionParameters.WarActionStage);

            var (warriorsReadyToAttack, warriorsReadyToDefense) = warriorCountByType.GetWarriorCountBySide();
            if (warriorsReadyToAttack == 0 || warriorsReadyToDefense == 0)
                return;

            RecalcMorality(warriorsReadyToDefense, warriorsReadyToAttack);
        }

        private void RecalcMorality(int warriorsReadyToDefense, int warriorsReadyToAttack)
        {
            foreach (var member in _warActionParameters.WarActionMembers)
            {
                if (!member.IsReadyToBattle(_warActionParameters.DayOfWar))
                    continue;

                var moralityDelta = _warActionParameters.WarActionStage switch
                {
                    enWarActionStage.Siege => GetMoralityDeltaForSiege(member, warriorsReadyToAttack, warriorsReadyToDefense),
                    enWarActionStage.Assault => GetMoralityDeltaForAssault(member, warriorsReadyToAttack, warriorsReadyToDefense),
                    enWarActionStage.Battle => GetMoralityDeltaForBattle(member, warriorsReadyToAttack, warriorsReadyToDefense),
                    _ => 0
                };

                member.Morality += moralityDelta;
                member.Morality = Math.Min(Math.Max(member.Morality, 0), 100);
            }
        }

        private int GetMoralityDeltaForSiege(WarActionMember member, int warriorsReadyToAttack, int warriorsReadyToDefense)
        {
            var ratio = member.IsAgressor
                ? (double)warriorsReadyToAttack / warriorsReadyToDefense
                : (double)warriorsReadyToDefense / warriorsReadyToAttack;
            var deltaDefault = (ratio - 1) * 100;

            if (member.IsAgressor)
            {
                return deltaDefault > 0
                    ? -2
                    : (int)deltaDefault * 2;
            }
            else
            {
                return deltaDefault > 0
                    ? (int)deltaDefault
                    : -4;
            }
        }

        private int GetMoralityDeltaForAssault(WarActionMember member, int warriorsReadyToAttack, int warriorsReadyToDefense)
        {
            var ratio = member.IsAgressor
                ? (double)warriorsReadyToAttack / warriorsReadyToDefense
                : (double)warriorsReadyToDefense / warriorsReadyToAttack;
            var deltaDefault = (ratio - 1) * 100;
            return (int)deltaDefault / 5;
        }

        private int GetMoralityDeltaForBattle(WarActionMember member, int warriorsReadyToAttack, int warriorsReadyToDefense)
        {
            var ratio = member.IsAgressor
                ? (double)warriorsReadyToAttack / warriorsReadyToDefense
                : (double)warriorsReadyToDefense / warriorsReadyToAttack;
            var deltaDefault = (ratio - 1) * 100;
            return (int)deltaDefault;
        }
    }
}
