using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions
{
    public abstract class BaseAction
    {
        protected Command _command;
        protected Random _random = new Random();
        protected Turn currentTurn;

        protected abstract int ImportanceBase { get; } 
        /*
            <1000 не важно
            1000-5000 - важно
            5000+ - важно
        */

        public EventStory EventStory { get; set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; set; }


        public BaseAction(Command command, Turn currentTurn)
        {
            _command = command;
            this.currentTurn = currentTurn;
        }

        public abstract bool Execute();
    }
}
