using Microsoft.EntityFrameworkCore;
using System;
using YAGO.World.Infrastructure.Database.Models.Users;

namespace YAGO.World.Infrastructure.Database.Models.StoryDatas
{
    public class StoryData
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdate { get; set; }
        public long UserId { get; set; }
        public long CurrentStoryNodeId { get; set; }
        public string StoryDataJson { get; set; }

        public virtual User User { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<StoryData>();
            model.HasKey(m => m.Id);
            model.HasOne(m => m.User)
                .WithMany(u => u.StoryDatas)
                .HasForeignKey(m => m.UserId);
            model.HasIndex(m => m.UserId);
        }
    }
}
