using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions
{
    public class WarAction
    {
        private Command _warCommand;
        private Random _random = new Random();

        public WarAction(Command growthCommand)
        {
            _warCommand = growthCommand;
        }

        internal bool Execute()
        {
            var startPowerAgressor = _warCommand.Organization.Power;
            var startPowerTarget = _warCommand.Target.Power;

            var story = new StringBuilder();

            var isRebellion = _warCommand.Organization.SuzerainId == _warCommand.TargetOrganizationId;
            story.AppendLine(isRebellion
                ? $"{_warCommand.Organization.Name} (воинов - {_warCommand.Organization.Power / 2000}) поднимает востание против сюзерена {_warCommand.Target.Name} (воинов - {_warCommand.Target.Power / 2000}).\r\n"
                : $"{_warCommand.Organization.Name} (воинов - {_warCommand.Organization.Power / 2000}) нападает на {_warCommand.Target.Name} (воинов - {_warCommand.Target.Power / 2000}) с целью захвата.\r\n");

            var probabilityOfVictory = startPowerAgressor / 4.0 / startPowerTarget;
            var random = _random.NextDouble();
            var isVictory = random < probabilityOfVictory;

            var minArmy = Math.Min(startPowerAgressor, startPowerTarget);
            var avarageAgressorLost = minArmy * 0.25;
            var avarageTargetLost = minArmy * 0.2;
            var realAgressorLost = (int)Math.Round((0.75 + _random.NextDouble() / 2) * avarageAgressorLost);
            var realTargetLost = (int)Math.Round((0.75 + _random.NextDouble() / 2) * avarageTargetLost);

            var powerAgressor = startPowerAgressor - realAgressorLost;
            var powerTarget = startPowerTarget - realTargetLost;

            if (isVictory)
            {
                if (isRebellion)
                {
                    _warCommand.Organization.Suzerain = null;
                    story.AppendLine($"{_warCommand.Organization.Name} побеждает в войне и снимает с себя вассальные обязательства.\r\n");
                }
                else
                {
                    _warCommand.Target.Suzerain = _warCommand.Organization;
                    story.AppendLine($"{_warCommand.Organization.Name} побеждает в войне. {_warCommand.Target.Name} становится вассалом.\r\n");
                }
            }
            else
            {
                if (isRebellion)
                {
                    var executed = (int)Math.Round(powerTarget * 0.1 * (0.5 + _random.NextDouble()));
                    story.AppendLine($"{_warCommand.Target.Name} побеждает и казнит группу лилеров - {executed}.\r\n");
                    powerTarget -= executed;
                }
                else
                {
                    story.AppendLine($"{_warCommand.Target.Name} побеждает в войне и сохраняет контроль над своими землями.\r\n");
                }
            }

            story.AppendLine($"В ходе войны {_warCommand.Organization.Name} теряет войнов - {(startPowerAgressor - powerAgressor) / 2000}. " +
                $"Теперь колчиество войнов у них - {powerAgressor / 2000}.\r\n");
            story.AppendLine($"В ходе войны {_warCommand.Target.Name} теряет войнов - {(startPowerTarget - powerTarget) / 2000}. " +
                $"Теперь колчиество войнов у них - {powerTarget / 2000}.");

            _warCommand.Organization.Power = powerAgressor;
            _warCommand.Target.Power = powerTarget;
            _warCommand.Result = story.ToString();
            return true;
        }

    }
}
