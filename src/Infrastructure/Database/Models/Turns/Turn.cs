using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using YAGO.World.Infrastructure.Database.Models.EventDomains;
using YAGO.World.Infrastructure.Database.Models.Events;
using YAGO.World.Infrastructure.Helpers.StartingDatas;

namespace YAGO.World.Infrastructure.Database.Models.Turns
{
    public class Turn
    {
        public int Id { get; set; }
        public DateTime Started { get; set; }
        public bool IsActive { get; set; }
        public string TurnJson { get; set; }

        public virtual List<Event> EventStories { get; set; }
        public virtual List<EventObject> EventObjects { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<Turn>();
            model.HasKey(m => m.Id);

            model.HasData(StartingData.GetFirstTurn());
        }
    }
}
