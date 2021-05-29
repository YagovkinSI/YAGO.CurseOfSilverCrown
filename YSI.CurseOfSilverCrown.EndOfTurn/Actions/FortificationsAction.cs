using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Core.Commands;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class FortificationsAction : CommandActionBase
    {
        protected int ImportanceBase => 500;

        protected override bool RemoveCommandeAfterUse => true;

        public FortificationsAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        protected override bool CheckValidAction()
        {
            FixCoffersForAction();

            return Command.Type == enCommandType.Fortifications &&
                Command.Coffers > 0 &&
                Command.Status == enCommandStatus.ReadyToMove;
        }

        protected override bool Execute()
        {
            var coffers = Command.Domain.Coffers;
            var fortifications = Command.Domain.Fortifications;

            var spentCoffers = Math.Min(coffers, Command.Coffers);
            var getFortifications = spentCoffers;

            var newCoffers = coffers - spentCoffers;
            var newFortifications = fortifications + getFortifications;

            Command.Domain.Coffers = newCoffers;
            Command.Domain.Fortifications = newFortifications;

            var eventStoryResult = new EventStoryResult(enEventResultType.Fortifications);
            var eventOrganizationChanges = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enActionParameter.Fortifications,
                                Before = FortificationsHelper.GetDefencePercent(fortifications),
                                After = FortificationsHelper.GetDefencePercent(newFortifications),
                            },
                            new EventParametrChange
                            {
                                Type = enActionParameter.Coffers,
                                Before = coffers,
                                After = newCoffers
                            }
                        };
            eventStoryResult.AddEventOrganization(Command.DomainId, enEventOrganizationType.Main, eventOrganizationChanges);

            var dommainEventStories = new Dictionary<int, int>
            {
                { Command.Domain.Id, spentCoffers / 4}
            };
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }
    }
}
