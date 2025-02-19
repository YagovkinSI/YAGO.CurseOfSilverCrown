using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Database.Turns;
using YAGO.World.Host.Infrastructure.Database;

namespace YAGO.World.Host.Helpers.Actions
{
    internal abstract class DomainActionBase : ActionBase
    {
        protected Domain Domain { get; set; }

        public DomainActionBase(ApplicationDbContext context, Turn currentTurn, Domain domain)
            : base(context, currentTurn)
        {
            Domain = domain;
        }
    }
}
