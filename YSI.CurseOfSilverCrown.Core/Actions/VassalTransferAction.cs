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
using YSI.CurseOfSilverCrown.Core.Event;
using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal class VassalTransferAction : ActionBase
    {
        protected int ImportanceBase => 500;

        public VassalTransferAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
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
            eventStoryResult.AddEventOrganization(Command.Domain, enEventOrganizationType.Main, new List<EventParametrChange>());
            eventStoryResult.AddEventOrganization(Command.Target, enEventOrganizationType.Vasal, new List<EventParametrChange>());
            eventStoryResult.AddEventOrganization(Command.Target2, enEventOrganizationType.Suzerain, new List<EventParametrChange>());

            EventStory = new EventStory
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = eventStoryResult.ToJson()
            };


            OrganizationEventStories = new List<DomainEventStory>
            {
                new DomainEventStory
                {
                    Domain = Command.Domain,
                    Importance = 5000,
                    EventStory = EventStory
                }
            };
            if (Command.DomainId != Command.TargetDomainId)
                OrganizationEventStories.Add(new DomainEventStory
                {
                    Domain = Command.Target,
                    Importance = 5000,
                    EventStory = EventStory
                });
            if (Command.DomainId != Command.Target2DomainId && Command.TargetDomainId != Command.Target2DomainId)
                OrganizationEventStories.Add(new DomainEventStory
                {
                    Domain = Command.Target2,
                    Importance = 5000,
                    EventStory = EventStory
                });

            return true;
        }
    }
}
