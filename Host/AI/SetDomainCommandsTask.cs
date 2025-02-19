using System;
using System.Linq;
using YAGO.World.Host.APIModels.BudgetModels;
using YAGO.World.Host.Database.Commands;
using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Infrastructure.Database;
using YAGO.World.Host.Parameters;

namespace YAGO.World.Host.AI
{
    internal class SetDomainCommandsTask
    {
        public ApplicationDbContext Context { get; }
        public Organization Domain { get; }
        public AIPattern AIPattern { get; }

        internal SetDomainCommandsTask(ApplicationDbContext context, Organization domain, AIPattern aiPattern)
        {
            Context = context;
            Domain = domain;
            AIPattern = aiPattern;
        }

        internal void Execute()
        {
            if (Domain.Gold < CoffersParameters.StartCount * 0.2)
                return;

            var budget = new Budget(Context, Domain);
            var notSpending = Math.Max(-(budget.Lines.Single(l => l.Type == BudgetLineType.Total).Coffers.ExpectedValue.Value
                - Domain.Gold), 0);
            var spending = Domain.Gold - notSpending * 3;
            if (spending < CoffersParameters.StartCount * 0.2)
                return;

            ResetCommands();
            var commanfType = notSpending > 0
                ? CommandType.Investments
                : ChooseCommandType();
            var command = CreateCommand(commanfType, spending);

            _ = Context.Add(command);
            _ = Context.SaveChanges();
        }

        private void ResetCommands()
        {
            var commandTypesForDelete = new[] {
                CommandType.Growth,
                CommandType.Investments,
                CommandType.Fortifications,
                CommandType.GoldTransfer
            };
            var commandsForDelete = Domain.Commands
                .Where(c => commandTypesForDelete.Contains(c.Type))
                .ToList();
            Context.RemoveRange(commandsForDelete);
            _ = Context.SaveChanges();
        }

        private CommandType ChooseCommandType()
        {
            var chooseWar = AIPattern.GetPeaceful() < 0.5;
            var chooseInvestment = AIPattern.GetRisky() > 0.5;

            return chooseInvestment
                ? CommandType.Investments
                : chooseWar
                    ? CommandType.Growth
                    : CommandType.Fortifications;
        }

        private Command CreateCommand(CommandType commanfType, int spending)
        {
            return new Command
            {
                ExecutorType = ExecutorType.Domain,
                ExecutorId = Domain.Id,
                DomainId = Domain.Id,
                Type = commanfType,
                Gold = commanfType switch
                {
                    CommandType.Growth => spending / 2 / 100 * 100,
                    CommandType.Investments => spending,
                    CommandType.Fortifications => spending / 2,
                    _ => spending / 2
                },
                Status = CommandStatus.ReadyToMove
            };
        }
    }
}
