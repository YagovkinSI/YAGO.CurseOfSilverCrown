using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions.Organizations
{
    internal class ActionOrganization
    {
        public int Id { get; set; }

        public enEventOrganizationType EventOrganizationType { get; set; }

        public List<EventParametrChange> EventOrganizationChanges { get; set; }

        public ActionOrganization()
        {
        }

        public ActionOrganization(int domainId, enEventOrganizationType organizationType)
        {
            Id = domainId;
            EventOrganizationType = organizationType;
        }
    }
}
