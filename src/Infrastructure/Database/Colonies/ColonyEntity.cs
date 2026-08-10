using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using YAGO.World.Infrastructure.Database.Cycles;
using YAGO.World.Infrastructure.Database.Users;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    public class ColonyEntity
    {
        public Guid Id { get; private set; }
        [Updatable]
        public long UserId { get; private set; }
        [Updatable]
        public string Name { get; private set; } = string.Empty;
        [Updatable]
        public double Solars { get; private set; }
        [Updatable]
        public string StatesJson { get; private set; } = "[]";
        [Obsolete]
        public bool Deactivated { get; private set; }
        [Obsolete]
        public DateTime? DeactivateAtUtc { get; private set; }

        public virtual UserEntity? User { get; set; }
        public virtual List<CycleEntity>? Cycles { get; set; }

        protected ColonyEntity() { }

        public ColonyEntity(
            Guid id,
            long userId,
            string name,
            double solars,
            string statesJson)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Solars = solars;
            StatesJson = statesJson;
        }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<ColonyEntity>();
            model.HasKey(m => m.Id);

            model.HasOne(x => x.User)
                .WithMany(x => x.Colonies)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            model.HasIndex(x => x.Name)
                .IsUnique();

            model.HasIndex(m => m.UserId);
        }

        internal void SetStatesJson(ColonyParameters colonyParameters)
        {
            StatesJson = JsonConvert.SerializeObject(colonyParameters);
        }

        internal void AddSolars(int solars)
        {
            Solars += solars;
        }
    }
}
