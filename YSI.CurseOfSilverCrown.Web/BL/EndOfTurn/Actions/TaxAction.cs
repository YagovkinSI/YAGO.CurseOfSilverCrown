using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions
{
    public class TaxAction : BaseAction
    {
        protected override int ImportanceBase => 500;

        public TaxAction(Command command, Turn currentTurn)
            : base(command, currentTurn)
        {
        }

        public override bool Execute()
        {
            var coffers = _command.Organization.Coffers;
            var usedWarriors = _command.Warriors;
            var koef = Math.Pow(usedWarriors, 0.25) * (0.9 + _random.NextDouble() / 5.0);

            var getCoffers = (int)Math.Round(koef * 3000);

            var newCoffers = coffers + getCoffers;

            _command.Organization.Coffers = newCoffers;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = Enums.enEventResultType.TaxCollection,
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
                                Type = Enums.enEventParametrChange.Coffers,
                                Before = coffers,
                                After = newCoffers
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
                    Importance = getCoffers / 10,
                    EventStory = EventStory
                }
            };

            return true;
        }
    }
}
