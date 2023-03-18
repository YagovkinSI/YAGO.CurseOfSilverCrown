using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Commands;
using YSI.CurseOfSilverCrown.Core.Database.Domains;
using YSI.CurseOfSilverCrown.Core.Database.Turns;
using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.AI
{
    internal class UserAI
    {
        private ApplicationDbContext Context { get; }
        private Domain Domain { get; }
        private Turn CurrentTurn { get; }
        private AIPattern AIPattern { get; }

        public UserAI(ApplicationDbContext context, int personId, Turn currentTurn)
        {
            Context = context;
            CurrentTurn = currentTurn;
            Domain = context.Domains
                .Single(d => d.OwnerId == personId);
            AIPattern = new AIPattern(personId);
        }

        public void SetCommands()
        {
            SetParameters();

            var setUnitCommandsTask = new SetUnitCommandsTask(Context, Domain, AIPattern);
            setUnitCommandsTask.Execute();

            var setDomainCommandsTask = new SetDomainCommandsTask(Context, Domain, AIPattern);
            setDomainCommandsTask.Execute();

            SetRebelionCommand();
        }

        private void SetParameters()
        {
            var grants = Context.Commands
                .Where(c => c.Type == CommandType.GoldTransfer && c.TargetDomainId == Domain.Id)
                .ToList();

            foreach (var grant in grants)
            {
                if (Context.Domains.IsSameKingdoms(grant.Domain, Domain))
                {
                    AIPattern.Risky -= grant.Gold / 2000.0;
                    AIPattern.Peaceful += grant.Gold / 2000.0;
                    AIPattern.Loyalty += grant.Gold / 2000.0;
                }
                else
                {
                    AIPattern.Risky -= grant.Gold / 2000.0;
                    AIPattern.Peaceful -= grant.Gold / 2000.0;
                    AIPattern.Loyalty -= grant.Gold / 2000.0;
                }
            }
        }

        private void SetRebelionCommand()
        {
            if (Domain.SuzerainId == null)
                return;

            var canRebelion = Domain.TurnOfDefeat + RebelionHelper.TurnCountWithoutRebelion < CurrentTurn.Id;
            if (!canRebelion)
                return;

            var wishRebelion = CalcWishRebelion();
            if (wishRebelion)
            {
                var command = new Command
                {
                    ExecutorType = ExecutorType.Domain,
                    ExecutorId = Domain.Id,
                    DomainId = Domain.Id,
                    Type = CommandType.Rebellion,
                    Status = CommandStatus.ReadyToMove
                };
                _ = Context.Add(command);
                _ = Context.SaveChanges();
            }
        }

        private bool CalcWishRebelion()
        {
            var currentLoyality = AIPattern.GetLoyalty();
            if (currentLoyality > 0.7)
                return false;

            var allWarriors = Domain.WarriorCount;
            var powerBalance = allWarriors * 1.1 / Domain.Suzerain.WarriorCount;
            return powerBalance >= 0.8 && powerBalance - (4 * currentLoyality) > 0;
        }
    }
}
