using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Enums;

namespace YSI.CurseOfSilverCrown.Web.Models.DbModels
{
    public class Organization
    {
        public string Id { get; set; }
        public enOrganizationType OrganizationType { get; set; }
        public int ProvinceId { get; set; }
        public string SuzerainId { get; set; }

        public Province Province { get; set; }
        public Organization Suzerain { get; set; }

        public User User { get; set; }
        public List<Command> Commands { get; set; }
        public List<Command> ToOrganizationCommands { get; set; }
        public List<Organization> Vassals { get; set; }
    }
}
