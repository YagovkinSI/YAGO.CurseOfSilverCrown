namespace YSI.CurseOfSilverCrown.Core.MainModels.Commands.DomainCommands
{
    public class GoldTransferCommand : BaseCommand
    {
        public GoldTransferCommand(Command command)
            : base(command)
        {
            TypeInt = (int)enDomainCommandType.GoldTransfer;
        }

        public override string Name => "Отправка золота";

        public override string[] Descriptions => new[]
        {
            $"Отправка золота - отправка средств из казны в другое владение для помощи в развитии или оплаты оговоренных услуг.",
            $"За один сезон вы не можете отправить больше {GoldTransferHelper.MaxGoldTransfer} золотых монет."
        };

        public override bool IsSingleCommand => false;

        public override bool NeedTarget => true;

        public override string TargetName => "Отправить золото во владение";

        public override bool NeedTarget2 => false;

        public override string Target2Name => string.Empty;

        public override bool NeedCoffers => true;
        public override int MaxCoffers => GoldTransferHelper.MaxGoldTransfer;

        public override bool NeedWarriors => false;
    }
}
