using YAGO.World.Domain.Colonies.Reforms;
using YAGO.World.Domain.Stations;

namespace YAGO.World.Domain.Colonies.Policies
{
    public class ColonyPolicy
    {
        public ColonyReforms Reforms { get; }
        public Station? Station { get; private set; }
        public Asteroid? Asteroid { get; private set; }

        public ColonyPolicy(
            ColonyReforms reforms,
            Station? station,
            Asteroid? asteroid)
        {
            Reforms = reforms;
            Station = station;
            Asteroid = asteroid;
        }

        public static ColonyPolicy CreateNew()
        {
            var reforms = ColonyReforms.CreateNew();
            return new ColonyPolicy(
                reforms,
                station: null,
                asteroid: null);
        }

        internal void SetAsteroid(AsteroidId asteroidId)
        {
            Asteroid = AsteroidDataset.Get(asteroidId);
        }

        internal void SetStation(StationModelId stationId)
        {
            Station = StationModelDataset.Data[stationId];
        }
    }
}
