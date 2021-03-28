using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Enums;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Event
{

    public class EventStoryResult
    {
        public List<EventOrganization> Organizations { get; set; }
        public enEventResultType EventResultType { get; set; }
    }

    public class EventOrganization
    {
        public string Id { get; set; }

        public enEventOrganizationType EventOrganizationType { get; set; }

        public List<EventParametrChange> EventOrganizationChanges { get; set; }
    }

    public class EventParametrChange
    {
        public enEventParametrChange Type { get; set; }
        public int Before { get; set; }
        public int After { get; set; }
    }
}
