using System;
using YSI.CurseOfSilverCrown.Core.Database.Units;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Commands.UnitCommands
{
    public class CollectTaxCommand : BaseCommand
    {
        public CollectTaxCommand(Unit command)
            : base(command)
        {
            TypeInt = (int)UnitCommandType.CollectTax;
        }

        public override string Name => "Временный роспуск отряда";

        public override string[] Descriptions => new[]
        {
            "Временный роспуск отряда - вы отправляете воинов по домам, экономя часть козны, но и уменьшая эффективность отрядов.",
            $"Распущенные отряды также будут участвовать в защите владения, но с эффективностью {Math.Round(WarConstants.WariorDefenseTax*100, 2)}%."
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
