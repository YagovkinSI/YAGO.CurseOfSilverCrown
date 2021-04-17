using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Web.Models.DbModels
{
    public class Turn
    {
        public int Id { get; set; }
        public DateTime Started { get; set; }
        public bool IsActive { get; set; }

        public string Name => GetName();

        private string GetName()
        {
            var year = 587 + (Id - 1) / 4;
            switch (Id % 4)
            {
                case 1:
                    return $"{year} год, зима (ход {Id})";
                case 2:
                    return $"{year} год, весна (ход {Id})";
                case 3:
                    return $"{year} год, лето (ход {Id})";
                case 4:
                default:
                    return $"{year} год, осень (ход {Id})";
            }
        }

        public List<EventStory> EventStories { get; set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; set; }
    }
}
