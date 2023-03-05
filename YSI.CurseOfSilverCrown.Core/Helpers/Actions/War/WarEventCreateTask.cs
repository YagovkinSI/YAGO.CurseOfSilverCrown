using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Events;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Helpers.Events;
using YSI.CurseOfSilverCrown.Core.Helpers.War;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Actions.War
{
    internal class WarEventCreateTask
    {
        private ApplicationDbContext _context { get; }
        private WarActionParameters _warActionParameters { get; }
        public EventJson EventStoryResult { get; private set; }
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
                ? EventType.FastWarSuccess
                : !_warActionParameters.IsBreached
                    ? EventType.SiegeFail
                    : EventType.FastWarFail;
            EventStoryResult = new EventJson(type);
            FillEventOrganizationList(organizationsMembers);

            var importanceByLosses = _warActionParameters.WarActionMembers.Sum(p => p.WarriorLosses) * WarriorParameters.Price * 2;
            var importanceByVitory = _warActionParameters.IsVictory
                ? DomainHelper.GetImprotanceDoamin(_context, _warActionParameters.TargetDomainId)
                : 0;

            DommainEventStories = organizationsMembers.ToDictionary(
                o => o.Key,
                o => importanceByLosses + importanceByVitory);
        }


        private void FillEventOrganizationList(IEnumerable<IGrouping<int, WarActionMember>> organizationsMembers)
        {
            foreach (var organizationsMember in organizationsMembers)
            {
                var eventOrganizationType = GetEventOrganizationType(organizationsMember);
                var allWarriorsDomainOnStart = organizationsMember.First().AllWarriorsBeforeWar;
                var allWarriorsInBattleOnStart = organizationsMember.Sum(p => p.WarriorsOnStart);
                var allWarriorsLost = organizationsMember.Sum(p => p.WarriorLosses);
                var temp = new List<EventParticipantParameterChange>
                {
                    EventJsonParametrChangeHelper.Create(
                        EventParticipantParameterType.WarriorInWar, allWarriorsInBattleOnStart, allWarriorsInBattleOnStart - allWarriorsLost
                    ),
                    EventJsonParametrChangeHelper.Create(
                        EventParticipantParameterType.Warrior, allWarriorsDomainOnStart, allWarriorsDomainOnStart - allWarriorsLost
                    )
                };
                EventStoryResult.AddEventOrganization(organizationsMember.First().Organization.Id, eventOrganizationType, temp);
            }

            if (!organizationsMembers.Any(o => GetEventOrganizationType(o) == EventParticipantType.Defender))
            {
                var target = _context.Domains.Find(_warActionParameters.TargetDomainId);
                var temp = new List<EventParticipantParameterChange>();
                EventStoryResult.AddEventOrganization(target.Id, EventParticipantType.Defender, temp);
            }
        }

        private EventParticipantType GetEventOrganizationType(IGrouping<int, WarActionMember> organizationsMember)
        {
            switch (organizationsMember.First().Type)
            {
                case enTypeOfWarrior.Agressor:
                    return EventParticipantType.Agressor;
                case enTypeOfWarrior.AgressorSupport:
                    return EventParticipantType.SupporetForAgressor;
                default:
                    return organizationsMember.First().Organization.Id == _warActionParameters.TargetDomainId
                        ? EventParticipantType.Defender
                        : EventParticipantType.SupporetForDefender;
            }
        }
    }
}
