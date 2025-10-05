using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using YAGO.World.Infrastructure.Database.Users;

namespace YAGO.World.Infrastructure.Database.Cities
{
    [Table("Cities")]
    public class CityEntity
    {
        public long Id { get; private set; }
        public long UserId { get; private set; }
        public string Name { get; private set; }
        public string Descripion { get; private set; }

        public virtual UserEntity? User { get; set; }

        protected CityEntity() { }

        public CityEntity(
            long id,
            long userId,
            string name,
            string description)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Descripion = description;
        }

        internal static CityEntity CreateNew(
            long userId,
            string name,
            string description)
        {
            return new CityEntity(
                id: default,
                userId: userId,
                name: name,
                description: description
            );
        }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<CityEntity>();
            model.HasKey(m => m.Id);

            model.HasIndex(m => m.UserId);
            model.HasOne(m => m.User)
                .WithMany(m => m.Cities)
                .HasForeignKey(m => m.UserId);
        }
    }
}
