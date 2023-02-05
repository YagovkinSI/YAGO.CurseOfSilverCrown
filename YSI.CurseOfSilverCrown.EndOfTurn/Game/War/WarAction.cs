using System;
using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.EndOfTurn.Actions;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;

//TODO: Big File

namespace YSI.CurseOfSilverCrown.EndOfTurn.Game.War
{
    internal partial class WarAction : UnitActionBase
    {
        public bool IsBreached { get; protected set; }
        public bool IsVictory => _warActionStage == enWarActionStage.AgressorWin;
        public int? TargetDomainId { get; private set; }
        public List<WarActionMember> WarActionMembers { get; set; }

        private enWarActionStage _warActionStage = enWarActionStage.Siege;
        private double _currentFortifications;
        private int _dayOfWar = 0;


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
        }

        protected override bool Execute()
        {
            Prepare();

            while (_warActionStage != enWarActionStage.AgressorWin && _warActionStage != enWarActionStage.DefenderWin)
            {
                var (defebderCount, agressorCount) = WarriorsCheck();
                RetreatCheck(defebderCount, agressorCount);
                CalcWarActionStage();
            }
            CalsWarResult();

            CreateEvent(WarActionMembers, IsBreached);

            return true;
        }

        private void CalsWarResult()
        {
            var resultCalcTask = new WarActionResultCalcTask(Context, Unit, WarActionMembers, CurrentTurn, IsVictory);
            resultCalcTask.Execute();
        }

        private void Prepare()
        {
            TargetDomainId = Unit.TargetDomainId;

            var membersFindTask = new WarActionMembersFindTask(Context, Unit);
            WarActionMembers = membersFindTask.Execute();

            _currentFortifications = Unit.Target.Fortifications;
        }

        private (int, int) WarriorsCheck()
        {
            var defendersInCastle = GetWarriorCount(enTypeOfWarrior.TargetDefense);
            var defendersWithSupport = GetWarriorCount(enTypeOfWarrior.TargetDefense, enTypeOfWarrior.TargetSupport);
            var warriorsReadyToAttack = GetWarriorCount(enTypeOfWarrior.Agressor, enTypeOfWarrior.AgressorSupport);
            if (defendersInCastle == 0 || defendersWithSupport > 1.2 * warriorsReadyToAttack)
                _warActionStage = enWarActionStage.Battle;

            if (defendersWithSupport <= 0)
            {
                _warActionStage = enWarActionStage.AgressorWin;
            }

            if (warriorsReadyToAttack <= 0)
            {
                _warActionStage = enWarActionStage.DefenderWin;
            }

            return (defendersWithSupport, warriorsReadyToAttack);
        }

        private void RetreatCheck(int warriorsReadyToDefense, int warriorsReadyToAttack)
        {
            if (warriorsReadyToDefense == 0 || warriorsReadyToAttack == 0)
                return;

            RecalcMorality(warriorsReadyToDefense, warriorsReadyToAttack);

            WarriorsCheck();
        }

        private void RecalcMorality(int warriorsReadyToDefense, int warriorsReadyToAttack)
        {
            foreach (var member in WarActionMembers)
            {
                if (!member.IsReadyToBattle(_dayOfWar))
                    continue;

                var moralityDelta = _warActionStage switch
                {
                    enWarActionStage.Siege => GetMoralityDeltaForSiege(member, warriorsReadyToAttack, warriorsReadyToDefense),
                    enWarActionStage.Assault => GetMoralityDeltaForAssault(member, warriorsReadyToAttack, warriorsReadyToDefense),
                    enWarActionStage.Battle => GetMoralityDeltaForBattle(member, warriorsReadyToAttack, warriorsReadyToDefense)
                };

                member.Morality += moralityDelta;
                member.Morality = Math.Min(Math.Max(member.Morality, 0), 100);
            }
        }

