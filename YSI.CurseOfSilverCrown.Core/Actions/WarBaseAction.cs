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
using YSI.CurseOfSilverCrown.Core.Event;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal abstract partial class WarBaseAction : ActionBase
    {
        public WarBaseAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
        }

        protected override bool Execute()
        {
            if (!IsValidAttack())
                return false;

            var warParticipants = GetWarParticipants();

            var isVictory = CalcVictory(warParticipants);
            CalcLossesInCombats(warParticipants, isVictory);

            SetFinalOfWar(warParticipants, isVictory);            

            CreateEvent(warParticipants, isVictory);

            return true;
        }

        protected abstract void SetFinalOfWar(List<WarParticipant> warParticipants, bool isVictory);
        protected abstract bool IsValidAttack();

        protected abstract void CreateEvent(List<WarParticipant> warParticipants, bool isVictory);

        protected void FillEventOrganizationList(EventStoryResult eventStoryResult, IEnumerable<IGrouping<string, WarParticipant>> organizationsParticipants)
        {
            foreach (var organizationsParticipant in organizationsParticipants)
            {
                var eventOrganizationType = GetEventOrganizationType(organizationsParticipant);
                var temp = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enActionParameter.WarriorInWar,
                                Before = organizationsParticipant.Sum(p => p.WarriorsOnStart),
                                After = organizationsParticipant.Sum(p => p.WarriorsOnStart - p.WarriorLosses)
                            },
                            new EventParametrChange
                            {
                                Type = enActionParameter.Warrior,
                                Before = organizationsParticipant.First().AllWarriorsBeforeWar,
                                After = organizationsParticipant.First().Organization.Warriors
                            }
                };
                eventStoryResult.AddEventOrganization(organizationsParticipant.First().Organization, eventOrganizationType, temp);
            }
        }

        protected enEventOrganizationType GetEventOrganizationType(IGrouping<string, WarParticipant> organizationsParticipant)
        {
            if (Command.OrganizationId == organizationsParticipant.Key)
                return enEventOrganizationType.Agressor;
            if (Command.TargetOrganizationId == organizationsParticipant.Key)
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
            var agressorOrganization = Command.Organization;
            var targetOrganization = Context.Organizations
                .Include(o => o.Commands)
                .Include(o => o.ToOrganizationCommands)
                .Include("ToOrganizationCommands.Organization")
                .Include("ToOrganization2Commands.Organization")
                .Single(o => o.Id == Command.TargetOrganizationId);

            var warParticipants = new List<WarParticipant>();

            var agressorUnit = new WarParticipant(Command);
            warParticipants.Add(agressorUnit);

            var agressorSupportUnits = targetOrganization.ToOrganizationCommands
                .Where(c => c.Type == enCommandType.WarSupportAttack && c.Target2OrganizationId == Command.OrganizationId)
                .Select(c => new WarParticipant(c));
            warParticipants.AddRange(agressorSupportUnits);

            var targetTaxUnit = new WarParticipant(targetOrganization);
            warParticipants.Add(targetTaxUnit);

            var targetSupportUnits = targetOrganization.ToOrganizationCommands
                .Where(c => c.Type == enCommandType.WarSupportDefense)
                .Select(c => new WarParticipant(c));
            warParticipants.AddRange(targetSupportUnits);

            return warParticipants;
        }
    }
}
