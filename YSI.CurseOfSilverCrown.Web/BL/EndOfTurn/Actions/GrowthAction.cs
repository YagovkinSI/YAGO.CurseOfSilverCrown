using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Constants;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions
{
    public class GrowthAction : BaseAction
    {
        protected override int ImportanceBase => 500;

        public GrowthAction(Command command, Turn currentTurn) 
            : base(command, currentTurn)
        {
        }

        public override bool Execute()
        {
            var coffers = _command.Organization.Coffers;
            var warriors = _command.Organization.Warriors;

            var spentCoffers = Math.Min(coffers, _command.Coffers);
            var getWarriors = spentCoffers / Constants.OutfitWarrioir;

            var newCoffers = coffers - spentCoffers;
            var newWarriors = warriors + getWarriors;

            _command.Organization.Coffers = newCoffers;
            _command.Organization.Warriors = newWarriors;

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = enEventResultType.Growth,
                Organizations = new List<EventOrganization>
                {
                    new EventOrganization
                    {
                        Id = _command.Organization.Id,
                        EventOrganizationType = enEventOrganizationType.Main,
                        EventOrganizationChanges = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enEventParametrChange.Warrior,
                                Before = warriors,
                                After = newWarriors
                            },
                            new EventParametrChange
                            {
                                Type = enEventParametrChange.Coffers,
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
                    Importance = getWarriors * 50,
                    EventStory = EventStory
                }
            };

            return true;
        }
    }
}
