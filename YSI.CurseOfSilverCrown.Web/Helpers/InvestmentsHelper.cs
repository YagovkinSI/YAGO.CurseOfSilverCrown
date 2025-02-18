using System;

namespace YSI.CurseOfSilverCrown.Web.Helpers
{
    public static class InvestmentsHelper
    {
        public const int StartInvestment = 50000;

        public static int GetInvestmentTax(int investments)
        {
            var currentCalcInvestments = investments;
            var tax = 0;
            
            var stepCoefs = new[] { 2, 1, 3, 4, 6, int.MaxValue / StartInvestment };
            var taxCoefs = new[] { 0.25, 0.1, 0.05, 0.02, 0.01, 0.004 };
            var stepIndex = 0;
            while (currentCalcInvestments > 0 && stepCoefs.Length > stepIndex)
            {
                var stepMax = StartInvestment * stepCoefs[stepIndex];
                var step = Math.Min(currentCalcInvestments, stepMax);
                tax += (int)Math.Floor(step * taxCoefs[stepIndex]);
                currentCalcInvestments -= step;
                stepIndex++;
            }
            return tax;
        }
    }
}
