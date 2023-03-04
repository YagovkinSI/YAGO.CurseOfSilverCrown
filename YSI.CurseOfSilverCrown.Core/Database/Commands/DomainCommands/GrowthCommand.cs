using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Database.Commands.DomainCommands
{
    public class GrowthCommand : BaseCommand
    {
        public GrowthCommand(Command command)
            : base(command)
        {
            TypeInt = (int)enDomainCommandType.Growth;
        }

        public override string Name => "Сбор войск";

        public override string[] Descriptions => new[]
        {
            $"Сбор войск - действие позволяющее увеличить численность войск. В поселениях владения набираются люди, " +
            $"способные стать хорошими воинами. Они проходят обучение и получают созданное для них снаряжение." +
            $" Обучение одного воина обходится казне в {WarriorParameters.Price + WarriorParameters.Maintenance} золотых монет, " +
            $"{WarriorParameters.Price} из которых уходит на закупку и создание оружия, доспехов и другого снаряжения, " +
            $"а {WarriorParameters.Maintenance} тратится на содержание и обучение новобранцев.",
            $"При отдаче приказа вы можете оплатить из казны только {WarriorParameters.Price} монет на воина. " +
            $"Плата за содержание будет вычтена из казны после сбора налогов в этом сезоне.",
            $"Воины которых вы набираете эти приказом смогут принимать участие в боях только в следующем сезоне. " +
            $"При нападении в текущем сезоне эти воины не учитываются."
        };

        public override bool IsSingleCommand => true;

        public override bool NeedTarget => false;

        public override string TargetName => string.Empty;


        public override bool NeedTarget2 => false;

        public override string Target2Name => string.Empty;


        public override bool NeedCoffers => true;
        public override int StepCoffers => WarriorParameters.Price;

        public override bool NeedWarriors => false;
    }
}
