using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using YAGO.World.Infrastructure.Database.Models.Cities;

namespace YAGO.World.Infrastructure.Database.Models.Users
{
    public class UserEntity : IdentityUser<long>
    {
        public DateTime Register { get; set; }
        public DateTime LastActivityTime { get; set; }

        public virtual List<CityEntity> Cities { get; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<UserEntity>();
            model.HasKey(m => m.Id);
        }
    }
}
