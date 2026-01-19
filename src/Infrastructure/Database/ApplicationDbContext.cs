using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using YAGO.World.Infrastructure.Database.Buildings;
using YAGO.World.Infrastructure.Database.Colonies;
using YAGO.World.Infrastructure.Database.Cycles;
using YAGO.World.Infrastructure.Database.Users;

namespace YAGO.World.Infrastructure.Database
{
    public class ApplicationDbContext : IdentityDbContext<UserEntity, IdentityRole<long>, long>
    {
        public DbSet<ColonyEntity> Colonies { get; set; }
        public DbSet<CycleEntity> Cycles { get; set; }
        [Obsolete]
        public DbSet<BuildingEntity> Buildings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .ConfigureWarnings(warnings =>
                    warnings.Ignore(RelationalEventId.PendingModelChangesWarning))
                .UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            UserEntity.CreateModel(builder);
            ColonyEntity.CreateModel(builder);
            BuildingEntity.CreateModel(builder);
        }
    }
}
