namespace YAGO.World.Domain.Colonies
{
    public enum ColonyParameterType
    {
        Unknown = 0,

        //0xxxxx Level0
        //00xxxx BaseParams
        Solars = 1,
        GavernorType = 2,
        Population = 3,
        ZonesOccupied = 4,
        SolarIncome = 5,

        //01xxxx Companies
        //011xxx Minning
        //011xxx Companies
        EngineeringTeam = 01_10_10,
        MiningBrigade = 01_10_20,
        RehabilitationContingent = 01_10_30,
    }
}
