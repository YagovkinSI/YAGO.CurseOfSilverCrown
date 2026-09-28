using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Stations;

namespace YAGO.World.Domain.Services
{
    /// <summary>
    /// Доступ к данным выбранной станции по идентификатору из набора (Dataset).
    /// Colony хранит только StationId; сам объект станции живёт в StationModelDataset.
    /// </summary>
    public static class ColonyStationExtensions
    {
        public static Station? GetStation(this ColonyState colonyState)
        {
            return StationModelDataset.Find(colonyState.Policy.StationId);
        }

        public static string? GetStationName(this ColonyState colonyState)
        {
            return colonyState.GetStation()?.Name;
        }

        /// <summary>
        /// Суммарное число модулей на выбранной станции (или 0, если станция не выбрана).
        /// </summary>
        public static int GetStationModulesTotal(this ColonyState colonyState)
        {
            return colonyState.GetStation()?.ModulesTotal ?? 0;
        }
    }
}