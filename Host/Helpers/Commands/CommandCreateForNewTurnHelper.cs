using System.Linq;
using YAGO.World.Host.Database.Commands;
using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Infrastructure.Database;

namespace YAGO.World.Host.Helpers.Commands
{
    public static class CommandCreateForNewTurnHelper
    {
        public static void CreateNewCommandsForOrganizations(ApplicationDbContext context)
        {
            var domains = context.Domains.ToArray();
            foreach (var organization in domains)
            {
                CreateNewCommandsForBotOrganizations(context, organization);
            }
            context.SaveChanges();
        }

        public static void CreateNewCommandsForOrganizations(ApplicationDbContext context, Domain domain)
        {
            CreateNewCommandsForBotOrganizations(context, domain);
            context.SaveChanges();
        }

        private static void CreateNewCommandsForBotOrganizations(ApplicationDbContext context, Domain domain)
        {
            var growth = GetGrowthCommand(context, domain);
            var investments = GetInvestmentsCommand(domain);
            var fortifications = GetFortificationsCommand(domain);
            context.AddRange(growth, investments, fortifications);
        }

        private static Command GetGrowthCommand(ApplicationDbContext context, Domain domain)
        {
            return new Command
            {
                Gold = 0,
                ExecutorType = ExecutorType.Domain,
                ExecutorId = domain.Id,
                DomainId = domain.Id,
                Type = CommandType.Growth,
                Status = CommandStatus.ReadyToMove
            };
        }

        private static Command GetInvestmentsCommand(Domain domain, int? initiatorId = null)
        {
            return new Command
            {
                Gold = 0,
                ExecutorType = ExecutorType.Domain,
                ExecutorId = domain.Id,
                DomainId = domain.Id,
                Type = CommandType.Investments,
                Status = CommandStatus.ReadyToMove
            };
        }

        private static Command GetFortificationsCommand(Domain domain, int? initiatorId = null)
        {
            return new Command
            {
                Gold = 0,
                ExecutorType = ExecutorType.Domain,
                ExecutorId = domain.Id,
                DomainId = domain.Id,
                Type = CommandType.Fortifications,
                Status = CommandStatus.ReadyToMove
            };
        }
    }
}
