using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.BL.Models.Main;
using YSI.CurseOfSilverCrown.Core.BL.Models.Min;
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
        public int Warriors { get; set; }
        public double DefenseCoeficient { get; set; }

        public OrganizationInfo(DomainMin organization)
        {
            Id = organization.Id;
            Name = organization.Name;
            Coffers = organization.Coffers;
            Warriors = organization is DomainMain domainMain
                ? domainMain.Warrioirs
                : 0;
            DefenseCoeficient = FortificationsHelper.GetWariorDefenseCoeficient(WarConstants.WariorDefenseSupport, 
                organization.Fortifications);
        }

        public static IEnumerable<OrganizationInfo> GetOrganizationInfoList(IEnumerable<DomainMin> organizations)
        {
            return organizations
                .OrderBy(o => o.Name)
                .Select(o => new OrganizationInfo(o));
        }
    }
}
