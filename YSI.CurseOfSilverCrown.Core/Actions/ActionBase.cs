using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    public abstract class ActionBase
    {
        public ApplicationDbContext Context { get; }
        protected Turn CurrentTurn { get; }
        protected Command Command { get; set; }
        public Organization Organization { get; set; }
        protected Random Random { get; } = new Random();

        public ActionBase(ApplicationDbContext context, Turn currentTurn, Command command)
        {
            Command = command;
            Context = context;
            CurrentTurn = currentTurn;
        }

        public ActionBase(ApplicationDbContext context, Turn currentTurn, Organization organization)
        {
            Organization = organization;
            Context = context;
            CurrentTurn = currentTurn;
        }

        public abstract bool Execute();



    }
}
