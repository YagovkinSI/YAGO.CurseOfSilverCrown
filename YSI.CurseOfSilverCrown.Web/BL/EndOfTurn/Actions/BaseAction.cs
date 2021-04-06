using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions
{
    public abstract class BaseAction
    {
        protected Command _command;
        protected Random _random = new Random();
        protected Turn currentTurn;

        protected abstract int ImportanceBase { get; }

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
