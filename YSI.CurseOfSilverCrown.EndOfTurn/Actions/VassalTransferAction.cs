using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.BL.Models;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class VassalTransferAction : CommandActionBase
    {
        protected int ImportanceBase => 500;

        protected override bool RemoveCommandeAfterUse => true;

        public VassalTransferAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        protected override bool CheckValidAction()
        {
            if (Command.TargetDomainId == null)
                return false;
            var targetDomain = Context.GetDomainMin(Command.TargetDomainId.Value).Result;

            return Command.Type == enCommandType.VassalTransfer &&
                (targetDomain.SuzerainId == Domain.Id || 
                 targetDomain.Id == Domain.Id && Domain.SuzerainId == null) &&
                Command.Target2DomainId != null &&
                Command.Target2DomainId != Domain.Id &&
                Command.Status == enCommandStatus.ReadyToRun;
        }

        protected override bool Execute()
        {
            if (Command.TargetDomainId == Command.Target2DomainId && Command.TargetDomainId == Command.DomainId)
                return false;

            var vassal = Command.Target;
            var newSuzerain = Command.Target2;

            var isLiberation = vassal.Id == newSuzerain.Id;
            if (isLiberation)
            {
                vassal.Suzerain = null;
                vassal.SuzerainId = null;
                vassal.TurnOfDefeat = int.MinValue;
            }
            else
            {
                vassal.Suzerain = newSuzerain;
                vassal.SuzerainId = newSuzerain.Id;
                vassal.TurnOfDefeat = int.MinValue;

                var suzerain = newSuzerain;
                while (suzerain.SuzerainId != null)
                {
                    if (suzerain.SuzerainId == vassal.Id)
                    {
                        suzerain.SuzerainId = null;
                        suzerain.Suzerain = null;
                        suzerain.TurnOfDefeat = int.MinValue;
                        Context.Update(suzerain);
                        break;
                    }
                    suzerain = Context.Domains.Find(suzerain.SuzerainId);
                }
            }
            Context.Update(vassal);

            var type = isLiberation
                    ? enEventResultType.Liberation
                    : Command.DomainId == vassal.Id
                        ? enEventResultType.VoluntaryOath
                        : enEventResultType.ChangeSuzerain;
            var eventStoryResult = new EventStoryResult(type);
            eventStoryResult.AddEventOrganization(Command.DomainId, enEventOrganizationType.Main, new List<EventParametrChange>());
            eventStoryResult.AddEventOrganization(Command.TargetDomainId.Value, enEventOrganizationType.Vasal, new List<EventParametrChange>());
            eventStoryResult.AddEventOrganization(Command.Target2DomainId.Value, enEventOrganizationType.Suzerain, new List<EventParametrChange>());

            var dommainEventStories = new Dictionary<int, int>
            {
                { Domain.Id, 5000 }
            };
            if (!dommainEventStories.ContainsKey(Command.TargetDomainId.Value))
                dommainEventStories.Add(Command.TargetDomainId.Value, 5000);
            if (!dommainEventStories.ContainsKey(Command.Target2DomainId.Value))
                dommainEventStories.Add(Command.Target2DomainId.Value, 5000);
            CreateEventStory(eventStoryResult, dommainEventStories);

            return true;
        }
    }
}
