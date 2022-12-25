using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database.PregenDatas;

namespace YSI.CurseOfSilverCrown.Core.Database.Models
{
    public class Turn
    {
        public int Id { get; set; }
        public DateTime Started { get; set; }
        public bool IsActive { get; set; }

        public virtual List<EventStory> EventStories { get; set; }
        public virtual List<DomainEventStory> OrganizationEventStories { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<Turn>();
            model.HasKey(m => m.Id);

            model.HasData(PregenData.GetFirstTurn());
        }
    }
}
