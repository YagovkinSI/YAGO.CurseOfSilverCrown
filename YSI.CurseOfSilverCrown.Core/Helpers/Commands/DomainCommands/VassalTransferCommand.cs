using YSI.CurseOfSilverCrown.Core.Database.Commands;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Commands.DomainCommands
{
    public class VassalTransferCommand : BaseCommand
    {
        public VassalTransferCommand(Command command)
            : base(command)
        {
            TypeInt = (int)CommandType.VassalTransfer;
        }

        public override string Name => "Передача вассала";

        public override string[] Descriptions => new[]
        {
            $"С помощью данной команды можно: ",
            $"1. Передать своего вассала под покровительство любой другого владения (при этом вы отказываетесь от вассала). " +
            $"Для этого в первой графе \"Передать владение\" выберите вассала, от которого хотите отказаться, " +
            "а в графе \"Под покровительство владению\" выберите провинцию которая получит жту провинцию себе в вассалы. ",
            "2. Освободить вассала, даровав ему независимости (при этом вы отказываетесь от вассала). " +
            "Для этого в обоих графах (\"Передать владение\" и  \"Под покровительство владению\") " +
            "выберите вассала, от которого хотите отказаться. ",
            "3. Добровольно стать вассалов любого другого владения (при этом вы теряете независимость). " +
            "Для этого в первой графе \"Передать владение\" выберите свою провинцию, " +
            "а в графе \"Под покровительство владению\" выберите владение котрому будете подчиняться. " +
            "Если у Вас уже есть сюзерен, то эта опция не доступна."
        };

        public override bool IsSingleCommand => false;

        public override bool NeedTarget => true;

        public override string TargetName => "Передать владение";

        public override bool NeedTarget2 => true;

        public override string Target2Name => "Под покровительство владению";

        public override bool NeedCoffers => false;

        public override bool NeedWarriors => false;
    }
}
