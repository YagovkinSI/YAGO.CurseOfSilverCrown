using Newtonsoft.Json;
using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.MainModels.EventDomains;
using YSI.CurseOfSilverCrown.Core.MainModels.Events;
using YSI.CurseOfSilverCrown.EndOfTurn.Actions.Organizations;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Event
{
    internal class EventStoryResult
    {
        public List<ActionOrganization> Organizations { get; set; }
        public enEventType EventResultType { get; set; }

        public EventStoryResult(enEventType eventResultType)
        {
            EventResultType = eventResultType;
            Organizations = new List<ActionOrganization>();
        }

        public void AddEventOrganization(int domainId, enEventDomainType organizationType,
            List<EventParametrChange> eventParametrChanges)
        {
            var eventOrganization = new ActionOrganization(domainId, organizationType)
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
