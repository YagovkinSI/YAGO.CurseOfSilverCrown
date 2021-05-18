using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Interfaces;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    public abstract class ActionBase
    {
        public ApplicationDbContext Context { get; }
        protected Turn CurrentTurn { get; }
        protected ICommand Command { get; set; }
        public Domain Domain { get; set; }
        protected Random Random { get; } = new Random();


        protected EventStory EventStory { get; set; }
        protected List<DomainEventStory> OrganizationEventStories { get; set; }

        public ActionBase(ApplicationDbContext context, Turn currentTurn, ICommand command)
        {
            Command = command;
            Context = context;
            CurrentTurn = currentTurn;
        }

        public ActionBase(ApplicationDbContext context, Turn currentTurn, Domain organization)
        {
            Domain = organization;
            Context = context;
            CurrentTurn = currentTurn;
        }

        public int ExecuteAction(int number, bool removeCommandeAfterUse)
        {
            var success = Execute();
            if (success)
            {
                EventStory.Id = number;
                number++;
                Context.Add(EventStory);
                Context.AddRange(OrganizationEventStories);
                if (removeCommandeAfterUse)
                    Context.Remove(Command);
            }
            return number;
        }

        protected abstract bool Execute();
    }
}
