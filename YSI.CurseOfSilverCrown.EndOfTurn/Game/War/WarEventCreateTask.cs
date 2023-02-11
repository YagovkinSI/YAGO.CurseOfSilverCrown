using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.EndOfTurn.Actions;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Game.War
{
    internal class WarEventCreateTask
    {
        private ApplicationDbContext _context { get; }
        private WarActionParameters _warActionParameters { get; }
        public EventStoryResult EventStoryResult { get; private set; }
        public Dictionary<int, int> DommainEventStories { get; private set; }

        public WarEventCreateTask(ApplicationDbContext context, WarActionParameters warActionParameters)
        {
            _context = context;
            _warActionParameters = warActionParameters;
        }

        public void Execute()
        {
            var organizationsMembers = _warActionParameters.WarActionMembers
                .Where(m => m.IsReadyToBattle(_warActionParameters.DayOfWar) || m.WarriorLosses > 0 || m.Morality <= 0)
                .GroupBy(p => p.Organization.Id);

            var type = _warActionParameters.IsVictory
                ? enEventResultType.FastWarSuccess
                : !_warActionParameters.IsBreached
                    ? enEventResultType.SiegeFail
                    : enEventResultType.FastWarFail;
            EventStoryResult = new EventStoryResult(type);
            FillEventOrganizationList(organizationsMembers);

            var importance = 5000 + _warActionParameters.WarActionMembers.Sum(p => p.WarriorLosses) * 50 +
                (_warActionParameters.IsVictory ? 2000 : 0);

            DommainEventStories = organizationsMembers.ToDictionary(
                o => o.Key,
                o => importance);
        }


        private void FillEventOrganizationList(IEnumerable<IGrouping<int, WarActionMember>> organizationsMembers)
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
                EventStoryResult.AddEventOrganization(organizationsMember.First().Organization.Id, eventOrganizationType, temp);
            }

            if (!organizationsMembers.Any(o => GetEventOrganizationType(o) == enEventOrganizationType.Defender))
            {
                var target = _context.Domains.Find(_warActionParameters.TargetDomainId);
                var temp = new List<EventParametrChange>();
                EventStoryResult.AddEventOrganization(target.Id, enEventOrganizationType.Defender, temp);
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
                    return organizationsMember.First().Organization.Id == _warActionParameters.TargetDomainId
                        ? enEventOrganizationType.Defender
                        : enEventOrganizationType.SupporetForDefender;
            }
        }
    }
}
