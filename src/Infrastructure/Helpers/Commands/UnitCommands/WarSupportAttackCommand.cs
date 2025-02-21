using YAGO.World.Infrastructure.Database.Models.Units;

namespace YAGO.World.Infrastructure.Helpers.Commands.UnitCommands
{
    public class WarSupportAttackCommand : BaseCommand
    {
        public WarSupportAttackCommand(Unit command)
            : base(command)
        {
            TypeInt = (int)UnitCommandType.WarSupportAttack;
        }

        public override string Name => "Помощь в нападении";

        public override string[] Descriptions => new[]
        {
            "Помощь в нападении - команда помочь одному владению атаковать другое.",
            "Если вы отправляете воинов помогать в нападении, то они не смогут в этом ходу защищать ваше владение."
        };

        public override bool IsSingleCommand => false;

        public override bool NeedTarget => true;

        public override string TargetName => "В нападении на владение";


        public override bool NeedTarget2 => true;

        public override string Target2Name => "Помочь владению";


        public override bool NeedCoffers => false;

        public override bool NeedWarriors => true;
    }
}
