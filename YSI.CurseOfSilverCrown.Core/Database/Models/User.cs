using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core.Database.Models
{
    public class User : IdentityUser
    {
        public int? PersonId { get; set; }
        public DateTime LastActivityTime { get; set; }

        public Person Person { get; set; }
    }
}
