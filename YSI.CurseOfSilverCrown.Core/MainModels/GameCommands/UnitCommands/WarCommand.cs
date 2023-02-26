using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;

namespace YSI.CurseOfSilverCrown.Core.MainModels.GameCommands.UnitCommands
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

        public override string TargetName => "Нападение на владение";


        public override bool NeedTarget2 => false;

        public override string Target2Name => string.Empty;


        public override bool NeedCoffers => false;

        public override bool NeedWarriors => true;
    }
}
