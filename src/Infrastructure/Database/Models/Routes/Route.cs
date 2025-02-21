using Microsoft.EntityFrameworkCore;
using YAGO.World.Infrastructure.Database.Models.Domains;
using YAGO.World.Infrastructure.Helpers.StartingDatas;

namespace YAGO.World.Infrastructure.Database.Models.Routes
{
    public class Route
    {
        public int FromDomainId { get; set; }
        public int ToDomainId { get; set; }
        public string RouteJson { get; set; }

        public virtual Organization FromDomain { get; set; }
        public virtual Organization ToDomain { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<Route>();
            model.HasKey(m => new { m.FromDomainId, m.ToDomainId });

            model.HasOne(m => m.FromDomain)
                .WithMany(m => m.RoutesFromHere)
                .HasForeignKey(m => m.FromDomainId)
                .OnDelete(DeleteBehavior.Restrict);
            model.HasOne(m => m.ToDomain)
                .WithMany(m => m.RoutesToHere)
                .HasForeignKey(m => m.ToDomainId)
                .OnDelete(DeleteBehavior.Restrict);

            model.HasIndex(m => m.FromDomainId);
            model.HasIndex(m => m.ToDomainId);

            model.HasData(StartingData.Routes);
        }
    }
}
