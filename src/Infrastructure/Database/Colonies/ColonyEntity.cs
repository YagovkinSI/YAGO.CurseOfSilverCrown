using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YAGO.World.Domain.Common;
using YAGO.World.Infrastructure.Database.Turns;
using YAGO.World.Infrastructure.Database.Users;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    public class ColonyEntity : IEntity<Guid>
    {
        public Guid Id { get; private set; }
        public long UserId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public double Solars { get; private set; }
        public string JsonData { get; private set; } = "{}";
        [Timestamp]
        public uint Version { get; private set; }

        public virtual UserEntity? User { get; set; }
        public virtual List<TurnEntity>? Turns { get; set; }

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
            JsonData = statesJson;
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
            JsonData = JsonConvert.SerializeObject(colonyParameters);
        }

        internal void AddSolars(int solars)
        {
            Solars += solars;
        }
    }
}
