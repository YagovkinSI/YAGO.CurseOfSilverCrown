using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Commands
{
    public class WarSupportDefenseCommand : BaseCommand
    {
        public WarSupportDefenseCommand(Command command)
            : base(command)
        {
            Type = enCommandType.WarSupportDefense;
        }

        public override string Name => "Защита провинции";

        public override string[] Descriptions => new[]
        {
            $"Защита провинции - команда защищать провинцию от нападений. " +
            $"Воины, оствашиеся в провинции без приказа защиты защищают её с эфективностью в " +
            $"{((int) Math.Round(WarConstants.WariorDefenseTax*100))}%.",
            $"Если вы отправляете воинов защищать чужую провинцию, то они не смогут в этом ходу защищать вашу провинцию."
        };            

        public override bool NeedTarget => true;

        public override string TargetName => "Защита провинции";

        public override bool NeedTarget2 => false;

        public override string Target2Name => string.Empty;

        public override bool NeedCoffers => false;

        public override bool NeedWarriors => true;
    }
}
