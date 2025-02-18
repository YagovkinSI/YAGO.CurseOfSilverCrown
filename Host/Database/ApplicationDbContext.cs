using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YAGO.World.Host.Database.Commands;
using YAGO.World.Host.Database.EventDomains;
using YAGO.World.Host.Database.Events;
using YAGO.World.Host.Database.Turns;
using YAGO.World.Host.Database.Relations;
using YAGO.World.Host.Database.Routes;
using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Database.Users;
using YAGO.World.Host.Database.Units;
using YAGO.World.Host.Database.Errors;

namespace YAGO.World.Host.Database
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Domain> Domains { get; set; }
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
            Domain.CreateModel(builder);
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
