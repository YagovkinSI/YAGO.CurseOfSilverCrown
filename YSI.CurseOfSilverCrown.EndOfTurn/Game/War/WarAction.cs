using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.EndOfTurn.Actions;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Game.War
{
    internal partial class WarAction : UnitActionBase
    {
        public bool IsVictory { get; protected set; }
        public int? TargetDomainId { get; private set; }

        public WarAction(ApplicationDbContext context, Turn currentTurn, int unitId)
                : base(context, currentTurn, unitId)
        {
        }

        public override bool CheckValidAction()
        {
            return Unit.Type == enArmyCommandType.War &&
                Unit.TargetDomainId != null &&
                Unit.Status == enCommandStatus.ReadyToMove &&
                RouteHelper.IsNeighbors(Context, Unit.PositionDomainId.Value, Unit.TargetDomainId.Value) &&
                !KingdomHelper.IsSameKingdoms(Context.Domains, Unit.Domain, Unit.Target);
            //TODO: Про своё королевство отдельная новость
        }

        protected override bool Execute()
        {
            TargetDomainId = Unit.TargetDomainId;

            var membersFindTask = new WarActionMembersFindTask(Context, Unit);
            var warMembers = membersFindTask.Execute();

            var battleCalcTask = new WarActionBattleCalcTask(Context, Unit, warMembers);
            var (breached, IsVictory) = battleCalcTask.Execute();

            var resultCalcTask = new WarActionResultCalcTask(Context, Unit, warMembers, CurrentTurn, IsVictory);
            resultCalcTask.Execute();

            CreateEvent(warMembers, breached, IsVictory);

            return true;
        }

        private void CreateEvent(List<WarActionMember> warMembers, bool breached, bool isVictory)
        {
            var organizationsMembers = warMembers
                .Where(p => breached || p.Type != enTypeOfWarrior.TargetSupport)
                .GroupBy(p => p.Organization.Id);

            var type = !breached
                ? enEventResultType.SiegeFail
                : isVictory
                        ? enEventResultType.FastWarSuccess
                        : enEventResultType.FastWarFail;
            var eventStoryResult = new EventStoryResult(type);
            FillEventOrganizationList(eventStoryResult, organizationsMembers);

            var importance = 5000 + warMembers.Sum(p => p.WarriorLosses) * 50 + (isVictory ? 2000 : 0);

            var dommainEventStories = organizationsMembers.ToDictionary(
                o => o.Key,
                o => importance);
            CreateEventStory(eventStoryResult, dommainEventStories);
        }

        private void FillEventOrganizationList(EventStoryResult eventStoryResult,
            IEnumerable<IGrouping<int, WarActionMember>> organizationsMembers)
        {
            foreach (var organizationsMember in organizationsMembers)
            {
                var eventOrganizationType = GetEventOrganizationType(organizationsMember);
                var allWarriorsDomainOnStart = organizationsMember.First().AllWarriorsBeforeWar;
                var allWarriorsInBattleOnStart = organizationsMember.Sum(p => p.WarriorsOnStart);
                var allWarriorsLost = organizationsMember.Sum(p => p.WarriorLosses);
                var temp = new List<EventParametrChange>
                {
                    EventParametrChangeHelper.Create(
                        enActionParameter.WarriorInWar, allWarriorsInBattleOnStart, allWarriorsInBattleOnStart - allWarriorsLost
                    ),
                    EventParametrChangeHelper.Create(
                        enActionParameter.Warrior, allWarriorsDomainOnStart, allWarriorsDomainOnStart - allWarriorsLost
                    )
                };
                eventStoryResult.AddEventOrganization(organizationsMember.First().Organization.Id, eventOrganizationType, temp);
            }

            if (!organizationsMembers.Any(o => GetEventOrganizationType(o) == enEventOrganizationType.Defender))
            {
                var target = Context.Domains.Find(TargetDomainId);
                var temp = new List<EventParametrChange>();
                eventStoryResult.AddEventOrganization(target.Id, enEventOrganizationType.Defender, temp);
            }
        }

        private enEventOrganizationType GetEventOrganizationType(IGrouping<int, WarActionMember> organizationsMember)
        {
            switch (organizationsMember.First().Type)
            {
                case enTypeOfWarrior.Agressor:
                    return enEventOrganizationType.Agressor;
                case enTypeOfWarrior.AgressorSupport:
                    return enEventOrganizationType.SupporetForAgressor;
                default:
                    return organizationsMember.First().Organization.Id == TargetDomainId
                        ? enEventOrganizationType.Defender
                        : enEventOrganizationType.SupporetForDefender;
            }
        }
    }
}
