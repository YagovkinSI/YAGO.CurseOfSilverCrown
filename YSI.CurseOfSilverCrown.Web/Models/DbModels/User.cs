using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Enums;

namespace YSI.CurseOfSilverCrown.Web.Models.DbModels
{
    public class User : IdentityUser
    {
        public string OrganizationId { get; set; }

        public Organization Organization { get; set; }
    }
}
