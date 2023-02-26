using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.MainModels.EventDomains;

namespace YSI.CurseOfSilverCrown.Core.MainModels.Events
{
    internal class EventJsonDomain
    {
        public int Id { get; set; }

        public enEventDomainType EventOrganizationType { get; set; }

        public List<EventJsonParametrChange> EventOrganizationChanges { get; set; }

        public EventJsonDomain(int domainId, enEventDomainType organizationType)
        {
            Id = domainId;
            EventOrganizationType = organizationType;
        }
    }
}
