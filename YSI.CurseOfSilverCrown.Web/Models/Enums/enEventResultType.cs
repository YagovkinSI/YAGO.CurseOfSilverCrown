using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Web.Enums
{
    public enum enEventResultType
    {
        //Commands 1000+
        //Idleness = 0,
        Idleness = 1,

        //Growth = 1,
        Growth = 1001,

        //War = 2,
        FastWarSuccess = 2001,
        FastWarFail = 2002,
        FastRebelionSuccess = 2003,
        FastRebelionFail = 2004,

        //TaxCollection = 3,
        TaxCollection = 3001,

        //Investments = 4,
        Investments = 4001,

        //Auto 100000+
        //VasalTax
        VasalTax = 100001,


        //Maintenance = 101K,
        Maintenance = 101001,

        //Mutiny = 102K,
        Mutiny = 102001,
    }
}
