using System;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.APIModels;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Commands;
using YSI.CurseOfSilverCrown.Core.Database.Commands.DomainCommands;
using YSI.CurseOfSilverCrown.Core.Database.Domains;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.AI
{
    internal class SetDomainCommandsTask
    {
        public ApplicationDbContext Context { get; }
        public Domain Domain { get; }
        public AIPattern AIPattern { get; }

        internal SetDomainCommandsTask(ApplicationDbContext context, Domain domain, AIPattern aiPattern)
        {
            Context = context;
            Domain = domain;
            AIPattern = aiPattern;
        }

        internal void Execute()
        {
            if (Domain.Coffers < CoffersParameters.StartCount * 0.2)
                return;

            var budget = new Budget(Context, Domain, Domain.PersonId);
            var notSpending = Math.Max(-(budget.Lines.Single(l => l.Type == enLineOfBudgetType.Total).Coffers.ExpectedValue.Value
                - Domain.Coffers), 0);
            var spending = Domain.Coffers - notSpending * 3;
            if (spending < CoffersParameters.StartCount * 0.2)
                return;

            ResetCommands();
            var commanfType = notSpending > 0
                ? enDomainCommandType.Investments
                : ChooseCommandType();
            var command = CreateCommand(commanfType, spending);

            Context.Add(command);
            Context.SaveChanges();
        }

        private void ResetCommands()
        {
            var commandTypesForDelete = new[] {
                enDomainCommandType.Growth,
                enDomainCommandType.Investments,
                enDomainCommandType.Fortifications,
                enDomainCommandType.GoldTransfer
            };
            var commandsForDelete = Domain.Commands
                .Where(c => commandTypesForDelete.Contains(c.Type))
                .ToList();
            Context.RemoveRange(commandsForDelete);
            Context.SaveChanges();
        }

        private enDomainCommandType ChooseCommandType()
        {
            var chooseWar = AIPattern.GetPeaceful() < 0.5;
            var chooseInvestment = AIPattern.GetRisky() > 0.5;

            return chooseInvestment
                ? enDomainCommandType.Investments
                : chooseWar
                    ? enDomainCommandType.Growth
                    : enDomainCommandType.Fortifications;
        }

        private Command CreateCommand(enDomainCommandType commanfType, int spending)
        {
            return new Command
            {
                DomainId = Domain.Id,
                Type = commanfType,
                Coffers = commanfType switch
                {
                    enDomainCommandType.Growth => (spending / 2) / 100 * 100,
                    enDomainCommandType.Investments => spending,
                    enDomainCommandType.Fortifications => spending / 2,
                    _ => spending / 2
                },
                InitiatorPersonId = Domain.PersonId,
                Status = enCommandStatus.ReadyToMove
            };
        }
    }
}
