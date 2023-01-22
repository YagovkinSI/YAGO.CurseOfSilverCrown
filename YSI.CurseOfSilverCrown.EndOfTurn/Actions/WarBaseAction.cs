using System;
using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal abstract partial class WarBaseAction : UnitActionBase
    {
        public bool IsVictory { get; protected set; }
        public int? TargetDomainId { get; private set; }

        public WarBaseAction(ApplicationDbContext context, Turn currentTurn, int unitId)
            : base(context, currentTurn, unitId)
        {
        }

        protected override bool Execute()
        {
            TargetDomainId = Unit.TargetDomainId;
            var warParticipants = GetWarParticipants();

            IsVictory = CalcVictory(warParticipants);
            CalcLossesInCombats(warParticipants, IsVictory);

            SetFinalOfWar(warParticipants, IsVictory);

            CreateEvent(warParticipants, IsVictory);

            return true;
        }

        protected abstract void SetFinalOfWar(List<WarParticipant> warParticipants, bool isVictory);

        protected abstract void CreateEvent(List<WarParticipant> warParticipants, bool isVictory);

        protected void FillEventOrganizationList(EventStoryResult eventStoryResult, IEnumerable<IGrouping<int, WarParticipant>> organizationsParticipants)
        {
            foreach (var organizationsParticipant in organizationsParticipants)
            {
                var eventOrganizationType = GetEventOrganizationType(organizationsParticipant);
                var allWarriorsDomainOnStart = organizationsParticipant.First().AllWarriorsBeforeWar;
                var allWarriorsInBattleOnStart = organizationsParticipant.Sum(p => p.WarriorsOnStart);
                var allWarriorsLost = organizationsParticipant.Sum(p => p.WarriorLosses);
                var temp = new List<EventParametrChange>
                {
                    EventParametrChangeHelper.Create(
                        enActionParameter.WarriorInWar, allWarriorsInBattleOnStart, allWarriorsInBattleOnStart - allWarriorsLost
                    ),
                    EventParametrChangeHelper.Create(
                        enActionParameter.Warrior, allWarriorsDomainOnStart, allWarriorsDomainOnStart - allWarriorsLost
                    )
                };
                eventStoryResult.AddEventOrganization(organizationsParticipant.First().Organization.Id, eventOrganizationType, temp);
            }

            if (!organizationsParticipants.Any(o => GetEventOrganizationType(o) == enEventOrganizationType.Defender))
            {
                var target = Context.Domains.Find(TargetDomainId);
                var temp = new List<EventParametrChange>();
                eventStoryResult.AddEventOrganization(target.Id, enEventOrganizationType.Defender, temp);
            }
        }

        protected enEventOrganizationType GetEventOrganizationType(IGrouping<int, WarParticipant> organizationsParticipant)
        {
            switch (organizationsParticipant.First().Type)
            {
                case enTypeOfWarrior.Agressor:
                    return enEventOrganizationType.Agressor;
                case enTypeOfWarrior.AgressorSupport:
                    return enEventOrganizationType.SupporetForAgressor;
                default:
                    return organizationsParticipant.First().Organization.Id == TargetDomainId
                        ? enEventOrganizationType.Defender
                        : enEventOrganizationType.SupporetForDefender;
            }
        }

        private void CalcLossesInCombats(List<WarParticipant> warParticipants, bool isVictory)
        {
            var agressotWarriorsCount = warParticipants
                .Where(p => p.IsAgressor)
                .Sum(p => p.WarriorsOnStart);
            var targetWarriorsCount = warParticipants
                .Where(p => !p.IsAgressor)
                .Sum(p => p.WarriorsOnStart);

            var random = new Random();
            var agressorLossesPercentDefault = WarConstants.AgressorLost +
                random.NextDouble() / 20 +
                (isVictory ? 0 : 0.05 + random.NextDouble() / 20);
            var targetLossesPercentDefault = WarConstants.TargetLost +
                random.NextDouble() / 20 +
                (!isVictory ? 0 : 0.05 + random.NextDouble() / 20);
            var agressorLossesPercent = agressotWarriorsCount <= targetWarriorsCount
                ? agressorLossesPercentDefault
                : agressorLossesPercentDefault * ((double)targetWarriorsCount / agressotWarriorsCount);
            var targetLossesPercent = agressotWarriorsCount >= targetWarriorsCount
                ? targetLossesPercentDefault
                : targetLossesPercentDefault * ((double)agressotWarriorsCount / targetWarriorsCount);

            warParticipants.ForEach(p => p.SetLost(p.IsAgressor ? agressorLossesPercent : targetLossesPercent));

            Context.UpdateRange(warParticipants.Select(p => p.Unit));
        }

        private bool CalcVictory(List<WarParticipant> warParticipants)
        {
            var targetDomain = Context.Domains.Find(Unit.TargetDomainId);

            var agressotPower = warParticipants
                .Where(p => p.IsAgressor)
                .Sum(p => p.WarriorsOnStart);
            var targetPower = warParticipants
                .Where(p => !p.IsAgressor)
                .Sum(p => p.GetPower(p.Organization.Fortifications))
                + WarConstants.DefaultDefenseWarrioirs *
                    FortificationsHelper.GetWariorDefenseCoeficient(WarConstants.WariorDefenseSupport, targetDomain.Fortifications);

            var agressotPowerResult = RandomHelper.AddRandom(agressotPower, 20);
            var targetPowerResult = RandomHelper.AddRandom(targetPower, 20);

            return agressotPowerResult >= targetPowerResult;
        }

        private List<WarParticipant> GetWarParticipants()
        {
            var agressorDomain = Unit.Domain;
            var targetDomain = Context.Domains.Find(Unit.TargetDomainId);

            var warParticipants = new List<WarParticipant>();

            var agressorParticipant = GetAgressorParticipant();
            warParticipants.Add(agressorParticipant);

            var agressorSupportParticipants = GetAgressorSupportParticipants(targetDomain);
            warParticipants.AddRange(agressorSupportParticipants);

            var targetDefenseParticipants = GetTargetDefenseParticipants(targetDomain);
            warParticipants.AddRange(targetDefenseParticipants);

            var targetDefenseSupportParticipants = GetTargetDefenseSupportParticipants(targetDomain);
            warParticipants.AddRange(targetDefenseSupportParticipants);

            return warParticipants;
        }

        private IEnumerable<WarParticipant> GetTargetDefenseSupportParticipants(Domain targetDomain)
        {
            var supportDefenseDomain = targetDomain.Suzerain ?? targetDomain;
            var unitsForSupport = supportDefenseDomain.Units
                .Where(u => u.Type != enArmyCommandType.ForDelete && u.Type != enArmyCommandType.CollectTax)
                .Where(c => c.Status != enCommandStatus.Retreat && c.Status != enCommandStatus.Destroyed)
                .Where(u => u.PositionDomainId != targetDomain.Id);

            var warParticipants = new List<WarParticipant>();
            foreach (var unit in unitsForSupport)
            {
                var newPosition = RouteHelper.GetNextPosition(Context,
                    unit.DomainId, unit.PositionDomainId.Value, targetDomain.Id, true);
                if (newPosition != unit.PositionDomainId.Value)
                {
                    var participant = new WarParticipant(unit, DomainHelper.GetWarriorCount(Context, unit.DomainId),
                        enTypeOfWarrior.TargetSupport);
                    warParticipants.Add(participant);
                }
            }

            return warParticipants;
        }

        private IEnumerable<WarParticipant> GetTargetDefenseParticipants(Domain targetDomain)
        {
            return WarParticipant.GetTargetDefenseParticipants(targetDomain);
        }

        private IEnumerable<WarParticipant> GetAgressorSupportParticipants(Domain targetDomain)
        {
            return targetDomain.ToDomainUnits
                .Where(c => c.Type == enArmyCommandType.WarSupportAttack &&
                    c.Target2DomainId == Unit.DomainId &&
                    c.Status == enCommandStatus.Complited)
                .Select(c => new WarParticipant(c, DomainHelper.GetWarriorCount(Context, c.DomainId),
                    enTypeOfWarrior.AgressorSupport));
        }

        private WarParticipant GetAgressorParticipant()
        {
            var allWarriors = DomainHelper.GetWarriorCount(Context, Unit.DomainId);
            return new WarParticipant(Unit, allWarriors, enTypeOfWarrior.Agressor);
        }
    }
}
