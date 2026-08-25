namespace YAGO.World.Domain.GameParameters
{
    public static class GameParameterIntegerHelper
    {
        public static bool IsInteger(this GameParameterType parameterType)
        {
            return parameterType switch
            {
                GameParameterType.SolarsCurrent or
                GameParameterType.SolarsDelta or
                GameParameterType.SolarDeltaIndustriesPrivate or
                GameParameterType.SolarDeltaIndustriesState or
                GameParameterType.PublicDebtService or
                GameParameterType.AdministrationSalary or
                GameParameterType.PopulationTaxSolars or
                GameParameterType.MoodCurrent or
                GameParameterType.MoodDelta => false,

                GameParameterType.ActionPointsCurrent or
                GameParameterType.ActionPointsDelta or
                GameParameterType.ModulesTotal or
                GameParameterType.ModulesUsed or
                GameParameterType.MiningSlotsFree or
                GameParameterType.TurnsCurrent or
                GameParameterType.Population or
                GameParameterType.ReformsTaxLevel or
                GameParameterType.ReformsSocialGuaranteesLevel => true,
            };
        }
    }
}
