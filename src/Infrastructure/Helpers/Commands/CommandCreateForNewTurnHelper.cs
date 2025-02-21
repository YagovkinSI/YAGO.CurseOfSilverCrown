using System.Linq;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Commands;
using YAGO.World.Infrastructure.Database.Models.Domains;

namespace YAGO.World.Infrastructure.Helpers.Commands
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

        public static void CreateNewCommandsForOrganizations(ApplicationDbContext context, Organization domain)
        {
            CreateNewCommandsForBotOrganizations(context, domain);
            context.SaveChanges();
        }

        private static void CreateNewCommandsForBotOrganizations(ApplicationDbContext context, Organization domain)
        {
            var growth = GetGrowthCommand(context, domain);
            var investments = GetInvestmentsCommand(domain);
            var fortifications = GetFortificationsCommand(domain);
            context.AddRange(growth, investments, fortifications);
        }

        private static Command GetGrowthCommand(ApplicationDbContext context, Organization domain)
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

        private static Command GetInvestmentsCommand(Organization domain, int? initiatorId = null)
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

        private static Command GetFortificationsCommand(Organization domain, int? initiatorId = null)
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
