namespace YAGO.World.Domain.Stations
{
    public class Asteroid
    {
        public AsteroidId Id { get; }
        public string Name { get; }

        /// <summary>
        /// Расстояние до Цереры (в астрономических единицах)
        /// </summary>
        public double DistanceToCeres { get; }

        /// <summary>
        /// Расстояние до Авалона (в астрономических единицах)
        /// </summary>
        public double DistanceToAvalon { get; }

        /// <summary>
        /// Лимит добывающих модулей на астероиде
        /// </summary>
        public int MiningModulesLimit { get; }

        public Asteroid(
            AsteroidId id,
            string name,
            double distanceToCeres,
            double distanceToAvalon,
            int miningModulesLimit)
        {
            Id = id;
            Name = name;
            DistanceToCeres = distanceToCeres;
            DistanceToAvalon = distanceToAvalon;
            MiningModulesLimit = miningModulesLimit;
        }
    }
}