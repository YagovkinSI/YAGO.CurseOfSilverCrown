using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YSI.CurseOfSilverCrown.Core.MainModels.Characters;
using YSI.CurseOfSilverCrown.Core.MainModels.Errors;
using YSI.CurseOfSilverCrown.Core.MainModels.Commands;
using YSI.CurseOfSilverCrown.Core.MainModels.Domains;
using YSI.CurseOfSilverCrown.Core.MainModels.Events;
using YSI.CurseOfSilverCrown.Core.MainModels.GameRelations;
using YSI.CurseOfSilverCrown.Core.MainModels.Routes;
using YSI.CurseOfSilverCrown.Core.MainModels.Turns;
using YSI.CurseOfSilverCrown.Core.MainModels.Units;
using YSI.CurseOfSilverCrown.Core.MainModels.Sessions;
using YSI.CurseOfSilverCrown.Core.MainModels.Users;
using YSI.CurseOfSilverCrown.Core.MainModels.EventDomains;

namespace YSI.CurseOfSilverCrown.Core.MainModels
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
            Events.Event.CreateModel(builder);
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
