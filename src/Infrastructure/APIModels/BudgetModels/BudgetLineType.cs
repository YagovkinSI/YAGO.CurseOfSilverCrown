namespace YAGO.World.Infrastructure.APIModels.BudgetModels
{
    public enum BudgetLineType
    {
        Current = 0,

        Idleness = 1,
        Maintenance = 2,
        Growth = 3,
        BaseTax = 4,
        VassalTax = 5,
        SuzerainTax = 6,
        War = 7,
        Investments = 8,
        WarSupportDefense = 9,
        InvestmentProfit = 10,
        AditionalTax = 11,
        Fortifications = 12,
        FortificationsMaintenance = 13,
        GoldTransfer = 14,
        Rebelion = 15,
        WarSupportAtack = 16,

        VassalTransfer = 70,

        NotAllocated = 90,
        Total = 100
    }
}
