using YAGO.World.Domain.Units.Enums;
using YAGO.World.Infrastructure.Database.Models.Units;

namespace YAGO.World.Infrastructure.Helpers.Commands.UnitCommands
{
    public class DisbandmentCommand : BaseCommand
    {
        public DisbandmentCommand(Unit command)
            : base(command)
        {
            TypeInt = (int)UnitCommandType.Disbandment;
        }

        public override string Name => "Роспуск отряда";

        public override string[] Descriptions => new[]
        {
            "Роспуск отряда удалит отряд. Вы не получите компенсации, но перестанете тратить средства на соержание отряда."
        };

        public override bool IsSingleCommand => true;

        public override bool NeedTarget => false;

        public override string TargetName => string.Empty;

        public override int? TargetId => DomainId;


        public override bool NeedTarget2 => false;

        public override string Target2Name => string.Empty;


        public override bool NeedCoffers => false;

        public override bool NeedWarriors => true;
    }
}
