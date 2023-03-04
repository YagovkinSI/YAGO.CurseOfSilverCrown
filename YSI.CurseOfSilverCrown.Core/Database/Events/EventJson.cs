using Newtonsoft.Json;
using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database.EventDomains;

namespace YSI.CurseOfSilverCrown.Core.Database.Events
{
    internal class EventJson
    {
        public enEventType EventResultType { get; set; }
        public List<EventJsonDomain> Organizations { get; set; }

        public EventJson(enEventType eventResultType)
        {
            EventResultType = eventResultType;
            Organizations = new List<EventJsonDomain>();
        }

        public void AddEventOrganization(int domainId, enEventDomainType organizationType,
            List<EventJsonParametrChange> eventParametrChanges)
        {
            var eventOrganization = new EventJsonDomain(domainId, organizationType)
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
