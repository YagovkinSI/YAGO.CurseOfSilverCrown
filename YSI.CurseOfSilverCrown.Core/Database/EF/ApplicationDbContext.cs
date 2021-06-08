using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.PregenDatas;

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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            CreateUsers(builder);
            CreateOrganizations(builder);
            CreateCommands(builder);
            CreateTurns(builder);
            CreateEventStories(builder);
            CreateOrganizationEventStories(builder);
            CreateRoutes(builder);
            CreateErrors(builder);
            CreateUnits(builder);
            CreateDomainRelations(builder);
            CreateGameSessions(builder);
        }

        private void CreateUsers(ModelBuilder builder)
        {
            var model = builder.Entity<User>();
            model.HasKey(m => m.Id);
            model.HasOne(m => m.Domain)
                .WithOne(m => m.User)
                .HasForeignKey<User>(m => m.DomainId);
            model.HasIndex(m => m.DomainId);
        }

        private void CreateOrganizations(ModelBuilder builder)
        {
            var model = builder.Entity<Domain>();
            model.HasKey(m => m.Id);
            model.HasOne(m => m.User)
                .WithOne(m => m.Domain)
                .HasForeignKey<User>(m => m.DomainId);
            model.HasOne(m => m.Suzerain)
                .WithMany(m => m.Vassals)
                .HasForeignKey(m => m.SuzerainId);

            model.HasIndex(m => m.SuzerainId);
            model.HasIndex(m => m.MoveOrder)
                .IsUnique();

            model.HasData(PregenData.Organizations);
        }

        private void CreateCommands(ModelBuilder builder)
        {
            var model = builder.Entity<Command>();
            model.HasKey(m => m.Id);
            model.HasOne(m => m.Domain)
                .WithMany(m => m.Commands)
                .HasForeignKey(m => m.DomainId);
            model.HasOne(m => m.Target)
                .WithMany(m => m.ToDomainCommands)
                .HasForeignKey(m => m.TargetDomainId);
            model.HasOne(m => m.Target2)
                .WithMany(m => m.ToDomain2Commands)
                .HasForeignKey(m => m.Target2DomainId);
            model.HasIndex(m => m.InitiatorDomainId);
            model.HasIndex(m => m.DomainId);
            model.HasIndex(m => m.Type);
            model.HasIndex(m => m.TargetDomainId);
        }

        private void CreateTurns(ModelBuilder builder)
        {
            var model = builder.Entity<Turn>();
            model.HasKey(m => m.Id);

            model.HasData(PregenData.GetFirstTurn());
        }

        private void CreateEventStories(ModelBuilder builder)
        {
            var model = builder.Entity<EventStory>();
            model.HasKey(m => new { m.TurnId, m.Id });
            model.HasOne(m => m.Turn)
                .WithMany(m => m.EventStories)
                .HasForeignKey(m => m.TurnId);

        }

        private void CreateOrganizationEventStories(ModelBuilder builder)
        {
            var model = builder.Entity<DomainEventStory>();
            model.HasKey(m => new { m.TurnId, m.DomainId, m.EventStoryId });
            model.HasOne(m => m.Turn)
                .WithMany(m => m.OrganizationEventStories)
                .HasForeignKey(m => m.TurnId)
                .OnDelete(DeleteBehavior.Restrict);
            model.HasOne(m => m.EventStory)
                .WithMany(m => m.DomainEventStories)
                .HasForeignKey(m => new { m.TurnId, m.EventStoryId });
            model.HasOne(m => m.Domain)
                .WithMany(m => m.DomainEventStories)
                .HasForeignKey(m => m.DomainId);
        }

        private void CreateRoutes(ModelBuilder builder)
        {
            var model = builder.Entity<Route>();
            model.HasKey(m => new { m.FromDomainId, m.ToDomainId });
            model.HasIndex(m => m.FromDomainId);
            model.HasIndex(m => m.ToDomainId);
            model.HasOne(m => m.FromDomain)
                .WithMany(m => m.RouteFromHere)
                .HasForeignKey(m => m.FromDomainId)
                .OnDelete(DeleteBehavior.Restrict);
            model.HasOne(m => m.ToDomain)
                .WithMany(m => m.RouteToHere)
                .HasForeignKey(m => m.ToDomainId)
                .OnDelete(DeleteBehavior.Restrict);

            model.HasData(PregenData.Routes);
        }

        private void CreateErrors(ModelBuilder builder)
        {
            var model = builder.Entity<Error>();
            model.HasKey(m => new { m.Id });
        }

        private void CreateUnits(ModelBuilder builder)
        {
            var model = builder.Entity<Unit>();
            model.HasKey(m => m.Id);
            model.HasOne(m => m.Domain)
                .WithMany(m => m.Units)
                .HasForeignKey(m => m.DomainId);
            model.HasOne(m => m.Initiator)
                .WithMany(m => m.UnitsWithMyCommands)
                .HasForeignKey(m => m.InitiatorDomainId)
                .OnDelete(DeleteBehavior.Restrict);
            model.HasOne(m => m.Target)
                .WithMany(m => m.ToDomainUnits)
                .HasForeignKey(m => m.TargetDomainId);
            model.HasOne(m => m.Target2)
                .WithMany(m => m.ToDomain2Units)
                .HasForeignKey(m => m.Target2DomainId);
            model.HasOne(m => m.Position)
                .WithMany(m => m.UnitsHere)
                .HasForeignKey(m => m.PositionDomainId);
            model.HasIndex(m => m.InitiatorDomainId);
            model.HasIndex(m => m.DomainId);
            model.HasIndex(m => m.PositionDomainId);
            model.HasIndex(m => m.Type);
            model.HasIndex(m => m.TargetDomainId);
            model.HasIndex(m => m.ActionPoints);

            //model.HasData(PregenData.Units);
        }

        private void CreateDomainRelations(ModelBuilder builder)
        {
            var model = builder.Entity<DomainRelation>();
            model.HasKey(m => m.Id);

            model.HasOne(m => m.SourceDomain)
                .WithMany(m => m.Relations)
                .HasForeignKey(m => m.SourceDomainId)
                .OnDelete(DeleteBehavior.Restrict);
            model.HasOne(m => m.TargetDomain)
                .WithMany(m => m.RelationsToThisDomain)
                .HasForeignKey(m => m.TargetDomainId)
                .OnDelete(DeleteBehavior.Restrict);

            model.HasIndex(m => m.SourceDomainId);
            model.HasIndex(m => m.TargetDomainId);
            model.HasIndex(m => new { m.SourceDomainId, m.TargetDomainId }).IsUnique();
        }

        private void CreateGameSessions(ModelBuilder builder)
        {
            var model = builder.Entity<GameSession>();
            model.HasKey(m => m.Id);

            model.HasIndex(m => m.StartSeesionTurnId);
            model.HasIndex(m => m.EndSeesionTurnId);
        }
    }
}
