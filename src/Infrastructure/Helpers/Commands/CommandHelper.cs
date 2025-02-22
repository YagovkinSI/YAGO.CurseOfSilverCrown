using System;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Commands;
using YAGO.World.Infrastructure.Database.Models.Domains;

namespace YAGO.World.Infrastructure.Helpers.Commands
{
    public static class CommandHelper
    {
        public static Organization GetDomain(this Command command, ApplicationDbContext context)
        {
            var domain = command.ExecutorType switch
            {
                ExecutorType.Domain => context.Domains.Find(command.ExecutorId),
                ExecutorType.Unit => context.Units.Find(command.ExecutorId).Domain,
                _ => throw new NotImplementedException(nameof(command.ExecutorType))
            };
            return domain;
        }
    }
}
