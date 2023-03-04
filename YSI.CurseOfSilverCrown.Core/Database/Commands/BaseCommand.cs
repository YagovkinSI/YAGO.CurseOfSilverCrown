using System.ComponentModel.DataAnnotations;
using YSI.CurseOfSilverCrown.Core.Database.Domains;
using YSI.CurseOfSilverCrown.Core.Database.Units;
using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.Core.Database.Commands
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
        public int Coffers { get; set; }

        [Display(Name = "Воины")]
        public int Warriors { get; set; }

        public bool IsArmy { get; }
        public int TypeInt { get; set; }

        public int? TargetDomainId { get; }

        public int? Target2DomainId { get; }

        public int InitiatorPersonId { get; }

        public enCommandStatus Status { get; set; }

        public Domain Domain { get; }

        public Domain Target { get; }

        public Domain Target2 { get; }

        public BaseCommand(Command command)
        {
            if (command == null)
                return;

            IsArmy = false;

            Id = command.Id;
            DomainId = command.DomainId;
            Coffers = command.Coffers;
            Warriors = command.Warriors;
            TypeInt = (int)command.Type;
            TargetDomainId = command.TargetDomainId;
            Target2DomainId = command.Target2DomainId;
            InitiatorPersonId = command.InitiatorPersonId;
            Status = command.Status;
        }

        public BaseCommand(Unit army)
        {
            if (army == null)
                return;

            IsArmy = true;

            Id = army.Id;
            DomainId = army.DomainId;
            Coffers = army.Coffers;
            Warriors = army.Warriors;
            TypeInt = (int)army.Type;
            TargetDomainId = army.TargetDomainId;
            Target2DomainId = army.Target2DomainId;
            InitiatorPersonId = army.InitiatorPersonId;
            Status = army.Status;
        }
    }
}
