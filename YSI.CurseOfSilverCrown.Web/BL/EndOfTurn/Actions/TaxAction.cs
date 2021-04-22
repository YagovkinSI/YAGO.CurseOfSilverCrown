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

        public static int GetTax(int warriors, int investments, double random)
        {
            var additionalWarriors = warriors - Constants.MinTaxAuthorities;
            var baseTax = Constants.MinTax;
            var randomBaseTax = baseTax * (0.99 + random / 100.0);

            var investmentTax = Constants.GetInvestmentTax(investments);
            var randomInvestmentTax = investmentTax * (0.99 + random / 100.0);

            var additionalTax = Constants.GetAdditionalTax(additionalWarriors, random);

            return (int)Math.Round(randomBaseTax + randomInvestmentTax + additionalTax);
        }

        public override bool Execute()
        {
            var coffers = _command.Organization.Coffers;
            var usedWarriors = _command.Warriors;

            var getCoffers = GetTax(usedWarriors, _command.Organization.Investments, _random.NextDouble());

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
                    Importance = getCoffers / 20,
                    EventStory = EventStory
                }
            };

            return true;
        }
    }
}
