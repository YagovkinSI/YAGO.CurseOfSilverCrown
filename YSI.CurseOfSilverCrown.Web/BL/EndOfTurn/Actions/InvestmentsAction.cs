using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions
{
    public class InvestmentsAction : BaseAction
    {
        protected override int ImportanceBase => 500;

        public InvestmentsAction(Command command, Turn currentTurn) 
            : base(command, currentTurn)
        {
        }

        public override bool Execute()
        {
            var coffers = _command.Organization.Coffers;
            var investments = _command.Organization.Investments;

            var spentCoffers = Math.Min(coffers, _command.Coffers);
            var getInvestments = spentCoffers;

            var newCoffers = coffers - spentCoffers;
            var newInvestments = investments + getInvestments;

            _command.Organization.Coffers = newCoffers;
            _command.Organization.Investments = newInvestments;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = Enums.enEventResultType.Investments,
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
                                Type = Enums.enEventParametrChange.Investments,
                                Before = investments,
                                After = newInvestments
                            },
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
                    Importance = spentCoffers / 4,
                    EventStory = EventStory
                }
            };

            return true;
        }
    }
}
