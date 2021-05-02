using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Utils;

namespace YSI.CurseOfSilverCrown.Core.Commands
{
    public static class IdlenessHelper
    {
        public static int GetOptimizedCoffers()
        {
            return RandomHelper.AddRandom(Constants.MinIdleness, roundRequest: -1);
        }

        public static bool IsOptimized(int coffers)
        {
            return coffers <= 3500;
        }
    }
}
