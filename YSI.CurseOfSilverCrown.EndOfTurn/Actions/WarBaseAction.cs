using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal abstract partial class WarBaseAction : UnitActionBase
    {
        public WarBaseAction(ApplicationDbContext context, Turn currentTurn, Unit command)
            : base(context, currentTurn, command)
        {
        }

        protected override bool Execute()
        {
            var warParticipants = GetWarParticipants();

            var isVictory = CalcVictory(warParticipants);
            CalcLossesInCombats(warParticipants, isVictory);

            SetFinalOfWar(warParticipants, isVictory);            

            CreateEvent(warParticipants, isVictory);

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
                            new EventParametrChange
                            {
                                Type = enActionParameter.WarriorInWar,
                                Before = allWarriorsInBattleOnStart,
                                After = allWarriorsInBattleOnStart - allWarriorsLost
                            },
                            new EventParametrChange
                            {
                                Type = enActionParameter.Warrior,
                                Before = allWarriorsDomainOnStart,
                                After = allWarriorsDomainOnStart - allWarriorsLost
                            }
                };
                eventStoryResult.AddEventOrganization(organizationsParticipant.First().Organization.Id, eventOrganizationType, temp);
            }
        }

        protected enEventOrganizationType GetEventOrganizationType(IGrouping<int, WarParticipant> organizationsParticipant)
        {
            if (Unit.DomainId == organizationsParticipant.Key)
                return enEventOrganizationType.Agressor;
            if (Unit.TargetDomainId == organizationsParticipant.Key)
                return enEventOrganizationType.Defender;
            if (organizationsParticipant.First().IsAgressor)
                return enEventOrganizationType.SupporetForAgressor;
            else
                return enEventOrganizationType.SupporetForDefender;
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
        }

        private bool CalcVictory(List<WarParticipant> warParticipants)
        {
            var agressotPower = warParticipants
                .Where(p => p.IsAgressor)
                .Sum(p => p.GetPower(p.Organization.Fortifications));
            var targetPower = warParticipants
                .Where(p => !p.IsAgressor)
                .Sum(p => p.GetPower(p.Organization.Fortifications));

            var agressotPowerResult = RandomHelper.AddRandom(agressotPower, 20);
            var targetPowerResult = RandomHelper.AddRandom(targetPower, 20);

            return agressotPowerResult >= targetPowerResult;
        }

        private List<WarParticipant> GetWarParticipants()
        {
            var agressorOrganization = Unit.Domain;
            var targetOrganization = Context.Domains
                .Include(o => o.Units)
                .Include(o => o.ToDomainUnits)
                .Include("ToDomainUnits.Domain")
                .Include("ToDomain2Units.Domain")
                .Single(o => o.Id == Unit.TargetDomainId);

            var warParticipants = new List<WarParticipant>();

            var allWarriors = DomainHelper.GetWarriorCount(Context, Unit.DomainId);
            var agressorUnit = new WarParticipant(Unit, allWarriors);
            warParticipants.Add(agressorUnit);

            var agressorSupportUnits = targetOrganization.ToDomainUnits
                .Where(c => c.Type == enArmyCommandType.WarSupportAttack && c.Target2DomainId == Unit.DomainId && c.Status == enCommandStatus.Complited)
                .Select(c => new WarParticipant(c, DomainHelper.GetWarriorCount(Context, c.DomainId)));
            warParticipants.AddRange(agressorSupportUnits);

            var targetTaxUnit = new WarParticipant(targetOrganization);
            warParticipants.Add(targetTaxUnit);

            var targetSupportUnits = targetOrganization.ToDomainUnits
                .Where(c => c.Type == enArmyCommandType.WarSupportDefense && c.Status == enCommandStatus.Complited)
                .Select(c => new WarParticipant(c, DomainHelper.GetWarriorCount(Context, c.DomainId)));
            warParticipants.AddRange(targetSupportUnits);

            return warParticipants;
        }
    }
}
