using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using Newtonsoft.Json;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.EndOfTurn.Actions.Organizations.Parameters;

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

        public ActionOrganization(int domainId, int allWarriosCount, enEventOrganizationType organizationType, int warriorsInAction = 0)
        {
            Id = domainId;
            EventOrganizationType = organizationType;
        }
    }
}
