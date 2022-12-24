namespace YSI.CurseOfSilverCrown.Core.Commands
{
    public static class InvestmentsHelper
    {
        public const int StartInvestment = 50000;

        public static int GetInvestmentTax(int investments)
        {
            var profit = 0;
            if (investments > 300000)
                profit = 70000 + (int)((investments - 300000) * 0.1);
            else if (investments > 100000)
                profit = 30000 + (int)((investments - 100000) * 0.2);
            else if (investments > 0)
                profit = (int)(investments * 0.3);
            return profit;
        }
    }
}
