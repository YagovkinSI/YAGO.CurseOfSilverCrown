namespace YAGO.World.Domain.GameParameters
{
    public static class GameParameterIntegerHelper
    {
        public static bool IsInteger(this GameParameterType parameterType)
        {
            return parameterType switch
            {
                GameParameterType.SolarsCurrent => false,

                GameParameterType.MiningSlotsFree or
                GameParameterType.TurnsCurrent or
                GameParameterType.Population => true,
            };
        }
    }
}
