using Microsoft.EntityFrameworkCore;
using YSI.CurseOfSilverCrown.Core.MainModels.Domains;
using YSI.CurseOfSilverCrown.Core.MainModels.Events;
using YSI.CurseOfSilverCrown.Core.MainModels.Turns;

namespace YSI.CurseOfSilverCrown.Core.MainModels.EventDomains
{
    public class EventDomain
    {
        public int TurnId { get; set; }
        public int DomainId { get; set; }
        public int EventStoryId { get; set; }

        public int Importance { get; set; }

        public virtual Turn Turn { get; set; }
        public virtual Event EventStory { get; set; }
        public virtual Domain Domain { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<EventDomain>();
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
