using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Commands;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.ViewModels
{
    public class OrganizationInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Warriors { get; set; }
        public double DefenseCoeficient { get; set; }

        private OrganizationInfo(Organization organization)
        {
            Id = organization.Id;
            Name = organization.Name;
            Warriors = organization.Warriors;
            DefenseCoeficient = FortificationsHelper.GetWariorDefenseCoeficient(WarConstants.WariorDefenseSupport, 
                organization.Fortifications);
        }

        public static IEnumerable<OrganizationInfo> GetOrganizationInfoList(IEnumerable<Organization> organizations)
        {
            return organizations
                .OrderBy(o => o.Name)
                .Select(o => new OrganizationInfo(o));
        }
    }
}
