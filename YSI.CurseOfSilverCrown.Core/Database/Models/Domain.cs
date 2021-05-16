using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.Core.Database.Models
{
    public class Domain
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Казна")]
        public int Coffers { get; set; }

        [Display(Name = "Воины")]
        public int Warriors { get; set; }

        [Display(Name = "Имущество провинции")]
        public int Investments { get; set; }

        [Display(Name = "Укрепления")]
        public int Fortifications { get; set; }


        #region Всё что связано с Сюзереном (в будущем вероятно в отдельную таблицу)
        public int? SuzerainId { get; set; }
        public int TurnOfDefeat { get; set; }

        [Display(Name = "Сюзерен")]
        public Domain Suzerain { get; set; }

        [Display(Name = "Вассалы")]
        public List<Domain> Vassals { get; set; }
        #endregion


        public User User { get; set; }

        [Display(Name = "Действие")]
        public List<Command> Commands { get; set; }

        [Display(Name = "Отряды")]
        public List<Unit> Units { get; set; }

        public List<Command> ToDomainCommands { get; set; }
        public List<Command> ToDomain2Commands { get; set; }

        public List<Unit> ToDomainUnits { get; set; }
        public List<Unit> ToDomain2Units { get; set; }
        public List<Unit> UnitsHere { get; set; }

        public List<DomainEventStory> DomainEventStories { get; set; }
        public List<Route> RouteFromHere { get; set; }
        public List<Route> RouteToHere { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
