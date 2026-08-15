using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using YAGO.World.Domain.Common;
using YAGO.World.Infrastructure.Database.Users;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    public class ColonyEntity : IEntity<long>
    {
        public long Id { get; private set; }
        public long UserId { get; private set; }
        public string JsonData { get; private set; } = "{}";
        [Timestamp]
        public uint Version { get; private set; }

        public virtual UserEntity? User { get; set; }

        protected ColonyEntity() { }

        public ColonyEntity(
            long id,
            long userId,
            string statesJson)
        {
            Id = id;
            UserId = userId;
            JsonData = statesJson;
        }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<ColonyEntity>();
            model.HasKey(m => m.Id);

            model.HasOne(x => x.User)
                .WithMany(x => x.Colonies)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            model.HasIndex(m => m.UserId);
        }

        internal void SetStatesJson(ColonyParameters colonyParameters)
        {
            JsonData = JsonConvert.SerializeObject(colonyParameters);
        }
    }
}
