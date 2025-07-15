using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace YAGO.World.Infrastructure.Database.Models.Users
{
    public class User : IdentityUser<long>
    {
        public DateTime Register { get; set; }
        public DateTime LastActivityTime { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<User>();
            model.HasKey(m => m.Id);
        }
    }
}
