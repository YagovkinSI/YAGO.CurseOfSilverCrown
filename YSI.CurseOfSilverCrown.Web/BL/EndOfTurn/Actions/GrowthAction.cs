using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions
{
    public class GrowthAction
    {
        private Command _growthCommand;
        private Random _random = new Random();

        private const int MaxPower = 1000000;
        private const int MinAvarageStep = 40000;
        private const int MinPower = 200000;

        public GrowthAction(Command growthCommand)
        {
            _growthCommand = growthCommand;
        }

        internal bool Execute()
        {
            var power = _growthCommand.Organization.Power;
            var avarageGrowth = power < MinPower
                ? MinAvarageStep
                : (MaxPower - power) * 0.03;
            var realGrowth = (_random.NextDouble() + 0.5) * avarageGrowth;
            var newPower = power + (int)Math.Round(realGrowth);
            _growthCommand.Organization.Power = newPower;
            _growthCommand.Result = $"Набрано и обучено {(newPower / 2000) - (power / 2000)} воинов. Теперь у вас {(newPower / 2000)} воинов.";
            return true;
        }
    }
}
