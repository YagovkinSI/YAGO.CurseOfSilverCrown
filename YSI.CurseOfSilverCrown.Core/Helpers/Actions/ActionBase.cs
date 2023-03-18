using System;
using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.EventDomains;
using YSI.CurseOfSilverCrown.Core.Database.Events;
using YSI.CurseOfSilverCrown.Core.Database.Turns;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Actions
{
    public abstract class ActionBase
    {
        public ApplicationDbContext Context { get; }
        protected Turn CurrentTurn { get; }
        protected Random Random { get; } = new Random();

        private Event EventStory { get; set; }
        private List<EventObject> EventObjects { get; set; }

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
                Context.AddRange(EventObjects);
                if (this is CommandActionBase commandActionBase)
                    commandActionBase.CheckAndDeleteCommand();
            }
            return number;
        }

        public abstract bool CheckValidAction();

        protected abstract bool Execute();

        internal void CreateEventStory(EventJson eventStory, Dictionary<int, int> domains)
        {
            EventStory = new Event
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = eventStory.ToJson()
            };

            EventObjects = new List<EventObject>();
            foreach (var domain in domains)
            {
                var organizationEventStories = new EventObject
                {
                    DomainId = domain.Key,
                    Importance = domain.Value,
                    EventStory = EventStory
                };
                EventObjects.Add(organizationEventStories);
            }
        }
    }
}
