using System.ComponentModel.DataAnnotations;
using YAGO.World.Infrastructure.Database.Models.Commands;
using YAGO.World.Infrastructure.Database.Models.Domains;
using YAGO.World.Infrastructure.Database.Models.Units;

namespace YAGO.World.Infrastructure.Helpers.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public abstract string Name { get; }
        public abstract string[] Descriptions { get; }

        public abstract bool IsSingleCommand { get; }

        public abstract bool NeedTarget { get; }
        public virtual int? TargetId { get; }
        public abstract string TargetName { get; }

        public abstract bool NeedTarget2 { get; }
        public abstract string Target2Name { get; }

        public abstract bool NeedCoffers { get; }
        public virtual int MaxCoffers => int.MaxValue;
        public virtual int StepCoffers => 1;

        public abstract bool NeedWarriors { get; }


        public int Id { get; }
        public int DomainId { get; }

        [Display(Name = "Казна")]
        public int Gold { get; set; }

        [Display(Name = "Воины")]
        public int Warriors { get; set; }


        public ExecutorType ExecutorType { get; }
        public int ExecutorId { get; }

        public int TypeInt { get; set; }

        public int? TargetDomainId { get; }

        public int? Target2DomainId { get; }

        public CommandStatus Status { get; set; }

        public Organization Domain { get; }

        public Organization Target { get; }

        public Organization Target2 { get; }

        public BaseCommand(Command command)
        {
            if (command == null)
                return;

            ExecutorType = ExecutorType.Domain;
            ExecutorId = command.ExecutorId;

            Id = command.Id;
            DomainId = command.DomainId;
            Gold = command.Gold;
            Warriors = command.Warriors;
            TypeInt = (int)command.Type;
            TargetDomainId = command.TargetDomainId;
            Target2DomainId = command.Target2DomainId;
            Status = command.Status;
        }

        public BaseCommand(Unit army)
        {
            if (army == null)
                return;

            ExecutorType = ExecutorType.Unit;
            ExecutorId = army.ExecutorId;

            Id = army.Id;
            DomainId = army.DomainId;
            Gold = army.Gold;
            Warriors = army.Warriors;
            TypeInt = (int)army.Type;
            TargetDomainId = army.TargetDomainId;
            Target2DomainId = army.Target2DomainId;
            Status = army.Status;
        }
    }
}
