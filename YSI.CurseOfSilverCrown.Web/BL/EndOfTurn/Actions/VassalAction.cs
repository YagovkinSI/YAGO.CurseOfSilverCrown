using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions
{
    public class VassalAction
    {
        private Random _random = new Random();
        private Organization organization;
        private Turn currentTurn;

        private const double DefaultTax = 0.02;

        public List<Command> NewCommands = new List<Command>();

        public VassalAction(Organization organization, Turn currentTurn)
        {
            this.organization = organization;
            this.currentTurn = currentTurn;
        }

        internal bool Execute()
        {
            var suzerain = organization.Suzerain;

            var startVassalPower = organization.Power;
            var startSuzerainPower = suzerain.Power;

            var realStep = (int)Math.Round((_random.NextDouble() / 2 + 0.75) * startVassalPower * DefaultTax);
            var newVassalPower = startVassalPower - realStep;
            organization.Power = newVassalPower;

            var income = startSuzerainPower > 2 * startVassalPower
                ? startVassalPower / startSuzerainPower * realStep
                : realStep;
            var newSuzerainPower = startSuzerainPower + income;
            suzerain.Power = newSuzerainPower;

            var vassalCommand = new Command
            {
                Id = Guid.NewGuid().ToString(),
                Organization = organization,
                Target = suzerain,
                Type = Enums.enCommandType.VassalTax,
                Turn = currentTurn,
                Result = $"Из-за вассального налога вы сокращаете воинов - {(startVassalPower - newVassalPower) / 2000}. Теперь у вас войнов - {newVassalPower / 2000}."
            };
            var suzeraindCommand = new Command
            {
                Id = Guid.NewGuid().ToString(),
                Organization = suzerain,
                Target = organization,
                Type = Enums.enCommandType.SuzerainIncome,
                Turn = currentTurn,
                Result = $"На налоги от вассаала {organization.Name} вы нанимаете воинов - {(newSuzerainPower - startSuzerainPower) / 2000}.  Теперь у вас войнов - {newSuzerainPower / 2000}"
            };

            NewCommands = new List<Command>() { vassalCommand, suzeraindCommand };

            return true;
        }

    }
}
