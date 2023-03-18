using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using YSI.CurseOfSilverCrown.Core.Database.Characters;

namespace YSI.CurseOfSilverCrown.Core.Database.Users
{
    public class User : IdentityUser
    {
        public int? CharacterId { get; set; }
        public DateTime LastActivityTime { get; set; }

        public virtual string UserJson { get; set; }

        public virtual Character Character { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<User>();
            model.HasKey(m => m.Id);

            model.HasOne(m => m.Character)
                .WithOne(m => m.User)
                .HasForeignKey<User>(m => m.CharacterId);
            model.HasIndex(m => m.CharacterId);
        }
    }
}
