using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Helpers;

namespace YAGO.World.Host.Database.Commands
{
    public class Command : ICommand
    {
        public int Id { get; set; }
        public ExecutorType ExecutorType { get; set; }
        public int ExecutorId { get; set; }
        public int DomainId { get; set; }

        [Display(Name = "Казна")]
        public int Gold { get; set; }

        [Display(Name = "Воины")]
        public int Warriors { get; set; }

        [Display(Name = "Действие")]
        public CommandType Type { get; set; }

        [Display(Name = "Цель")]
        public int? TargetDomainId { get; set; }

        [Display(Name = "Дополнительная цель")]
        public int? Target2DomainId { get; set; }

        [Display(Name = "Статус")]
        public CommandStatus Status { get; set; }
        public string CommandJson { get; set; }

        public virtual Domain Domain { get; set; }
        public virtual Domain Target { get; set; }
        public virtual Domain Target2 { get; set; }

        [NotMapped]
        public int TypeInt { get => (int)Type; set => Type = (CommandType)value; }

        internal bool IsValid()
        {
            throw new NotImplementedException();
        }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<Command>();
            model.HasKey(m => m.Id);

            model.HasOne(m => m.Domain)
                .WithMany(m => m.Commands)
                .HasForeignKey(m => m.DomainId);
            model.HasOne(m => m.Target)
                .WithMany(m => m.ToDomainCommands)
                .HasForeignKey(m => m.TargetDomainId);
            model.HasOne(m => m.Target2)
                .WithMany(m => m.ToDomain2Commands)
                .HasForeignKey(m => m.Target2DomainId);

            model.HasIndex(m => new { m.ExecutorType, m.ExecutorId });
            model.HasIndex(m => m.DomainId);
            model.HasIndex(m => m.Type);
            model.HasIndex(m => m.TargetDomainId);
        }
    }
}
