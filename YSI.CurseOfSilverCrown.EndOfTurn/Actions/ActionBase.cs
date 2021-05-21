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
        protected List<DomainEventStory> OrganizationEventStories { get; set; }

        public ActionBase(ApplicationDbContext context, Turn currentTurn)
        {
            Context = context;
            CurrentTurn = currentTurn;
        }

        public int ExecuteAction(int number)
        {
            var success = Execute();
            if (success)
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

        protected abstract bool Execute();
    }
}
