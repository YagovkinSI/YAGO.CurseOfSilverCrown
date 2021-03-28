using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions
{
    public class IdlenessAction : BaseAction
    {
        private const int Step = 25000;
        private const int MinPower = 200000;

        protected override int ImportanceBase => 10;


        public IdlenessAction(Command command)
            : base(command)
        {
        }

        public override bool Execute()
        {
            var power = _command.Organization.Power;
            var realStep = (power < MinPower ? 1 : -1) * (int)Math.Round((_random.NextDouble() + 0.5) * Step);
            var newPower = power + realStep;
            _command.Organization.Power = newPower;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = newPower > power
                    ? Enums.enEventResultType.Growth
                    : Enums.enEventResultType.Idleness,
                Organizations = new List<EventOrganization>
                {
                    new EventOrganization
                    {
                        Id = _command.Organization.Id,
                        EventOrganizationType = Enums.enEventOrganizationType.Main,
                        EventOrganizationChanges = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = Enums.enEventParametrChange.Warrior,
                                Before = power / 2000,
                                After = newPower / 2000
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
                    Importance = ImportanceBase * Math.Abs(newPower - power) / 2000,
                    EventStory = EventStory
                }
            };                

            //_idlenessCommand.Result = newPower > power
            //    ? $"Набрано и обучено {(newPower / 2000) - (power / 2000)} воинов. Теперь у вас {(newPower / 2000)} воинов."
            //    : $"Лагерь покинуло {(power / 2000) - (newPower / 2000)} воинов. Теперь у вас {(newPower / 2000)} воинов.";

            return true;
        }

    }
}
