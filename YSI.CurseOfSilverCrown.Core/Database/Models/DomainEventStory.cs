using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
