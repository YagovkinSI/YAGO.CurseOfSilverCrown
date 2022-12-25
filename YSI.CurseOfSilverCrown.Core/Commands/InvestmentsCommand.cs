using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;

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
            $"Инвестиции угасают при перенасыщении, например, при имуществе до 100.000 вложения будут окупаться за 3-4 хода, " +
            $"а далее за 5 и более ходов. В целом вкладывать деньги в инвестиции можно до бесконечности."
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
