using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using YSI.CurseOfSilverCrown.Core.Database.Domains;

namespace YSI.CurseOfSilverCrown.Core.Database.Users
{
    public class User : IdentityUser
    {
        public DateTime LastActivityTime { get; set; }

        public virtual string UserJson { get; set; }

        [NotMapped]
        public UserJson UserJsonDeserialized
        {
            get => string.IsNullOrEmpty(UserJson)
                    ? new UserJson()
                    : JsonConvert.DeserializeObject<UserJson>(UserJson);
            set => UserJson = JsonConvert.SerializeObject(value);
        }

        public virtual List<Domain> Domains { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<User>();
            model.HasKey(m => m.Id);
        }
    }
}
