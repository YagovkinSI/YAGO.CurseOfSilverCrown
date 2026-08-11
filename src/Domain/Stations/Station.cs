using System;
using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.Stations
{
    public class Station : IEntity<Guid>
    {
        public Guid Id { get; }
        public Guid ColonyId { get; }
        public StationModel Model { get; }

        public Station(
            Guid id, 
            Guid colonyId, 
            StationModelId stationType)
        {
            Id = id;
            ColonyId = colonyId;
            Model = StationModelDataset.Data[stationType];
        }

        internal static Station CreateNew(
            Guid colonyId,
            StationModelId stationTypeId)
        {
            return new Station(
                id: Guid.NewGuid(),
                colonyId,
                stationTypeId);
        }
    }
}
