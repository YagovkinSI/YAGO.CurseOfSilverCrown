using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using YAGO.World.Infrastructure.Database.Models.StoryDatas;

namespace YAGO.World.Infrastructure.Database.Models.Users
{
    public class User : IdentityUser<long>
    {
        public DateTime Register { get; set; }
        public DateTime LastActivityTime { get; set; }

        public virtual List<StoryData> StoryDatas { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<User>();
            model.HasKey(m => m.Id);
        }
    }
}
