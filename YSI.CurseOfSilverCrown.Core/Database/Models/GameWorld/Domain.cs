using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.PregenDatas;

namespace YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld
{
    public class Domain
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Казна")]
        public int Coffers { get; set; }

        public int Investments { get; set; }

        [Display(Name = "Укрепления")]
        public int Fortifications { get; set; }

        [Display(Name = "Размер владения")]
        public int MoveOrder { get; set; }

        public int PersonId { get; set; }

        public int? SuzerainId { get; set; }

        public int TurnOfDefeat { get; set; }

        [JsonIgnore]
        [Display(Name = "Сюзерен")]
        public Domain Suzerain { get; set; }

        [JsonIgnore]
        [Display(Name = "Вассалы")]
        public List<Domain> Vassals { get; set; }

        [JsonIgnore]
        public Person Person { get; set; }

        [JsonIgnore]
        [Display(Name = "Действия")]
        public List<Command> Commands { get; set; }

        [JsonIgnore]
        [Display(Name = "Отряды")]
        public List<Unit> Units { get; set; }

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
        public int WarriorCount
        {
            get
            {
                if (_warriorCount == null)
                {
                    _warriorCount = Units?
                        .Where(u => u.InitiatorPersonId == PersonId)
                        .Sum(u => u.Warriors) ?? 0;
                }

                return _warriorCount.Value;
            }
        }
        private int? _warriorCount = null;

        [NotMapped]
        [Display(Name = "Имущество владения")]
        public int InvestmentsShowed => Investments;

        public override string ToString()
        {
            return Name;
        }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<Domain>();
            model.HasKey(m => m.Id);
            model.HasOne(m => m.Person)
                .WithMany(m => m.Domains)
                .HasForeignKey(m => m.PersonId);
            model.HasOne(m => m.Suzerain)
                .WithMany(m => m.Vassals)
                .HasForeignKey(m => m.SuzerainId);

            model.HasIndex(m => m.SuzerainId);
            model.HasIndex(m => m.MoveOrder)
                .IsUnique();

            model.HasData(PregenData.Organizations);
        }
    }
}
