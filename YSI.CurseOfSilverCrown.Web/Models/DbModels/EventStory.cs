using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Web.Models.DbModels
{
    public class EventStory
    {
        public int TurnId { get; set; }
        public int Id { get; set; }

        public string EventStoryJson { get; set; }

        public Turn Turn { get; set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; set; }
    }
}
