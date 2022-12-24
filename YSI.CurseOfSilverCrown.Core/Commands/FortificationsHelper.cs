namespace YSI.CurseOfSilverCrown.Core.Commands
{
    public static class FortificationsHelper
    {
        public static int GetDefencePercent(int fortifications)
        {
            var defencePercent = 100;
            if (fortifications > 18000)
                defencePercent = 200 + ((fortifications - 18000) / 500);
            else if (fortifications > 5000)
                defencePercent = 150 + ((fortifications - 5000) / 300);
            else if (fortifications > 0)
                defencePercent = 100 + ((fortifications) / 100);
            return defencePercent;
        }

        public static double GetWariorDefenseCoeficient(double commandCoefficient, int fortifications)
        {
            var fortBouns = GetDefencePercent(fortifications);
            return commandCoefficient * fortBouns / 100.0;
        }
    }
}
