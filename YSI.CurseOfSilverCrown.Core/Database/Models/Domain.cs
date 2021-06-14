using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Display(Name = "Имущество владения")]
        public int Investments { get; set; }

        [Display(Name = "Укрепления")]
        public int Fortifications { get; set; }

        [Display(Name = "Порядок хода")]
        public int MoveOrder { get; set; }

        [Display(Name = "Правитель")]
        public int PersonId { get; set; }


        #region Всё что связано с Сюзереном (в будущем вероятно в отдельную таблицу)
        public int? SuzerainId { get; set; }
        public int TurnOfDefeat { get; set; }

        [Display(Name = "Сюзерен")]
        public Domain Suzerain { get; set; }

        [Display(Name = "Вассалы")]
        public List<Domain> Vassals { get; set; }
        #endregion


        [Obsolete]
        public User User { get; set; }
        public Person Person { get; set; }

        [Display(Name = "Действие")]
        public List<Command> Commands { get; set; }

        [Display(Name = "Отряды")]
        public List<Unit> Units { get; set; }
        public List<Unit> UnitsWithMyCommands { get; set; }

        public List<Command> ToDomainCommands { get; set; }
        public List<Command> ToDomain2Commands { get; set; }

        public List<Unit> ToDomainUnits { get; set; }
        public List<Unit> ToDomain2Units { get; set; }
        public List<Unit> UnitsHere { get; set; }

        [Display(Name = "Отношения")]
        public List<DomainRelation> Relations { get; set; }
        public List<DomainRelation> RelationsToThisDomain { get; set; }

        public List<DomainEventStory> DomainEventStories { get; set; }
        internal List<Route> RouteFromHere { get; set; }
        internal List<Route> RouteToHere { get; set; }

        [NotMapped]
        [Display(Name = "Войско")]
        public int Warriors
        {
            get
            {
                if (warriors == null)
                    warriors = Units
                        .Where(u => u.InitiatorDomainId == Id)
                        .Sum(u => u.Warriors);
                return warriors.Value;
            }
        }
        private int? warriors = null;

        public override string ToString()
        {
            return Name;
        }
    }
}
