using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Commands;
using YAGO.World.Infrastructure.Database.Models.Domains;
using YAGO.World.Infrastructure.Database.Models.Turns;

namespace YAGO.World.Infrastructure.Helpers.Actions
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
