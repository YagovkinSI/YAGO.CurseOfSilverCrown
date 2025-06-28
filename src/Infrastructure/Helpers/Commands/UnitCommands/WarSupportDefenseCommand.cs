using YAGO.World.Domain.Units.Enums;
using YAGO.World.Infrastructure.Database.Models.Units;

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
            $"Защита владения - команда защищать владение от нападений."
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
