using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Web.Database.EventDomains;
using YSI.CurseOfSilverCrown.Web.Database.Turns;

namespace YSI.CurseOfSilverCrown.Web.Database.Events
{
    public class Event
    {
        public int TurnId { get; set; }
        public int Id { get; set; }
        public EventType Type { get; set; }

        public string EventJson { get; set; }

        public virtual Turn Turn { get; set; }
        public virtual List<EventObject> EventObjects { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<Event>();
            model.HasKey(m => new { m.TurnId, m.Id });

            model.HasOne(m => m.Turn)
                .WithMany(m => m.EventStories)
                .HasForeignKey(m => m.TurnId);
        }
    }
}
