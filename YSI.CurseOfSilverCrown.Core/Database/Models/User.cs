using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.Core.Database.Models
{
    public class User : IdentityUser
    {
        public int? DomainId { get; set; }
        public DateTime LastActivityTime { get; set; }

        public Domain Domain { get; set; }
    }
}
