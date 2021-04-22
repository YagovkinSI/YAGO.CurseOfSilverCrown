using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.Models.ViewModels
{
    public class OrganizationInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Warriors { get; set; }

        public OrganizationInfo(Organization organization)
        {
            Id = organization.Id;
            Name = organization.Name;
            Warriors = organization.Warriors;
        }
    }
}
