using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;

namespace YSI.CurseOfSilverCrown.Core.Database.EF
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Domain> Domains { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<Turn> Turns { get; set; }
        public DbSet<EventStory> EventStories { get; set; }
        public DbSet<DomainEventStory> OrganizationEventStories { get; set; }
        internal DbSet<Route> Routes { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<DomainRelation> DomainRelations { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<Person> Persons { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            User.CreateModel(builder);
            Domain.CreateModel(builder);
            Command.CreateModel(builder);
            Turn.CreateModel(builder);
            EventStory.CreateModel(builder);
            DomainEventStory.CreateModel(builder);
            Route.CreateModel(builder);
            Error.CreateModel(builder);
            Unit.CreateModel(builder);
            DomainRelation.CreateModel(builder);
            GameSession.CreateModel(builder);
            Person.CreateModel(builder);
        }
    }
}
