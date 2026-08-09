using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Reforms;

namespace YAGO.World.Domain.Mappings
{
    internal static class PublicDebtContextMapping
    {
        public static PublicDebtContext ToPublicDebtContext(this ColonyState colonyState)
        {
            var yagoLevel = colonyState.GetYagoLevel();
            var debtLimit = yagoLevel switch
            {
                YagoLevel.Gray => 100_000,
                YagoLevel.Blue => 300_000,
                YagoLevel.Green => 1_000_000,
                YagoLevel.Gold => 3_000_000,
                _ => 0
            };
            return new PublicDebtContext(debtLimit);
        }
    }
}
