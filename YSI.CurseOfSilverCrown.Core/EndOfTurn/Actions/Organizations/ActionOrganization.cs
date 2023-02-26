using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.MainModels.EventDomains;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions.Organizations
{
    internal class ActionOrganization
    {
        public int Id { get; set; }

        public enEventDomainType EventOrganizationType { get; set; }

        public List<EventParametrChange> EventOrganizationChanges { get; set; }

        public ActionOrganization()
        {
        }

        public ActionOrganization(int domainId, enEventDomainType organizationType)
        {
            Id = domainId;
            EventOrganizationType = organizationType;
        }
    }
}
