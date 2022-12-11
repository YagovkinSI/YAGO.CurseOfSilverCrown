using Microsoft.EntityFrameworkCore;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;

namespace YSI.CurseOfSilverCrown.Core.Database.Models
{
    public class DomainEventStory
    {
        public int TurnId { get; set; }
        public int DomainId { get; set; }
        public int EventStoryId { get; set; }

        public int Importance { get; set; }

        public Turn Turn { get; set; }
        public EventStory EventStory { get; set; }
        public Domain Domain { get; set; }

        internal static void CreateModel(ModelBuilder builder)
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
    }
}
