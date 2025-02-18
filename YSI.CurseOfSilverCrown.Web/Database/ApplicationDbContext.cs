using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YSI.CurseOfSilverCrown.Web.Database.Commands;
using YSI.CurseOfSilverCrown.Web.Database.EventDomains;
using YSI.CurseOfSilverCrown.Web.Database.Events;
using YSI.CurseOfSilverCrown.Web.Database.Turns;
using YSI.CurseOfSilverCrown.Web.Database.Relations;
using YSI.CurseOfSilverCrown.Web.Database.Routes;
using YSI.CurseOfSilverCrown.Web.Database.Domains;
using YSI.CurseOfSilverCrown.Web.Database.Users;
using YSI.CurseOfSilverCrown.Web.Database.Units;
using YSI.CurseOfSilverCrown.Web.Database.Errors;

namespace YSI.CurseOfSilverCrown.Web.Database
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
