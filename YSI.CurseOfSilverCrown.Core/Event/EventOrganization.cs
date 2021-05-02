using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.Core.Event
{
    public class EventOrganization
    {
        public string Id { get; set; }

        public enEventOrganizationType EventOrganizationType { get; set; }

        public List<EventParametrChange> EventOrganizationChanges { get; set; }
    }
}