        private int GetMoralityDeltaForSiege(WarActionMember member, int warriorsReadyToAttack, int warriorsReadyToDefense)
        {
            var ratio = member.IsAgressor
                ? (double)warriorsReadyToAttack / warriorsReadyToDefense
                : (double)warriorsReadyToDefense / warriorsReadyToAttack;
            var deltaDefault = (ratio - 1) * 100;

            if (member.IsAgressor)
            {
                return deltaDefault > 0
                    ? -2
                    : (int)deltaDefault * 2;
            }
            else
            {
                return deltaDefault > 0
                    ? (int)deltaDefault
                    : -4;
            }
        }

        private int GetMoralityDeltaForAssault(WarActionMember member, int warriorsReadyToAttack, int warriorsReadyToDefense)
        {
            var ratio = member.IsAgressor
                ? (double)warriorsReadyToAttack / warriorsReadyToDefense
                : (double)warriorsReadyToDefense / warriorsReadyToAttack;
            var deltaDefault = (ratio - 1) * 100;
            return (int)deltaDefault / 5;
        }

        private int GetMoralityDeltaForBattle(WarActionMember member, int warriorsReadyToAttack, int warriorsReadyToDefense)
        {
            var ratio = member.IsAgressor
                ? (double)warriorsReadyToAttack / warriorsReadyToDefense
                : (double)warriorsReadyToDefense / warriorsReadyToAttack;
            var deltaDefault = (ratio - 1) * 100;
            return (int)deltaDefault;
        }

        private void CalcWarActionStage()
        {
            switch (_warActionStage)
            {
                case enWarActionStage.Siege:
                    RunSiegeStage();
                    break;
                case enWarActionStage.Assault:
                    RunAssaultStage();
                    break;
                case enWarActionStage.Battle:
                    RunBattleStage();
                    break;
                case enWarActionStage.AgressorWin:
                case enWarActionStage.DefenderWin:
                    break;
                default:
                    throw new NotImplementedException($"Неизвестный тип {_warActionStage}");
            }
        }

        //TODO: Big method
        private void RunSiegeStage()
        {
            var (_, agressorCount) = WarriorsCheck();
            var defendersInCastle = GetWarriorCount(enTypeOfWarrior.TargetDefense);
            if (defendersInCastle == 0 || agressorCount == 0)
                return;

            var siegeRoll = RandomHelper.Random2d6();
            if (siegeRoll == 0)
            {
                var percent = (int)Math.Ceiling(new Random().NextDouble() * 5);
                AddLosses(true, percent, enTypeOfWarrior.Agressor, enTypeOfWarrior.AgressorSupport);
                AddLosses(false, new Random().Next(0, 10), enTypeOfWarrior.TargetDefense);
            }
            else if (siegeRoll == 12)
            {
                _currentFortifications = 0;
                AddLosses(false, new Random().Next(0, 50), enTypeOfWarrior.Agressor, enTypeOfWarrior.AgressorSupport);
                AddLosses(false, new Random().Next(0, 50), enTypeOfWarrior.TargetDefense);
            }
            else
            {
                _currentFortifications -= agressorCount * siegeRoll / 3.5;
                AddLosses(false, new Random().Next(0, 30), enTypeOfWarrior.Agressor, enTypeOfWarrior.AgressorSupport);
                AddLosses(false, new Random().Next(0, 10), enTypeOfWarrior.TargetDefense);
            }

            if (_currentFortifications <= 0)
            {
                _dayOfWar += new Random().Next(1, 7);
                _warActionStage = enWarActionStage.Assault;
                IsBreached = true;
            }
            else
            {
                _currentFortifications /= 2;
                _dayOfWar += 7;
            }
        }

        private int GetWarriorCount(params enTypeOfWarrior[] types)
        {
            return WarActionMembers
                .Where(m => types.Contains(m.Type))
                .Where(m => m.IsReadyToBattle(_dayOfWar))
                .Sum(m => m.WarriorsOnStart - m.WarriorLosses);
        }

