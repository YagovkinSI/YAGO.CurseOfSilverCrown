using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Web.Database.Domains;

namespace YSI.CurseOfSilverCrown.Web.Database.Users
{
    public class User : IdentityUser
    {
        public DateTime LastActivityTime { get; set; }

        public virtual string UserJson { get; set; }

        public virtual List<Domain> Domains { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<User>();
            model.HasKey(m => m.Id);
        }
    }
}
