using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace YSI.CurseOfSilverCrown.Core.Database.Models
{
    public class EventStory
    {
        public int TurnId { get; set; }
        public int Id { get; set; }

        public string EventStoryJson { get; set; }

        public Turn Turn { get; set; }
        public List<DomainEventStory> DomainEventStories { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<EventStory>();
            model.HasKey(m => new { m.TurnId, m.Id });

            model.HasOne(m => m.Turn)
                .WithMany(m => m.EventStories)
                .HasForeignKey(m => m.TurnId);
        }
    }
}
