using Microsoft.EntityFrameworkCore;
using YAGO.World.Infrastructure.Database.Models.Users;

namespace YAGO.World.Infrastructure.Database.Models.Cities
{
    public class CityEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long UserId { get; set; }
        public int Gold { get; set; }
        public int Population { get; set; }
        public int Military { get; set; }
        public int Fortifications { get; set; }
        public int Control { get; set; }

        public virtual UserEntity User { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<CityEntity>();
            model.HasKey(m => m.Id);

            model.HasOne(m => m.User)
                .WithMany(m => m.Cities)
                .HasForeignKey(m => m.UserId);

            model.HasIndex(m => m.UserId);
        }
    }
}
