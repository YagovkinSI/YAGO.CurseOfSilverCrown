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
        protected override int ImportanceBase => 4000;

        public WarAction(Command command, Turn currentTurn)
            : base(command, currentTurn)
        {
        }

        public override bool Execute()
        {
            var startWarriorsAgressor = _command.Organization.Warriors;
            var startWarriorsTarget = _command.Target.Warriors;

            var startWarriorsAgressorUsed = _command.Warriors;
            var startWarriorsTargetUsed = _command.Target.Warriors - _command.Target.Commands
                .Where(c => c.Type == Enums.enCommandType.War).Sum(c => c.Warriors);

            var isRebellion = _command.Organization.SuzerainId == _command.TargetOrganizationId;

            var probabilityOfVictory = startWarriorsAgressorUsed / 4.0 / startWarriorsTargetUsed;
            var random = _random.NextDouble();
            var isVictory = random < probabilityOfVictory;

            var minWarrioirs = Math.Min(startWarriorsAgressorUsed, startWarriorsTargetUsed);
            var avarageAgressorLost = minWarrioirs * 0.25;
            var avarageTargetLost = minWarrioirs * 0.2;
            var realAgressorLost = (int)Math.Round((0.75 + _random.NextDouble() / 2) * avarageAgressorLost);
            var realTargetLost = (int)Math.Round((0.75 + _random.NextDouble() / 2) * avarageTargetLost);

            var newWarriorsAgressor = startWarriorsAgressor - realAgressorLost;
            var newWarriorsTarget = startWarriorsTarget - realTargetLost;

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
                    var executed = (int)Math.Round(newWarriorsTarget * 0.1 * (0.5 + _random.NextDouble()));
                    newWarriorsTarget -= executed;
                }
            }

            _command.Organization.Warriors = newWarriorsAgressor;
            _command.Target.Warriors = newWarriorsTarget;

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
                                Type = Enums.enEventParametrChange.WarriorInWar,
                                Before = startWarriorsAgressorUsed,
                                After = startWarriorsAgressorUsed - realAgressorLost
                            },
                            new EventParametrChange
                            {
                                Type = Enums.enEventParametrChange.Warrior,
                                Before = startWarriorsAgressor,
                                After = newWarriorsAgressor
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
                                Type = Enums.enEventParametrChange.WarriorInWar,
                                Before = startWarriorsTargetUsed,
                                After = startWarriorsTargetUsed - realTargetLost
                            },
                            new EventParametrChange
                            {
                                Type = Enums.enEventParametrChange.Warrior,
                                Before = startWarriorsTarget,
                                After = newWarriorsTarget
                            }
                        }
                    }
                }
            };

            EventStory = new EventStory
            {
                TurnId = currentTurn.Id,
                EventStoryJson = JsonConvert.SerializeObject(eventStoryResult)
            };

            OrganizationEventStories = new List<OrganizationEventStory>
            {
                new OrganizationEventStory
                {
                    Organization = _command.Organization,
                    Importance = isVictory
                        ? 100 * Math.Abs(startWarriorsAgressor - newWarriorsAgressor) + 5000
                        : 100 * Math.Abs(startWarriorsAgressor - newWarriorsAgressor),
                    EventStory = EventStory
                },
                new OrganizationEventStory
                {
                    Organization = _command.Target,
                    Importance = isVictory
                        ? 100 * Math.Abs(startWarriorsAgressor - newWarriorsAgressor) + 5000
                        : 100 * Math.Abs(startWarriorsAgressor - newWarriorsAgressor),
                    EventStory = EventStory
                }
            };

            return true;
        }

    }
}
