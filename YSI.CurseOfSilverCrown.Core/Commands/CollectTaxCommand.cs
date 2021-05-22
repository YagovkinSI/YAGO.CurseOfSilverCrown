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
    public class CollectTaxCommand : BaseCommand
    {
        public CollectTaxCommand(Unit command)
            : base(command)
        {
            TypeInt = (int)enArmyCommandType.CollectTax;
        }

        public override string Name => "Дополнительный сбор налогов";

        public override string[] Descriptions => new[] 
        {
            "Дополнительный сбор налогов - вы отправляете дополнительные силы в деревни для охраны порядка, а также сбора дополнительныого налога.",
            $"Все воины, выполняющие контроль провинции также будут участвовать в защите провинции, но лишь на {WarConstants.WariorDefenseTax*100}%."
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
