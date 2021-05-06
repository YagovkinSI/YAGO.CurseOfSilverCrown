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

        public EventStory EventStory { get; private set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; private set; }

        public VassalTransferAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        public override bool Execute()
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

            var eventStoryResult = new EventStoryResult
            {
                EventResultType = isLiberation 
                    ? enEventResultType.Liberation 
                    : Command.OrganizationId == vassal.Id
                        ? enEventResultType.VoluntaryOath
                        : enEventResultType.ChangeSuzerain,
                Organizations = new List<EventOrganization>
                {
                    new EventOrganization
                    {
                        Id = Command.Organization.Id,
                        EventOrganizationType = enEventOrganizationType.Main,
                        EventOrganizationChanges = new List<EventParametrChange>()
                    },
                    new EventOrganization
                    {
                        Id = Command.Target.Id,
                        EventOrganizationType = enEventOrganizationType.Vasal,
                        EventOrganizationChanges = new List<EventParametrChange>()
                    },
                    new EventOrganization
                    {
                        Id = Command.Target2.Id,
                        EventOrganizationType = enEventOrganizationType.Suzerain,
                        EventOrganizationChanges = new List<EventParametrChange>()
                    }
                }
            };

            EventStory = new EventStory
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = JsonConvert.SerializeObject(eventStoryResult)
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
