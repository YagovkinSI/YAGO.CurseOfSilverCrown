using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;

namespace YSI.CurseOfSilverCrown.Core.ViewModels
{
    public class OrganizationInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Warriors { get; set; }

        private OrganizationInfo(Organization organization)
        {
            Id = organization.Id;
            Name = organization.Name;
            Warriors = organization.Warriors;
        }

        public static IEnumerable<OrganizationInfo> GetOrganizationInfoList(IEnumerable<Organization> organizations)
        {
            return organizations
                .OrderBy(o => o.Name)
                .Select(o => new OrganizationInfo(o));
        }
    }
}
