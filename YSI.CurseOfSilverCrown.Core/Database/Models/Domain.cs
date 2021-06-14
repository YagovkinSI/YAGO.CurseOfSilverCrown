using Newtonsoft.Json;
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

        [JsonIgnore]
        [Display(Name = "Сюзерен")]
        public Domain Suzerain { get; set; }

        [JsonIgnore]
        [Display(Name = "Вассалы")]
        public List<Domain> Vassals { get; set; }
        #endregion


        [JsonIgnore]
        public Person Person { get; set; }

        [JsonIgnore]
        [Display(Name = "Действие")]
        public List<Command> Commands { get; set; }

        [JsonIgnore]
        [Display(Name = "Отряды")]
        public List<Unit> Units { get; set; }

        [JsonIgnore]
        public List<Unit> UnitsWithMyCommands { get; set; }

        [JsonIgnore]
        public List<Command> ToDomainCommands { get; set; }

        [JsonIgnore]
        public List<Command> ToDomain2Commands { get; set; }

        [JsonIgnore]
        public List<Unit> ToDomainUnits { get; set; }

        [JsonIgnore]
        public List<Unit> ToDomain2Units { get; set; }

        [JsonIgnore]
        public List<Unit> UnitsHere { get; set; }

        [JsonIgnore]
        [Display(Name = "Отношения")]
        public List<DomainRelation> Relations { get; set; }

        [JsonIgnore]
        public List<DomainRelation> RelationsToThisDomain { get; set; }

        [JsonIgnore]
        public List<DomainEventStory> DomainEventStories { get; set; }

        [JsonIgnore]
        internal List<Route> RouteFromHere { get; set; }

        [JsonIgnore]
        internal List<Route> RouteToHere { get; set; }

        [NotMapped]
        [Display(Name = "Войско")]
        public int Warriors
        {
            get
            {
                if (warriors == null)
                    warriors = Units?
                        .Where(u => u.InitiatorDomainId == Id)
                        .Sum(u => u.Warriors) ?? 0;
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
