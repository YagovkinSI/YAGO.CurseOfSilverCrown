using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.Core.Event
{
    public class EventStoryResult
    {
        public List<EventOrganization> Organizations { get; set; }
        public enEventResultType EventResultType { get; set; }
    }
}
