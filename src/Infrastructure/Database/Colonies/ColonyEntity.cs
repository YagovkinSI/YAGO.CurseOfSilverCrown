using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        public int Solars { get; private set; }
        public string BuildingIdsJson { get; private set; } = "[]";
        public string StatesJson { get; private set; } = "[]";

        public virtual UserEntity? User { get; set; }
        public virtual List<CycleEntity>? Cycles { get; set; }

        protected ColonyEntity() { }

        public ColonyEntity(
            long id,
            long userId,
            string name,
            int solars,
            string buildingIdsJson,
            string statesJson)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Solars = solars;
            BuildingIdsJson = buildingIdsJson;
            StatesJson = statesJson;
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

            var colonyParameters = new ColonyParameters(colony.CodeOfLaws, colony.Contracts);
            StatesJson = JsonConvert.SerializeObject(colonyParameters);
        }

        internal void SetStatesJson(ColonyParameters colonyParameters)
        {
            StatesJson = JsonConvert.SerializeObject(colonyParameters);
            BuildingIdsJson = "[]";
        }
    }
}
