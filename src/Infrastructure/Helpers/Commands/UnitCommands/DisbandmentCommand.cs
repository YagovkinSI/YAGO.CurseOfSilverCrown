using System;
using YAGO.World.Domain.Units.Enums;
using YAGO.World.Infrastructure.Database.Models.Units;
using YAGO.World.Infrastructure.Parameters;

namespace YAGO.World.Infrastructure.Helpers.Commands.UnitCommands
{
    public class DisbandmentCommand : BaseCommand
    {
        public DisbandmentCommand(Unit command)
            : base(command)
        {
            TypeInt = (int)UnitCommandType.Disbandment;
        }

        public override string Name => "Временный роспуск отряда";

        public override string[] Descriptions => new[]
        {
            "Временный роспуск отряда - вы отправляете воинов по домам, экономя часть козны, но и уменьшая эффективность отрядов.",
            $"Распущенные отряды возвращаются в домашнее владение. " +
            $"Они также будут участвовать в защите, но со штрафом к морали {Math.Round(WarConstants.WariorDisbandmentMoralityPenalty*100, 2)}%."
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
