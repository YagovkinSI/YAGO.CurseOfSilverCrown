using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.Core.Commands
{
    public class WarSupportAttackCommand : BaseCommand
    {
        public WarSupportAttackCommand(Command command)
            : base(command)
        {
            Type = enCommandType.WarSupportAttack;
        }

        public override string Name => "Помощь в нападении";

        public override string[] Descriptions => new[] 
        {
            "Помощь в нападении - команда помочь одной провинции атаковать другую.",
            "Если вы отправляете воинов помогать в нападении, то они не смогут в этом ходу защищать вашу провинцию."
        }; 

        public override bool NeedTarget => true;

        public override string TargetName => "В нападении на провинцию";


        public override bool NeedTarget2 => true;

        public override string Target2Name => "Помочь провинции";


        public override bool NeedCoffers => false;

        public override bool NeedWarriors => true;
    }
}
