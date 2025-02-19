using YAGO.World.Host.Database.Commands;
using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Database.Turns;
using YAGO.World.Host.Infrastructure.Database;

namespace YAGO.World.Host.Helpers.Actions
{
    internal abstract class CommandActionBase : ActionBase
    {
        protected Command Command { get; set; }
        protected Organization Domain { get; set; }

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
