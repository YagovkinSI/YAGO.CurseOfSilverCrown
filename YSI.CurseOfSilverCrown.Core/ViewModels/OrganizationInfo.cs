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
        public int Id { get; set; }
        public string Name { get; set; }
        public int Coffers { get; set; }
        public double DefenseCoeficient { get; set; }

        public OrganizationInfo(Domain organization)
        {
            Id = organization.Id;
            Name = organization.Name;
            Coffers = organization.Coffers;
            DefenseCoeficient = FortificationsHelper.GetWariorDefenseCoeficient(WarConstants.WariorDefenseSupport, 
                organization.Fortifications);
        }

        public static IEnumerable<OrganizationInfo> GetOrganizationInfoList(IEnumerable<Domain> organizations)
        {
            return organizations
                .OrderBy(o => o.Name)
                .Select(o => new OrganizationInfo(o));
        }
    }
}
