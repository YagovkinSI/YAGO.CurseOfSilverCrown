using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class GrowthAction : CommandActionBase
    {
        protected int ImportanceBase => 500;

        protected override bool RemoveCommandeAfterUse => true;

        public GrowthAction(ApplicationDbContext context, Turn currentTurn, Command command) 
            : base(context, currentTurn, command)
        {
        }

        protected override bool CheckValidAction()
        {
            FixCoffersForAction();

            return Command.Type == enCommandType.Growth &&
                Command.Coffers >= WarriorParameters.Price &&
                Command.Status == enCommandStatus.ReadyToRun;
        }

        protected override bool Execute()
        {
            var coffers = Command.Domain.Coffers;
            var warriors = DomainHelper.GetWarriorCount(Context, Command.Domain.Id);

            var spentCoffers = Math.Min(coffers, Command.Coffers);
            var getWarriors = spentCoffers / WarriorParameters.Price;

            var newCoffers = coffers - spentCoffers;
            var newWarriors = warriors + getWarriors;

            Command.Domain.Coffers = newCoffers;
            DomainHelper.SetWarriorCount(Context, Command.Domain.Id, newWarriors);

            var eventStoryResult = new EventStoryResult(enEventResultType.Growth);
            var temp = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enActionParameter.Warrior,
                                Before = warriors,
                                After = newWarriors
                            },
                            new EventParametrChange
                            {
                                Type = enActionParameter.Coffers,
                                Before = coffers,
                                After = newCoffers
                            }
                        };
            eventStoryResult.AddEventOrganization(Command.Domain, enEventOrganizationType.Main, temp);

            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, getWarriors * 50}
            };
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }
    }
}
