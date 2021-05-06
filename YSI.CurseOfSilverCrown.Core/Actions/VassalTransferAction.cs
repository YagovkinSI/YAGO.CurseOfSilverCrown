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
            if (Command.TargetOrganizationId == Command.Target2OrganizationId && Command.TargetOrganizationId == Command.OrganizationId)
                return false;

            var vassal = Command.Target;
            var newSuzerain = Command.Target2;

            var isLiberation = vassal.Id == newSuzerain.Id;
            if (isLiberation)
            {
                vassal.Suzerain = null;
                vassal.SuzerainId = null;
            }
            else
            {
                vassal.Suzerain = newSuzerain;
                vassal.SuzerainId = newSuzerain.Id;
            }

            var type = isLiberation
                    ? enEventResultType.Liberation
                    : Command.OrganizationId == vassal.Id
                        ? enEventResultType.VoluntaryOath
                        : enEventResultType.ChangeSuzerain;
            var eventStoryResult = new EventStoryResult(type);
            eventStoryResult.AddEventOrganization(Command.Organization, enEventOrganizationType.Main, new List<EventParametrChange>());
            eventStoryResult.AddEventOrganization(Command.Target, enEventOrganizationType.Vasal, new List<EventParametrChange>());
            eventStoryResult.AddEventOrganization(Command.Target2, enEventOrganizationType.Suzerain, new List<EventParametrChange>());

            EventStory = new EventStory
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = eventStoryResult.ToJson()
            };


            OrganizationEventStories = new List<OrganizationEventStory>
            {
                new OrganizationEventStory
                {
                    Organization = Command.Organization,
                    Importance = 5000,
                    EventStory = EventStory
                }
            };
            if (Command.OrganizationId != Command.TargetOrganizationId)
                OrganizationEventStories.Add(new OrganizationEventStory
                {
                    Organization = Command.Target,
                    Importance = 5000,
                    EventStory = EventStory
                });
            if (Command.OrganizationId != Command.Target2OrganizationId && Command.TargetOrganizationId != Command.Target2OrganizationId)
                OrganizationEventStories.Add(new OrganizationEventStory
                {
                    Organization = Command.Target2,
                    Importance = 5000,
                    EventStory = EventStory
                });

            return true;
        }
    }
}
