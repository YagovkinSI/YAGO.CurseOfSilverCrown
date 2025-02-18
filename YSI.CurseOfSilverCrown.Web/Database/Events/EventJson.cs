using Newtonsoft.Json;
using System.Collections.Generic;

namespace YSI.CurseOfSilverCrown.Web.Database.Events
{
    internal class EventJson
    {
        public List<EventParticipant> Organizations { get; set; }

        public EventJson()
        {
            Organizations = new List<EventParticipant>();
        }

        public void AddEventOrganization(int domainId, EventParticipantType organizationType,
            List<EventParticipantParameterChange> eventParametrChanges)
        {
            var eventOrganization = new EventParticipant(domainId, organizationType)
            {
                EventOrganizationChanges = eventParametrChanges
            };
            Organizations.Add(eventOrganization);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
