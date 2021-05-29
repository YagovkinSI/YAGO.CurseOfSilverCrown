using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core.Database.Enums
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
        DestroyedUnit = 2005,

        //TaxCollection = 3,
        TaxCollection = 3001,

        //Investments = 4,
        Investments = 4001,

        //Investments = 6,
        Liberation = 6001,
        ChangeSuzerain = 6002,
        VoluntaryOath = 6003,

        //Fortifications = 7,
        Fortifications = 7001,

        //GoldTransfer = 8,
        GoldTransfer = 8001,

        //Auto 100000+
        //VasalTax
        VasalTax = 100001,

        //Maintenance = 101K,
        Maintenance = 101001,
        FortificationsMaintenance = 101002,

        //Mutiny = 102K,
        Mutiny = 102001,

        //Corruption = 103K,
        Corruption = 103001,

        //UnitMove = 104K,
        UnitMove = 104001,
        UnitCantMove = 104002,
    }
}
