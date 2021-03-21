using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions
{
    public class IdlenessAction
    {
        private Command _idlenessCommand;
        private Random _random = new Random();

        private const int Step = 25000;
        private const int MinPower = 200000;

        public IdlenessAction(Command growthCommand)
        {
            _idlenessCommand = growthCommand;
        }

        internal bool Execute()
        {
            var power = _idlenessCommand.Organization.Power;
            var realStep = (power < MinPower ? 1 : -1) * (int)Math.Round((_random.NextDouble() + 0.5) * Step);
            var newPower = power + realStep;
            _idlenessCommand.Organization.Power = newPower;
            _idlenessCommand.Result = newPower > power
                ? $"Набрано и обучено {(newPower / 2000) - (power / 2000)} воинов. Теперь у вас {(newPower / 2000)} воинов."
                : $"Лагерь покинуло {(power / 2000) - (newPower / 2000)} воинов. Теперь у вас {(newPower / 2000)} воинов.";
            return true;
        }

    }
}
