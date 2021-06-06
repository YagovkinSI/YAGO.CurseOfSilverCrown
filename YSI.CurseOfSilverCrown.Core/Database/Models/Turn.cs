using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core.Database.Models
{
    public class Turn
    {
        public int Id { get; set; }
        public int EndOfLastGameTurnId { get; set; }
        public int NumberOfGame { get; set; }
        public DateTime Started { get; set; }
        public bool IsActive { get; set; }

        public string Name => GetName();

        private string GetName()
        {
            var number = Id - EndOfLastGameTurnId;
            var year = 587 + NumberOfGame * 10 + (Id - 1) / 4;
            switch (Id % 4)
            {
                case 1:
                    return $"{year} год, зима (ход {number})";
                case 2:
                    return $"{year} год, весна (ход {number})";
                case 3:
                    return $"{year} год, лето (ход {number})";
                case 4:
                default:
                    return $"{year} год, осень (ход {number})";
            }
        }

        public List<EventStory> EventStories { get; set; }
        public List<DomainEventStory> OrganizationEventStories { get; set; }
    }
}
