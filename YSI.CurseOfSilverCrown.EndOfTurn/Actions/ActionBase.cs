using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    public abstract class ActionBase
    {
        public ApplicationDbContext Context { get; }
        protected Turn CurrentTurn { get; }
        protected Random Random { get; } = new Random();


        protected EventStory EventStory { get; set; }
        private List<DomainEventStory> OrganizationEventStories { get; set; }

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

        protected abstract bool CheckValidAction();

        protected abstract bool Execute();

        protected void CreateOrganizationEventStories(Dictionary<int, int> domains)
        {
            if (EventStory == null)
                throw new NullReferenceException();

            OrganizationEventStories = new List<DomainEventStory>();
            foreach (var domain in domains)
            {
                var organizationEventStories = new DomainEventStory
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
