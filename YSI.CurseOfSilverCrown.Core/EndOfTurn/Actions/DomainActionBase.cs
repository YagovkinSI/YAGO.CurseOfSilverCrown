using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.MainModels;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
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