        private void AddLosses(bool isPercent, int value, params enTypeOfWarrior[] types)
        {
            var members = WarActionMembers
                .Where(m => types.Contains(m.Type))
                .Where(m => m.IsReadyToBattle(_dayOfWar));
            if (!members.Any())
                return;
            var percent = isPercent
                ? value / 100.0
                : Math.Min(1, (double)value / members.Sum(m => m.WarriorsOnStart - m.WarriorLosses));
            foreach (var member in members)
            {
                member.SetLost(percent);
            }
        }

        //TODO: Big method
        private void RunAssaultStage()
        {
            var (defendersCount, agressorCount) = WarriorsCheck();
            if (defendersCount == 0 || agressorCount == 0)
                return;

            var defendersInCastle = GetWarriorCount(enTypeOfWarrior.TargetDefense);
            var defendersOutCastle = defendersCount - defendersInCastle;

            if (defendersInCastle != 0)
            {
                var agressorLosses = Math.Min(defendersCount, 200) * 0.3 * RandomHelper.Random2d6() / 7.0;
                var inCastleLosses = Math.Min(agressorCount, 200) * 0.15 * RandomHelper.Random2d6() / 7.0;
                AddLosses(false, (int)agressorLosses, enTypeOfWarrior.Agressor, enTypeOfWarrior.AgressorSupport);
                AddLosses(false, (int)inCastleLosses, enTypeOfWarrior.TargetDefense);
            }

            if (defendersOutCastle != 0)
            {
                var countInBattle = Math.Max(defendersCount, agressorCount) / 10;
                var agressorLosses = Math.Min(defendersCount, countInBattle) * 0.25 * RandomHelper.Random2d6() / 7;
                var outCastleLosses = Math.Min(agressorCount, countInBattle) * 0.3 * RandomHelper.Random2d6() / 7;
                AddLosses(false, (int)agressorLosses, enTypeOfWarrior.Agressor, enTypeOfWarrior.AgressorSupport);
                AddLosses(false, (int)outCastleLosses, enTypeOfWarrior.TargetSupport);
            }

            var assaultRoll = RandomHelper.Random2d6();
            if (assaultRoll > 8)
                _warActionStage = enWarActionStage.Battle;
            _dayOfWar += new Random().Next(0, 2);
        }

        private void RunBattleStage()
        {
            var (defendersCount, agressorCount) = WarriorsCheck();
            if (defendersCount == 0 || agressorCount == 0)
                return;

            var countInBattle = Math.Max(defendersCount, agressorCount) / 10;
            var agressorLosses = Math.Min(defendersCount, countInBattle) * 0.3 * RandomHelper.Random2d6() / 7;
            var defendersLosses = Math.Min(agressorCount, countInBattle) * 0.3 * RandomHelper.Random2d6() / 7;
            AddLosses(false, (int)agressorLosses, enTypeOfWarrior.Agressor, enTypeOfWarrior.AgressorSupport);
            AddLosses(false, (int)defendersLosses, enTypeOfWarrior.TargetDefense, enTypeOfWarrior.TargetDefense);
            _dayOfWar += new Random().Next(0, 2);
        }

        private void CreateEvent(List<WarActionMember> warMembers, bool breached)
        {
            var organizationsMembers = warMembers
                .Where(m => m.IsReadyToBattle(_dayOfWar) || m.WarriorLosses > 0 || m.Morality <= 0)
                .GroupBy(p => p.Organization.Id);

            var type = IsVictory
                ? enEventResultType.FastWarSuccess
                : !breached
                    ? enEventResultType.SiegeFail
                    : enEventResultType.FastWarFail;
            var eventStoryResult = new EventStoryResult(type);
            FillEventOrganizationList(eventStoryResult, organizationsMembers);

            var importance = 5000 + warMembers.Sum(p => p.WarriorLosses) * 50 + (IsVictory ? 2000 : 0);

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
