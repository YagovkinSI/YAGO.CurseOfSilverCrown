using System;

namespace YAGO.World.Domain.Stations
{
    public class Station
    {
        public Guid Id { get; }
        public StationModel Model { get; }

        public Station(
            Guid id,
            StationModelId stationType)
        {
            Id = id;
            Model = StationModelDataset.Data[stationType];
        }

        internal static Station CreateNew(
            StationModelId stationTypeId)
        {
            return new Station(
                id: Guid.NewGuid(),
                stationTypeId);
        }
    }
}
