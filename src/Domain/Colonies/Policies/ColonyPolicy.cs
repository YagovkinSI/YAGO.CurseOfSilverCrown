using YAGO.World.Domain.Colonies.Reforms;

namespace YAGO.World.Domain.Colonies.Policies
{
    public class ColonyPolicy
    {
        public ColonyReforms Reforms { get; }
        public string? StationId { get; private set; }
        public string? AsteroidId { get; private set; }

        public ColonyPolicy(
            ColonyReforms reforms,
            string? stationId,
            string? asteroidId)
        {
            Reforms = reforms;
            StationId = stationId;
            AsteroidId = asteroidId;
        }

        public static ColonyPolicy CreateNew()
        {
            var reforms = ColonyReforms.CreateNew();
            return new ColonyPolicy(
                reforms,
                stationId: null,
                asteroidId: null);
        }

        internal void SetAsteroid(string asteroidCode)
        {
            AsteroidId = asteroidCode;
        }

        internal void SetStation(string stationCode)
        {
            StationId = stationCode;
        }
    }
}