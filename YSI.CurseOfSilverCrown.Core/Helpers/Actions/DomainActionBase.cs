using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Domains;
using YSI.CurseOfSilverCrown.Core.Database.Turns;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Actions
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
