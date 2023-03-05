using System.Collections.Generic;

namespace YSI.CurseOfSilverCrown.Core.Database.Events
{
    internal class EventParticipant
    {
        public int Id { get; set; }

        public EventParticipantType EventOrganizationType { get; set; }

        public List<EventParticipantParameterChange> EventOrganizationChanges { get; set; }

        public EventParticipant(int domainId, EventParticipantType organizationType)
        {
            Id = domainId;
            EventOrganizationType = organizationType;
        }
    }
}
