using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.Core.Commands
{
    public class WarCommand : BaseCommand
    {
        public WarCommand(Unit command)
            : base(command)
        {
            TypeInt = (int)enArmyCommandType.War;
        }

        public override string Name => "Нападение";

        public override string[] Descriptions => new[] 
        {
            "Нападение - команда атаковать чужое владение с целью её захвата.",
            "Вы не можете нападать на владение вашего королевства (своих вассалов, сюзерена, васслов сюзерена и т.д.)."
        };

        public override bool IsSingleCommand => false;

        public override bool NeedTarget => true;

        public override string TargetName => "Нападение на владене";


        public override bool NeedTarget2 => false;

        public override string Target2Name => string.Empty;


        public override bool NeedCoffers => false;

        public override bool NeedWarriors => true;
    }
}
