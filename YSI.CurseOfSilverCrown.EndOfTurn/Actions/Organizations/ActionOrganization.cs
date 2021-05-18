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
        [JsonIgnore]
        public Domain Organization { get; set; }

        [JsonIgnore]
        public CoffersActionParameter Coffers { get; set; }
        [JsonIgnore]
        public AllWarriorsActionParameter AllWarrioirs { get; set; }
        [JsonIgnore]
        public WarriorsActionParameter Warrioirs { get; set; }
        [JsonIgnore]
        public InvestmentsActionParameter Investments { get; set; }
        [JsonIgnore]
        public FortificationsActionParameter Fortifications { get; set; }

        public int Id { get; set; }

        public enEventOrganizationType EventOrganizationType { get; set; }

        public List<EventParametrChange> EventOrganizationChanges { get; set; }

        public ActionOrganization()
        {
        }

        public ActionOrganization(Domain organization, int allWarriosCount, enEventOrganizationType organizationType, int warriorsInAction = 0)
        {
            Organization = organization;
            Id = Organization.Id;
            EventOrganizationType = organizationType;

            Coffers = new CoffersActionParameter(organization.Coffers);
            AllWarrioirs = new AllWarriorsActionParameter(allWarriosCount);
            Warrioirs = new WarriorsActionParameter(warriorsInAction);
            Investments = new InvestmentsActionParameter(organization.Investments);
            Fortifications = new FortificationsActionParameter(organization.Fortifications);
        }
    }
}
