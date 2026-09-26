using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Stations;

namespace YAGO.World.Domain.Services
{
    /// <summary>
    /// Доступ к данным выбранного астероида по идентификатору из набора (Dataset).
    /// Colony хранит только AsteroidId; сам объект астероида живёт в AsteroidDataset.
    /// </summary>
    public static class ColonyAsteroidExtensions
    {
        public static Asteroid? GetAsteroid(this ColonyState colonyState)
        {
            return AsteroidDataset.Find(colonyState.Policy.AsteroidId);
        }

        public static string? GetAsteroidName(this ColonyState colonyState)
        {
            return colonyState.GetAsteroid()?.Name;
        }

        /// <summary>
        /// Лимит добывающих модулей на выбранном астероиде (или 0, если астероид не выбран).
        /// </summary>
        public static int GetAsteroidMiningModulesLimit(this ColonyState colonyState)
        {
            return colonyState.GetAsteroid()?.MiningModulesLimit ?? 0;
        }

        /// <summary>
        /// Логистический эффект близости к Церере, рассчитываемый по расстоянию до выбранного астероида.
        /// </summary>
        public static double GetLogisticEffect(this ColonyState colonyState)
        {
            return colonyState.GetAsteroid()?.DistanceToCeres switch
            {
                > 0.5 => 1.0,
                < 0.4 => 1.03,
                _ => 1.015
            };
        }
    }
}