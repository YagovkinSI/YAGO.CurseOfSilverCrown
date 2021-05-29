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
        public RebelionCommand(Command command)
            : base(command)
        {
            TypeInt = (int)enCommandType.Rebellion;
        }

        public override string Name => "Востание против сюзерена";

        public override string[] Descriptions => 
            new[] 
        {
            $"Восстание против сюзерена - команда отказа от вассальной клятвы и объявление о независимости."
        };

        public override bool IsSingleCommand => true;

        public override bool NeedTarget => false;

        public override string TargetName => string.Empty;


        public override bool NeedTarget2 => false;

        public override string Target2Name => string.Empty;


        public override bool NeedCoffers => false;

        public override bool NeedWarriors => false;
    }
}
