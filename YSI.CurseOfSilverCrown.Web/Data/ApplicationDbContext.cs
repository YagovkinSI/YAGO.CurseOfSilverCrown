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
            model.HasMany(m => m.Organizations)
                .WithOne(m => m.Province)
                .HasForeignKey(m => m.ProvinceId);

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
            model.HasMany(m => m.Vassals)
                .WithOne(m => m.Suzerain)
                .HasForeignKey(m => m.SuzerainId);
            model.HasMany(m => m.Commands)
                .WithOne(m => m.Organization)
                .HasForeignKey(m => m.OrganizationId);
            model.HasMany(m => m.ToOrganizationCommands)
                .WithOne(m => m.Target)
                .HasForeignKey(m => m.TargetOrganizationId);

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
            model.HasOne(m => m.Turn)
                .WithMany(m => m.Commands)
                .HasForeignKey(m => m.TurnId);
            model.HasIndex(m => m.TurnId);
            model.HasIndex(m => m.OrganizationId);
            model.HasIndex(m => m.Type);
            model.HasIndex(m => m.TargetOrganizationId);

            model.HasData(baseData.GetCommands());
        }

        private void CreateTurns(ModelBuilder builder)
        {
            var model = builder.Entity<Turn>();
            model.HasKey(m => m.Id);
            model.HasMany(m => m.Commands)
                .WithOne(m => m.Turn)
                .HasForeignKey(m => m.TurnId);

            model.HasData(baseData.GetFirstTurn());
        }
    }
}
