using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YAGO.World.Infrastructure.Database.Models.Commands;
using YAGO.World.Infrastructure.Database.Models.EventDomains;
using YAGO.World.Infrastructure.Database.Models.Events;
using YAGO.World.Infrastructure.Database.Models.Turns;
using YAGO.World.Infrastructure.Database.Models.Relations;
using YAGO.World.Infrastructure.Database.Models.Routes;
using YAGO.World.Infrastructure.Database.Models.Domains;
using YAGO.World.Infrastructure.Database.Models.Users;
using YAGO.World.Infrastructure.Database.Models.Units;
using YAGO.World.Infrastructure.Database.Models.Errors;

namespace YAGO.World.Infrastructure.Database
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Organization> Domains { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<Turn> Turns { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventObject> EventObjects { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Relation> Relations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            User.CreateModel(builder);
            Organization.CreateModel(builder);
            Command.CreateModel(builder);
            Turn.CreateModel(builder);
            Event.CreateModel(builder);
            EventObject.CreateModel(builder);
            Route.CreateModel(builder);
            Error.CreateModel(builder);
            Unit.CreateModel(builder);
            Relation.CreateModel(builder);
        }
    }
}
