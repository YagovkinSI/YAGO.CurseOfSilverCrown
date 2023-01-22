using Newtonsoft.Json;
using System.Collections.Generic;
using YSI.CurseOfSilverCrown.EndOfTurn.Actions.Organizations;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Event
{
    internal class EventStoryResult
    {
        public List<ActionOrganization> Organizations { get; set; }
        public enEventResultType EventResultType { get; set; }

        public EventStoryResult(enEventResultType eventResultType)
        {
            EventResultType = eventResultType;
            Organizations = new List<ActionOrganization>();
        }

        public void AddEventOrganization(int domainId, enEventOrganizationType organizationType, 
            List<EventParametrChange> eventParametrChanges)
        {
            var eventOrganization = new ActionOrganization(domainId, organizationType);
            eventOrganization.EventOrganizationChanges = eventParametrChanges;
            Organizations.Add(eventOrganization);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
