using System;
using YAGO.World.Infrastructure.Database.Models.Units;
using YAGO.World.Infrastructure.Parameters;

namespace YAGO.World.Infrastructure.Helpers.Commands.UnitCommands
{
    public class WarSupportDefenseCommand : BaseCommand
    {
        public WarSupportDefenseCommand(Unit command)
            : base(command)
        {
            TypeInt = (int)UnitCommandType.WarSupportDefense;
        }

        public override string Name => "Защита владения";

        public override string[] Descriptions => new[]
        {
            $"Защита владения - команда защищать владение от нападений. " +
            $"Воины, оствашиеся во владении без приказа защиты защищают её с эфективностью в " +
            $"{(int) Math.Round(WarConstants.WariorDefenseTax*100)}%.",
            $"Если вы отправляете воинов защищать чужое владение, то они не смогут в этом ходу защищать вашу провинцию."
        };

        public override bool IsSingleCommand => false;

        public override bool NeedTarget => true;

        public override string TargetName => "Защита владения";

        public override bool NeedTarget2 => false;

        public override string Target2Name => string.Empty;

        public override bool NeedCoffers => false;

        public override bool NeedWarriors => true;
    }
}
