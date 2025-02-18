namespace YSI.CurseOfSilverCrown.Web.Database.Events
{
    public enum EventType
    {
        //Commands 1000+
        //Idleness = 0,
        Idleness = 1,

        //Growth = 1,
        Growth = 1001,
        GrowthLevelI = 1002,
        GrowthLevelII = 1003,
        GrowthLevelIII = 1004,
        GrowthLevelIV = 1005,
        GrowthLevelV = 1006,

        //War = 2,
        FastWarSuccess = 2001,
        FastWarFail = 2002,
        FastRebelionSuccess = 2003,
        FastRebelionFail = 2004,
        DestroyedUnit = 2005,
        SiegeFail = 2006,

        //TaxCollection = 3,
        TaxCollection = 3001,

        //Investments = 4,
        Investments = 4001,
        InvestmentsLevelI = 4002,
        InvestmentsLevelII = 4003,
        InvestmentsLevelIII = 4004,
        InvestmentsLevelIV = 4005,
        InvestmentsLevelV = 4006,

        //Investments = 6,
        Liberation = 6001,
        ChangeSuzerain = 6002,
        VoluntaryOath = 6003,

        //Fortifications = 7,
        Fortifications = 7001,
        FortificationsLevelI = 7002,
        FortificationsLevelII = 7003,
        FortificationsLevelIII = 7004,
        FortificationsLevelIV = 7005,
        FortificationsLevelV = 7006,

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

        //NegativeEvents
        TownFire = 105001,
        CastleFire = 105002,
        Disease = 105003,
    }
}
