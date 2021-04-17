using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<Turn> Turns { get; set; }
        public DbSet<EventStory> EventStories { get; set; }
        public DbSet<OrganizationEventStory> OrganizationEventStories { get; set; }


        private readonly BaseData baseData = new BaseData();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            CreateUsers(builder);
            CreateProvinces(builder);
            CreateOrganizations(builder);
            CreateCommands(builder);
            CreateTurns(builder);
            CreateEventStories(builder);
            CreateOrganizationEventStories(builder);
        }

        private void CreateUsers(ModelBuilder builder)
        {
            var model = builder.Entity<User>();
            model.HasKey(m => m.Id);
            model.HasOne(m => m.Organization)
                .WithOne(m => m.User)
                .HasForeignKey<User>(m => m.OrganizationId);
            model.HasIndex(m => m.OrganizationId);
        }

        private void CreateProvinces(ModelBuilder builder)
        {
            var model = builder.Entity<Province>();
            model.HasKey(m => m.Id);

            model.HasData(baseData.GetProvinces()); 
        }

        private void CreateOrganizations(ModelBuilder builder)
        {
            var model = builder.Entity<Organization>();
            model.HasKey(m => m.Id);
            model.HasOne(m => m.Province)
                .WithMany(m => m.Organizations)
                .HasForeignKey(m => m.ProvinceId);
            model.HasOne(m => m.User)
                .WithOne(m => m.Organization)
                .HasForeignKey<User>(m => m.OrganizationId);
            model.HasOne(m => m.Suzerain)
                .WithMany(m => m.Vassals)
                .HasForeignKey(m => m.SuzerainId);

            model.HasIndex(m => m.OrganizationType);
            model.HasIndex(m => m.ProvinceId);
            model.HasIndex(m => m.SuzerainId);

            model.HasData(baseData.GetOrganizations());
        }

        private void CreateCommands(ModelBuilder builder)
        {
            var model = builder.Entity<Command>();
            model.HasKey(m => m.Id);
            model.HasOne(m => m.Organization)
                .WithMany(m => m.Commands)
                .HasForeignKey(m => m.OrganizationId);
            model.HasOne(m => m.Target)
                .WithMany(m => m.ToOrganizationCommands)
                .HasForeignKey(m => m.TargetOrganizationId);
            model.HasIndex(m => m.OrganizationId);
            model.HasIndex(m => m.Type);
            model.HasIndex(m => m.TargetOrganizationId);

            //model.HasData(baseData.GetCommands());
        }

        private void CreateTurns(ModelBuilder builder)
        {
            var model = builder.Entity<Turn>();
            model.HasKey(m => m.Id);

            model.HasData(baseData.GetFirstTurn());
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
            var model = builder.Entity<OrganizationEventStory>();
            model.HasKey(m => new { m.TurnId, m.OrganizationId, m.EventStoryId });
            model.HasOne(m => m.Turn)
                .WithMany(m => m.OrganizationEventStories)
                .HasForeignKey(m => m.TurnId)
                .OnDelete(DeleteBehavior.Restrict);
            model.HasOne(m => m.EventStory)
                .WithMany(m => m.OrganizationEventStories)
                .HasForeignKey(m => new { m.TurnId, m.EventStoryId });
            model.HasOne(m => m.Organization)
                .WithMany(m => m.OrganizationEventStories)
                .HasForeignKey(m => m.OrganizationId);
        }
    }
}
