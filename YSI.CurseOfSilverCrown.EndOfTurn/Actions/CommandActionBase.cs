using Microsoft.EntityFrameworkCore;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal abstract class CommandActionBase : ActionBase
    {
        protected Command Command { get; set; }
        protected Domain Domain { get; set; }

        protected abstract bool RemoveCommandeAfterUse { get; }

        public CommandActionBase(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn)
        {
            Command = command;
            Domain = Context.Domains
                .Include(d => d.Units)
                .Include(d => d.Suzerain)
                .Include(d => d.Vassals)
                .SingleAsync(d => d.Id == command.DomainId)
                .Result;
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
