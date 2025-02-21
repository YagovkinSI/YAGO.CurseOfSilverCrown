using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Domains;
using YAGO.World.Infrastructure.Database.Models.Turns;

namespace YAGO.World.Infrastructure.Helpers.Actions
{
    internal abstract class DomainActionBase : ActionBase
    {
        protected Organization Domain { get; set; }

        public DomainActionBase(ApplicationDbContext context, Turn currentTurn, Organization domain)
            : base(context, currentTurn)
        {
            Domain = domain;
        }
    }
}
