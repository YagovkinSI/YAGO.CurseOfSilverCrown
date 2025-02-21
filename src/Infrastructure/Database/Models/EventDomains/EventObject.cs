using Microsoft.EntityFrameworkCore;
using YAGO.World.Infrastructure.Database.Models.Domains;
using YAGO.World.Infrastructure.Database.Models.Events;
using YAGO.World.Infrastructure.Database.Models.Turns;

namespace YAGO.World.Infrastructure.Database.Models.EventDomains
{
    public class EventObject
    {
        public int TurnId { get; set; }
        public int DomainId { get; set; }
        public int EventStoryId { get; set; }

        public int Importance { get; set; }
        public string EventObjectJson { get; set; }

        public virtual Turn Turn { get; set; }
        public virtual Event EventStory { get; set; }
        public virtual Organization Domain { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<EventObject>();
            _ = model.HasKey(m => new { m.TurnId, m.DomainId, m.EventStoryId });

            _ = model.HasOne(m => m.Turn)
                .WithMany(m => m.EventObjects)
                .HasForeignKey(m => m.TurnId)
                .OnDelete(DeleteBehavior.Restrict);
            _ = model.HasOne(m => m.EventStory)
                .WithMany(m => m.EventObjects)
                .HasForeignKey(m => new { m.TurnId, m.EventStoryId });
            _ = model.HasOne(m => m.Domain)
                .WithMany(m => m.EventObjects)
                .HasForeignKey(m => m.DomainId);
        }
    }
}
