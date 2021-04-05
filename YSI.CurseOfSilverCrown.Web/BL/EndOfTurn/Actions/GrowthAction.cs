using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions
{
    public class GrowthAction : BaseAction
    {
        private const int MaxPower = 1000000;
        private const int MinAvarageStep = 40000;
        private const int MinPower = 200000;

        protected override int ImportanceBase => 20;


        public GrowthAction(Command command) 
            : base(command)
        {
        }

        public override bool Execute()
        {
            var power = _command.Organization.Warriors;
            var avarageGrowth = power < MinPower
                ? MinAvarageStep
                : (MaxPower - power) * 0.03;
            var realGrowth = (_random.NextDouble() + 0.5) * avarageGrowth;
            var newPower = power + (int)Math.Round(realGrowth);
            _command.Organization.Warriors = newPower;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = Enums.enEventResultType.Growth,
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

            //_command.Result = $"Набрано и обучено {(newPower / 2000) - (power / 2000)} воинов. Теперь у вас {(newPower / 2000)} воинов.";
            return true;
        }
    }
}
