using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YAGO.World.Infrastructure.Helpers;
using YAGO.World.Infrastructure.Database.Models.Commands;
using YAGO.World.Infrastructure.Database.Models.Domains;
using YAGO.World.Infrastructure.Helpers.StartingDatas;
using YAGO.World.Domain.Units.Enums;

namespace YAGO.World.Infrastructure.Database.Models.Units
{
    public class Unit : ICommand
    {
        public int Id { get; set; }
        public ExecutorType ExecutorType => ExecutorType.Unit;
        public int ExecutorId => Id;
        public int DomainId { get; set; }

        [Display(Name = "Казна")]
        public int Gold { get; set; }

        [Display(Name = "Воины")]
        public int Warriors { get; set; }


        [Display(Name = "Действие")]
        public UnitCommandType Type { get; set; }

        [Display(Name = "Цель")]
        public int? TargetDomainId { get; set; }

        [Display(Name = "Дополнительная цель")]
        public int? Target2DomainId { get; set; }

        [Display(Name = "Местоположение")]
        public int? PositionDomainId { get; set; }

        [Display(Name = "Статус")]
        public CommandStatus Status { get; set; }
        public string UnitJson { get; set; }

        public virtual Organization Domain { get; set; }

        public virtual Organization Target { get; set; }

        public virtual Organization Target2 { get; set; }

        public virtual Organization Position { get; set; }

        [NotMapped]
        public int TypeInt { get => (int)Type; set => Type = (UnitCommandType)value; }

        public override string ToString()
        {
            return $"Отряд владения {Domain?.Name ?? "???"} во владении {Position?.Name ?? "???"}, воинов - {Warriors}";
        }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<Unit>();
            model.HasKey(m => m.Id);

            CreateModelRelations(model);

            model.HasIndex(m => m.DomainId);
            model.HasIndex(m => m.PositionDomainId);
            model.HasIndex(m => m.Type);
            model.HasIndex(m => m.TargetDomainId);

            model.HasData(StartingData.Units);
        }

        private static void CreateModelRelations(EntityTypeBuilder<Unit> model)
        {
            model.HasOne(m => m.Domain)
                .WithMany(m => m.Units)
                .HasForeignKey(m => m.DomainId);
            model.HasOne(m => m.Target)
                .WithMany(m => m.ToDomainUnits)
                .HasForeignKey(m => m.TargetDomainId);
            model.HasOne(m => m.Target2)
                .WithMany(m => m.ToDomain2Units)
                .HasForeignKey(m => m.Target2DomainId);
            model.HasOne(m => m.Position)
                .WithMany(m => m.UnitsHere)
                .HasForeignKey(m => m.PositionDomainId);
        }
    }
}
