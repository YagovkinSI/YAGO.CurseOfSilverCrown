using YSI.CurseOfSilverCrown.Web.Database;
using YSI.CurseOfSilverCrown.Web.Database.Domains;
using YSI.CurseOfSilverCrown.Web.Database.Turns;

namespace YSI.CurseOfSilverCrown.Web.Helpers.Actions
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
