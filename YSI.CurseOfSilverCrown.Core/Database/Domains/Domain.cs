using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.Characters;
using YSI.CurseOfSilverCrown.Core.Database.Commands;
using YSI.CurseOfSilverCrown.Core.Database.EventDomains;
using YSI.CurseOfSilverCrown.Core.Database.Relations;
using YSI.CurseOfSilverCrown.Core.Database.Routes;
using YSI.CurseOfSilverCrown.Core.Database.Units;
using YSI.CurseOfSilverCrown.Core.Helpers.StartingDatas;

namespace YSI.CurseOfSilverCrown.Core.Database.Domains
{
    public class Domain
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Казна")]
        public int Gold { get; set; }

        public int Investments { get; set; }

        [Display(Name = "Укрепления")]
        public int Fortifications { get; set; }

        [Display(Name = "Размер владения")]
        public int Size { get; set; }

        public int OwnerId { get; set; }

        public int? SuzerainId { get; set; }

        public int TurnOfDefeat { get; set; }

        public string DomainJson { get; set; }

        [JsonIgnore]
        [Display(Name = "Сюзерен")]
        public virtual Domain Suzerain { get; set; }

        [JsonIgnore]
        [Display(Name = "Вассалы")]
        public virtual List<Domain> Vassals { get; set; }

        [JsonIgnore]
        public virtual Character Owner { get; set; }

        [JsonIgnore]
        [Display(Name = "Действия")]
        public virtual List<Command> Commands { get; set; }

        [JsonIgnore]
        [Display(Name = "Отряды")]
        public virtual List<Unit> Units { get; set; }

        [JsonIgnore]
        public virtual List<Command> ToDomainCommands { get; set; }

        [JsonIgnore]
        public virtual List<Command> ToDomain2Commands { get; set; }

        [JsonIgnore]
        public virtual List<Unit> ToDomainUnits { get; set; }

        [JsonIgnore]
        public virtual List<Unit> ToDomain2Units { get; set; }

        [JsonIgnore]
        public virtual List<Unit> UnitsHere { get; set; }

        [JsonIgnore]
        [Display(Name = "Отношения")]
        public virtual List<Relation> Relations { get; set; }

        [JsonIgnore]
        public virtual List<Relation> ToDomainRelations { get; set; }

        [JsonIgnore]
        public virtual List<EventObject> EventObjects { get; set; }

        [JsonIgnore]
        internal virtual List<Route> RoutesFromHere { get; set; }

        [JsonIgnore]
        internal virtual List<Route> RoutesToHere { get; set; }

        [NotMapped]
        [Display(Name = "Войско")]
        public int WarriorCount
        {
            get
            {
                if (_warriorCount == null)
                {
                    _warriorCount = Units?
                        .Where(u => u.InitiatorCharacterId == OwnerId)
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
            model.HasOne(m => m.Owner)
                .WithMany(m => m.Domains)
                .HasForeignKey(m => m.OwnerId);
            model.HasOne(m => m.Suzerain)
                .WithMany(m => m.Vassals)
                .HasForeignKey(m => m.SuzerainId);

            model.HasIndex(m => m.SuzerainId);
            model.HasIndex(m => m.Size)
                .IsUnique();

            model.HasData(StartingData.Domains);
        }
    }
}
