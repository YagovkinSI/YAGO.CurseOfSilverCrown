using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.Core.Commands;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.BL.Models.Min;
using YSI.CurseOfSilverCrown.Core.BL.Models;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class RebelionAction : WarBaseAction
    {
        private DomainMin Domain { get; set; } 

        public RebelionAction(ApplicationDbContext context, Turn currentTurn, int unitId)
            : base(context, currentTurn, unitId)
        {
            Domain = Context.GetDomainMin(Unit.DomainId).Result;
        }

        protected override bool CheckValidAction()
        {
            return Unit.Type == enArmyCommandType.Rebellion &&
                Unit.TargetDomainId != null &&
                Unit.TargetDomainId == Domain.SuzerainId &&
                Unit.PositionDomainId == Domain.SuzerainId &&
                Unit.Warriors > 0 && 
                Unit.Status == enCommandStatus.ReadyToRun;
        }

        protected override void SetFinalOfWar(List<WarParticipant> warParticipants, bool isVictory)
        {
            if (isVictory)
            {
                Unit.Domain.SuzerainId = null;
                Unit.Domain.Suzerain = null;
                Unit.Domain.TurnOfDefeat = int.MinValue;
            }
            else
            {
                warParticipants
                    .Single(p => p.Type == enTypeOfWarrior.Agressor)
                    .SetExecuted();
                Unit.Domain.TurnOfDefeat = CurrentTurn.Id;
            }
        }

        protected override void CreateEvent(List<WarParticipant> warParticipants, bool isVictory)
        {
            var organizationsParticipants = warParticipants
                .GroupBy(p => p.Organization.Id);

            var type = isVictory
                        ? enEventResultType.FastRebelionSuccess
                        : enEventResultType.FastRebelionFail;
            var eventStoryResult = new EventStoryResult(type);
            FillEventOrganizationList(eventStoryResult, organizationsParticipants);

            var importance = warParticipants.Sum(p => p.WarriorLosses) * 50 + (isVictory ? 5000 : 0);

            var dommainEventStories = organizationsParticipants.ToDictionary(
                o => o.Key,
                o => importance);
            CreateEventStory(eventStoryResult, dommainEventStories);
        }
    }
}
