using System;
using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.Users;

namespace YSI.CurseOfSilverCrown.Web.ApiModels
{
    public class UserPublicData
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public DateTime LastActivity { get; set; }
        public List<int> DomainIds { get; set; }

        public UserPublicData(User user) 
        { 
            Id = user.Id;
            UserName = user.UserName;
            LastActivity = user.LastActivityTime;
            DomainIds = user.Domains.Select(d => d.Id).ToList();
        }
    }
}
