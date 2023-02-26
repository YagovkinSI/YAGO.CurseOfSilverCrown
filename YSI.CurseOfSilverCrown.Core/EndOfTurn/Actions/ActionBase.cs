using System;
using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.EventDomains;
using YSI.CurseOfSilverCrown.Core.MainModels.Events;
using YSI.CurseOfSilverCrown.Core.MainModels.Turns;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    public abstract class ActionBase
    {
        public ApplicationDbContext Context { get; }
        protected Turn CurrentTurn { get; }
        protected Random Random { get; } = new Random();

        private Core.MainModels.Events.Event EventStory { get; set; }
        private List<EventDomain> OrganizationEventStories { get; set; }

        public ActionBase(ApplicationDbContext context, Turn currentTurn)
        {
            Context = context;
            CurrentTurn = currentTurn;
        }

        public int ExecuteAction(int number)
        {
            var isValid = CheckValidAction();
            var isActionEnd = isValid
                ? Execute()
                : true;

            if (isActionEnd && EventStory != null)
            {
                EventStory.Id = number;
                number++;
                Context.Add(EventStory);
                Context.AddRange(OrganizationEventStories);
                if (this is CommandActionBase commandActionBase)
                    commandActionBase.CheckAndDeleteCommand();
            }
            return number;
        }

        public abstract bool CheckValidAction();

        protected abstract bool Execute();

        internal void CreateEventStory(EventStoryResult eventStory, Dictionary<int, int> domains)
        {
            EventStory = new Core.MainModels.Events.Event
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = eventStory.ToJson()
            };

            OrganizationEventStories = new List<EventDomain>();
            foreach (var domain in domains)
            {
                var organizationEventStories = new EventDomain
                {
                    DomainId = domain.Key,
                    Importance = domain.Value,
                    EventStory = EventStory
                };
                OrganizationEventStories.Add(organizationEventStories);
            }
        }
    }
}
