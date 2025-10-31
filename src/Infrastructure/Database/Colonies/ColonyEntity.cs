using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using YAGO.World.Domain.Colonies;
using YAGO.World.Infrastructure.Database.Cycles;
using YAGO.World.Infrastructure.Database.Users;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    public class ColonyEntity
    {
        public long Id { get; private set; }
        public long UserId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public decimal Solars { get; private set; }
        [Obsolete("Теперь расчитывается через BuildingIdsJson")]
        public decimal SolarsIncome { get; private set; }
        [Obsolete("Теперь расчитывается через BuildingIdsJson")]
        public decimal Reputation { get; private set; }
        [Obsolete("Теперь расчитывается через BuildingIdsJson")]
        public int Population { get; private set; }
        [Obsolete("Теперь расчитывается через BuildingIdsJson")]
        public int ZonesOccupied { get; private set; }
        public string BuildingIdsJson { get; private set; } = "[]";

        public virtual UserEntity? User { get; set; }
        public virtual List<CycleEntity>? Cycles { get; set; }

        protected ColonyEntity() { }

        public ColonyEntity(
            long id,
            long userId,
            string name,
            decimal solars,
            string buildingIdsJson)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Solars = solars;
            BuildingIdsJson = buildingIdsJson;
        }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<ColonyEntity>();
            model.HasKey(m => m.Id);

            model.HasOne(x => x.User).
                WithMany(x => x.Colonies).
                HasForeignKey(m => m.UserId);

            model.HasIndex(x => x.Name)
                .IsUnique();

            model.HasIndex(m => m.UserId);
        }

        internal void Update(Colony colony)
        {
            Name = colony.Name;
            Solars = colony.Solars;
            BuildingIdsJson = JsonConvert.SerializeObject(colony.BuildingIds);
        }

        internal void MoveToBuildingIds()
        {
            var buildingIds = SolarsIncome switch
            {
                50 => new long[] { 1, 1 },
                60 => new long[] { 2, 2 },
                70 => new long[] { 3, 3 },
                _ => throw new InvalidOperationException("Ошибка обновления UseBuildingIds!")
            };
            SolarsIncome = 0;
            Reputation = 0;
            Population = 0;
            ZonesOccupied = 0;
            BuildingIdsJson = JsonConvert.SerializeObject(buildingIds);
        }
    }
}
