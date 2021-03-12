using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using YAGO.World.Infrastructure.Database.Users;

namespace YAGO.World.Infrastructure.Database
{
    public class ApplicationDbContext : IdentityDbContext<UserEntity, IdentityRole<long>, long>
    {
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
        }
    }
}
