using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YSI.CurseOfSilverCrown.Core.Database.Characters;
using YSI.CurseOfSilverCrown.Core.Database.Commands;
using YSI.CurseOfSilverCrown.Core.Database.EventDomains;
using YSI.CurseOfSilverCrown.Core.Database.Events;
using YSI.CurseOfSilverCrown.Core.Database.Turns;
using YSI.CurseOfSilverCrown.Core.Database.Relations;
using YSI.CurseOfSilverCrown.Core.Database.Routes;
using YSI.CurseOfSilverCrown.Core.Database.Domains;
using YSI.CurseOfSilverCrown.Core.Database.Users;
using YSI.CurseOfSilverCrown.Core.Database.Units;
using YSI.CurseOfSilverCrown.Core.Database.Sessions;
using YSI.CurseOfSilverCrown.Core.Database.Errors;

namespace YSI.CurseOfSilverCrown.Core.Database
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Domain> Domains { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<Turn> Turns { get; set; }
        public DbSet<Event> EventStories { get; set; }
        public DbSet<EventDomain> OrganizationEventStories { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Relation> DomainRelations { get; set; }
        public DbSet<Session> GameSessions { get; set; }
        public DbSet<Character> Persons { get; set; }

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
            EventDomain.CreateModel(builder);
            Route.CreateModel(builder);
            Error.CreateModel(builder);
            Unit.CreateModel(builder);
            Relation.CreateModel(builder);
            Session.CreateModel(builder);
            Character.CreateModel(builder);
        }
    }
}
