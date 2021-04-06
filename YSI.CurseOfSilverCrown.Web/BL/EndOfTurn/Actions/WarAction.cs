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

            var isRebellion = _command.Organization.SuzerainId == _command.TargetOrganizationId;

            var probabilityOfVictory = startWarriorsAgressor / 4.0 / startWarriorsTarget;
            var random = _random.NextDouble();
            var isVictory = random < probabilityOfVictory;

            var minWarrioirs = Math.Min(startWarriorsAgressor, startWarriorsTarget);
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
                        ? ImportanceBase * 5 * Math.Abs(startWarriorsAgressor - newWarriorsAgressor) / 2000
                        : ImportanceBase * Math.Abs(startWarriorsAgressor - newWarriorsAgressor) / 2000,
                    EventStory = EventStory
                },
                new OrganizationEventStory
                {
                    Organization = _command.Target,
                    Importance = isVictory
                        ? ImportanceBase * 5 * Math.Abs(startWarriorsAgressor - newWarriorsAgressor) / 2000
                        : ImportanceBase * Math.Abs(startWarriorsAgressor - newWarriorsAgressor) / 2000,
                    EventStory = EventStory
                }
            };

            return true;
        }

    }
}
