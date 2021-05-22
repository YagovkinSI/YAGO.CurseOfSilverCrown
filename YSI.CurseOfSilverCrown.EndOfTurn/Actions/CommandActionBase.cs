using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.BL.Models;
using YSI.CurseOfSilverCrown.Core.BL.Models.Min;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal abstract class CommandActionBase : ActionBase
    {
        protected Command Command { get; set; }
        protected DomainMin Domain { get; set; }

        protected abstract bool RemoveCommandeAfterUse { get; }

        public CommandActionBase(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn)
        {
            Command = command;
            Domain = Context.GetDomainMin(command.DomainId).Result;
        }

        protected void FixCoffersForAction()
        {
            if (Command.Coffers > Domain.Coffers)
                Command.Coffers = Domain.Coffers;
            //TODO: Имеет смысл добавить событие на изменение передаваемой суммы
        }

        public void CheckAndDeleteCommand()
        {
            if (RemoveCommandeAfterUse)
                Context.Remove(Command);
        }
    }
}
