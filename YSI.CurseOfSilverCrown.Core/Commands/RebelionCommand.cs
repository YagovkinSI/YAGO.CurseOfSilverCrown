using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.Core.Commands
{
    public class RebelionCommand : BaseCommand
    {
        public RebelionCommand(Unit command, Domain domain)
            : base(command)
        {
            TypeInt = (int)enArmyCommandType.Rebellion;
            TargetId = domain.SuzerainId;
        }

        public override string Name => "Востание против сюзерена";

        public override string[] Descriptions => 
            new[] 
        {
            $"Восстание против сюзерена - команда атаковать владение сюзерена с целью освобождения себя от вассальной клятвы.",
            $"Но будьте осторожны! Неудачное восстание может привести к серии казней и заставить сюзерена усилить контроль за пленными."
        };

        public override bool IsSingleCommand => true;

        public override bool NeedTarget => false;
        public override int? TargetId { get; }

        public override string TargetName => string.Empty;


        public override bool NeedTarget2 => false;

        public override string Target2Name => "Помочь владению";


        public override bool NeedCoffers => false;

        public override bool NeedWarriors => true;
    }
}
