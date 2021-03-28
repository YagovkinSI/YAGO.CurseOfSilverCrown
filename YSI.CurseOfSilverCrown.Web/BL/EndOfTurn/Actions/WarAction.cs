using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions
{
    public class WarAction : BaseAction
    {
        protected override int ImportanceBase => 100;

        public WarAction(Command command)
            : base(command)
        {
        }

        public override bool Execute()
        {
            var startPowerAgressor = _command.Organization.Power;
            var startPowerTarget = _command.Target.Power;

            var story = new StringBuilder();

            var isRebellion = _command.Organization.SuzerainId == _command.TargetOrganizationId;

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
                    _command.Organization.Suzerain = null;
                }
                else
                {
                    _command.Target.Suzerain = _command.Organization;
                }
            }
            else
            {
                if (isRebellion)
                {
                    var executed = (int)Math.Round(powerTarget * 0.1 * (0.5 + _random.NextDouble()));
                    powerTarget -= executed;
                }
            }

            _command.Organization.Power = powerAgressor;
            _command.Target.Power = powerTarget;
            _command.Result = story.ToString();

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = isRebellion
                    ? isVictory
                        ? Enums.enEventResultType.FastRebelionSuccess
                        : Enums.enEventResultType.FastRebelionFail
                    : isVictory
                        ? Enums.enEventResultType.FastWarSuccess
                        : Enums.enEventResultType.FastWarFail,
                Organizations = new List<EventOrganization>
                {
                    new EventOrganization
                    {
                        Id = _command.Organization.Id,
                        EventOrganizationType = Enums.enEventOrganizationType.Agressor,
                        EventOrganizationChanges = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = Enums.enEventParametrChange.Warrior,
                                Before = startPowerAgressor / 2000,
                                After = powerAgressor / 2000
                            }
                        }
                    },
                    new EventOrganization
                    {
                        Id = _command.Target.Id,
                        EventOrganizationType = Enums.enEventOrganizationType.Defender,
                        EventOrganizationChanges = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = Enums.enEventParametrChange.Warrior,
                                Before = startPowerTarget / 2000,
                                After = powerTarget / 2000
                            }
                        }
                    }
                }
            };

            EventStory = new EventStory
            {
                TurnId = _command.TurnId,
                EventStoryJson = JsonConvert.SerializeObject(eventStoryResult)
            };

            OrganizationEventStories = new List<OrganizationEventStory>
            {
                new OrganizationEventStory
                {
                    Organization = _command.Organization,
                    Importance = isVictory
                        ? ImportanceBase * 5 * Math.Abs(startPowerAgressor - powerAgressor) / 2000
                        : ImportanceBase * Math.Abs(startPowerAgressor - powerAgressor) / 2000,
                    EventStory = EventStory
                },
                new OrganizationEventStory
                {
                    Organization = _command.Target,
                    Importance = isVictory
                        ? ImportanceBase * 5 * Math.Abs(startPowerAgressor - powerAgressor) / 2000
                        : ImportanceBase * Math.Abs(startPowerAgressor - powerAgressor) / 2000,
                    EventStory = EventStory
                }
            };

            //story.AppendLine(isRebellion
            //    ? $"{_command.Organization.Name} (воинов - {_command.Organization.Power / 2000}) поднимает востание против сюзерена {_command.Target.Name} (воинов - {_command.Target.Power / 2000}).\r\n"
            //    : $"{_command.Organization.Name} (воинов - {_command.Organization.Power / 2000}) нападает на {_command.Target.Name} (воинов - {_command.Target.Power / 2000}) с целью захвата.\r\n");

            //story.AppendLine($"В ходе войны {_command.Organization.Name} теряет войнов - {(startPowerAgressor - powerAgressor) / 2000}. " +
            //    $"Теперь колчиество войнов у них - {powerAgressor / 2000}.\r\n");
            //story.AppendLine($"В ходе войны {_command.Target.Name} теряет войнов - {(startPowerTarget - powerTarget) / 2000}. " +
            //    $"Теперь колчиество войнов у них - {powerTarget / 2000}.");

            //story.AppendLine($"{_command.Organization.Name} побеждает в войне и снимает с себя вассальные обязательства.\r\n");
            //story.AppendLine($"{_command.Organization.Name} побеждает в войне. {_command.Target.Name} становится вассалом.\r\n");
            //story.AppendLine($"{_command.Target.Name} побеждает и казнит группу лилеров - {executed}.\r\n");
            //story.AppendLine($"{_command.Target.Name} побеждает в войне и сохраняет контроль над своими землями.\r\n");

            return true;
        }

    }
}
