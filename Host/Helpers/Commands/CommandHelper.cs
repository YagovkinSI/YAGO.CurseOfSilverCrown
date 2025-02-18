using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using YAGO.World.Host.Database;
using YAGO.World.Host.Database.Commands;
using YAGO.World.Host.Database.Domains;

namespace YAGO.World.Host.Helpers.Commands
{
    public static class CommandHelper
    {
        public static Domain GetDomain(this Command command, ApplicationDbContext context)
        {
            var domain = command.ExecutorType switch
            {
                ExecutorType.Domain => context.Domains.Find(command.ExecutorId),
                ExecutorType.Unit => context.Units.Find(command.ExecutorId).Domain,
                _ => throw new NotImplementedException(nameof(command.ExecutorType))
            };
            return domain;
        }

        public static void CheckAndFix(ApplicationDbContext context, int domainId)
        {
            if (!context.Commands.Any(c => c.DomainId == domainId))
            {
                var domain = context.Domains.Find(domainId);
                CommandCreateForNewTurnHelper.CreateNewCommandsForOrganizations(context, domain);
            }
        }
    }
}
