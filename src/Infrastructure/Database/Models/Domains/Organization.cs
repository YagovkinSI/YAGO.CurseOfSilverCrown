using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using YAGO.World.Infrastructure.Database.Models.Commands;
using YAGO.World.Infrastructure.Database.Models.EventDomains;
using YAGO.World.Infrastructure.Database.Models.Relations;
using YAGO.World.Infrastructure.Database.Models.Routes;
using YAGO.World.Infrastructure.Database.Models.Units;
using YAGO.World.Infrastructure.Database.Models.Users;
using YAGO.World.Infrastructure.Helpers.StartingDatas;

namespace YAGO.World.Infrastructure.Database.Models.Domains
{
    public class Organization
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

        public string UserId { get; set; }

        public int? SuzerainId { get; set; }

        public int TurnOfDefeat { get; set; }

        public string DomainJson { get; set; }

        [JsonIgnore]
        [Display(Name = "Сюзерен")]
        public virtual Organization Suzerain { get; set; }

        [JsonIgnore]
        [Display(Name = "Вассалы")]
        public virtual List<Organization> Vassals { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

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
            var model = builder.Entity<Organization>();
            model.HasKey(m => m.Id);
            model.HasOne(m => m.User)
                .WithMany(m => m.Domains)
                .HasForeignKey(m => m.UserId);
            model.HasOne(m => m.Suzerain)
                .WithMany(m => m.Vassals)
                .HasForeignKey(m => m.SuzerainId);

            model.HasIndex(m => m.UserId);
            model.HasIndex(m => m.SuzerainId);
            model.HasIndex(m => m.Size)
                .IsUnique();

            model.HasData(StartingData.Domains);
        }
    }
}
