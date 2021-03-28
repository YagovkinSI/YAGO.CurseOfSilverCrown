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
                    return $"{year} год - Сезон 1 - Зима";
                case 2:
                    return $"{year} год - Сезон 2 - Весна";
                case 3:
                    return $"{year} год - Сезон 3 - Лето";
                case 4:
                default:
                    return $"{year} год - Сезон 4 - Осень";
            }
        }

        public List<Command> Commands { get; set; }
        public List<EventStory> EventStories { get; set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; set; }
    }
}
