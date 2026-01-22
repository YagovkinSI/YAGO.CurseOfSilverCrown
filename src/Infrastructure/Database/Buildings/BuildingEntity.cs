using Microsoft.EntityFrameworkCore;

namespace YAGO.World.Infrastructure.Database.Buildings
{
    public class BuildingEntity
    {
        public long Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public int Cost { get; private set; }
        public int ZonesOccupied { get; private set; }
        public int SolarsIncome { get; private set; }
        public int Challenges { get; private set; }
        public int Population { get; private set; }
        public string[] Description { get; private set; } = new string[0];

        protected BuildingEntity() { }

        public BuildingEntity(
            long id,
            string name,
            int cost,
            int zonesOccupied,
            int solarsIncome,
            int challenges,
            int population,
            string[] description)
        {
            Id = id;
            Name = name;
            Cost = cost;
            ZonesOccupied = zonesOccupied;
            SolarsIncome = solarsIncome;
            Challenges = challenges;
            Population = population;
            Description = description;
        }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<BuildingEntity>();
            model.HasKey(m => m.Id);
        }
    }
}
