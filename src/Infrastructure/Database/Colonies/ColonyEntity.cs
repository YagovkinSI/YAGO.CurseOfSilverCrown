using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using YAGO.World.Infrastructure.Database.Cycles;
using YAGO.World.Infrastructure.Database.Users;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    public class ColonyEntity
    {
        public long Id { get; private set; }
        public long UserId { get; private set; }
        public string? Name { get; private set; }
        public decimal Solars { get; private set; }
        public decimal SolarsIncome { get; private set; }
        public decimal Reputation { get; private set; }
        public int Population { get; private set; }
        public int ZonesOccupied { get; private set; }
        public int ZonesTotal { get; private set; }

        public virtual UserEntity? User { get; set; }
        public virtual List<CycleEntity>? Cycles { get; set; }

        protected ColonyEntity() { }

        public ColonyEntity(
            long id,
            long userId,
            string? name,
            decimal solars,
            decimal solarsIncome,
            decimal reputation,
            int population,
            int zonesOccupied,
            int zonesTotal)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Solars = solars;
            SolarsIncome = solarsIncome;
            Reputation = reputation;
            Population = population;
            ZonesOccupied = zonesOccupied;
            ZonesTotal = zonesTotal;
        }

        internal static ColonyEntity CreateNew(
            long userId,
            string name,
            decimal solarsIncome,
            decimal repitation,
            int population)
        {
            return new ColonyEntity(
                id: default,
                userId: userId,
                name: name,
                solars: 1000,
                solarsIncome: solarsIncome,
                reputation: repitation,
                population: population,
                zonesOccupied: 4000,
                zonesTotal: 10000
            );
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
    }
}
