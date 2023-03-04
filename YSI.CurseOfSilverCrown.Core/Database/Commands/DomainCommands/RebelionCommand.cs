namespace YSI.CurseOfSilverCrown.Core.Database.Commands.DomainCommands
{
    public class RebelionCommand : BaseCommand
    {
        public RebelionCommand(Command command)
            : base(command)
        {
            TypeInt = (int)enDomainCommandType.Rebellion;
        }

        public override string Name => "Востание против сюзерена";

        public override string[] Descriptions =>
            new[]
        {
            $"Восстание против сюзерена - команда отказа от вассальной клятвы и объявление о независимости."
        };

        public override bool IsSingleCommand => true;

        public override bool NeedTarget => false;

        public override string TargetName => string.Empty;


        public override bool NeedTarget2 => false;

        public override string Target2Name => string.Empty;


        public override bool NeedCoffers => false;

        public override bool NeedWarriors => false;
    }
}
