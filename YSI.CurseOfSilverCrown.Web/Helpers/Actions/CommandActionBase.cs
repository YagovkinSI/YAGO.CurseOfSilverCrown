using YSI.CurseOfSilverCrown.Web.Database;
using YSI.CurseOfSilverCrown.Web.Database.Commands;
using YSI.CurseOfSilverCrown.Web.Database.Domains;
using YSI.CurseOfSilverCrown.Web.Database.Turns;

namespace YSI.CurseOfSilverCrown.Web.Helpers.Actions
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
            Domain = Context.Domains.Find(command.DomainId);
        }

        protected void FixCoffersForAction()
        {
            if (Command.Gold > Domain.Gold)
                Command.Gold = Domain.Gold;
            //TODO: Имеет смысл добавить событие на изменение передаваемой суммы
        }

        public void CheckAndDeleteCommand()
        {
            if (RemoveCommandeAfterUse)
                Context.Remove(Command);
        }
    }
}
