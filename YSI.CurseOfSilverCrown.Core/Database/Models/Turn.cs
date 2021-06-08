using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core.Database.Models
{
    public class Turn
    {
        public int Id { get; set; }
        public DateTime Started { get; set; }
        public bool IsActive { get; set; }

        public List<EventStory> EventStories { get; set; }
        public List<DomainEventStory> OrganizationEventStories { get; set; }
    }
}
