using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core.Commands
{
    public static class InvestmentsHelper
    {
        private const int MaxInvestment = 50000;
        private const int MaxProfit = 4500;

        public const int IlusionInvestment = 250000;

        public static double GetCoeficient()
        {
            return (double)MaxProfit * MaxProfit / MaxInvestment * 1.5;
        }

        public static int GetInvestmentTax(int investments)
        {
            var koef = InvestmentsHelper.GetCoeficient();
            return (int)Math.Round(Math.Sqrt(investments * koef));
        }
    }
}
