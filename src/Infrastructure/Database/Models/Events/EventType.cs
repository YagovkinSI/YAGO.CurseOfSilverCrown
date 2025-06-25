namespace YAGO.World.Infrastructure.Database.Models.Events
{
    public enum EventType
    {
        Unknown = 0,

        #region События ПСК 1 (1 - 10 000)
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
        #endregion

        #region События Yago World (10 001 - 100 000)
        //10 001 - 11 000 - События связанные с военной частью игры
        //10 001 - 10 100 - События связаные с управлением юнитами
        //10 001 - 10 010 - Набор отряда
        //10 011 - 10 020 - Роспуск отряда
        DisbandmentUnit = 10_011,
        #endregion

        #region События ПСК 2 (101 000 +)
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
        #endregion
    }
}
