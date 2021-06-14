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
    public class InvestmentsCommand : BaseCommand
    {
        public InvestmentsCommand(Command command)
            : base(command)
        {
            TypeInt = (int)enCommandType.Investments;
        }

        public override string Name => "Вложение средств в имущество владения";

        public override string[] Descriptions => new[] 
        {
            $"Вложение средств в имущество владения - действие позволяющее выполнить инвестиции во владение, " +
            $"чтобы собирать больше налогов. Вложения подразумевают постройку дорог и мельниц, налаживание торговли и другое. " +
            $"Инвестиции угасают при перенасыщении, то есть первая тысяча инвестиции окупиться за 2 хода, " +
            $"двадцатая будет окупаться 10 ходов и т.д. В целом вкладывать деньги в инвестиции можно до бесконечности, " +
            $"но с каждым разом они будут всё дольше окупаться."
        };

        public override bool IsSingleCommand => true;

        public override bool NeedTarget => false;

        public override string TargetName => string.Empty;


        public override bool NeedTarget2 => false;

        public override string Target2Name => string.Empty;


        public override bool NeedCoffers => true;

        public override bool NeedWarriors => false;
    }
}
