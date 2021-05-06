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
using YSI.CurseOfSilverCrown.Core.Commands;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal class WarAction : ActionBase
    {
        private readonly ApplicationDbContext context;

        protected int ImportanceBase => 4000;

        public WarAction(ApplicationDbContext context, Turn currentTurn, Command command)
            : base(context, currentTurn, command)
        {
            this.context = context;
        }

        protected override bool Execute()
        {
            var isRebellion = Command.Organization.SuzerainId == Command.TargetOrganizationId;
            return isRebellion
                ? ExecuteRebellion()
                : ExecuteAttack();
        }

        private bool ExecuteRebellion()
        {
            var warParticipants = GetWarParticipants();

            var isVictory = CalcVictory(warParticipants);
            CalcLossesInCombats(warParticipants, isVictory);
            if (isVictory)
            {
                Command.Organization.SuzerainId = null;
                Command.Organization.Suzerain = null;
            }
            else
                warParticipants
                    .Single(p => p.Type == enTypeOfWarrior.TargetTax)
                    .SetExecuted();

            CreateEvent(warParticipants, true, isVictory);

            return true;
        }

        private bool ExecuteAttack()
        {
            var warParticipants = GetWarParticipants();

            var isVictory = CalcVictory(warParticipants);
            CalcLossesInCombats(warParticipants, isVictory); 
            if (isVictory)
            {
                Command.Target.SuzerainId = Command.OrganizationId;
                Command.Target.Suzerain = Command.Organization;

                var commandForDelete = warParticipants
                    .Where(p => p.Type == enTypeOfWarrior.TargetSupport)
                    .Select(p => p.Command)
                    .ToList();
                commandForDelete.ForEach(c => c.Type = enCommandType.ForDelete);

                Command.Type = enCommandType.WarSupportDefense;

            }

            CreateEvent(warParticipants, false, isVictory);

            return true;
        }

        private void CreateEvent(List<WarParticipant> warParticipants, bool isRebalion, bool isVictory)
        {
            var organizationsParticipants = warParticipants
                .GroupBy(p => p.Organization.Id);

            var type = isRebalion
                    ? isVictory
                        ? enEventResultType.FastRebelionSuccess
                        : enEventResultType.FastRebelionFail
                    : isVictory
                        ? enEventResultType.FastWarSuccess
                        : enEventResultType.FastWarFail;
            var eventStoryResult = new EventStoryResult(type);
            FillEventOrganizationList(eventStoryResult, organizationsParticipants);

            EventStory = new EventStory
            {
                TurnId = CurrentTurn.Id,
                EventStoryJson = eventStoryResult.ToJson()
            };

            var importance = warParticipants.Sum(p => p.WarriorLosses) * 50 + (isVictory ? 5000 : 0);
            OrganizationEventStories = new List<OrganizationEventStory>();            
            foreach (var organizationsParticipant in organizationsParticipants)
            {
                var organizationEventStory = new OrganizationEventStory
                {
                    OrganizationId = organizationsParticipant.Key,
                    Importance = importance,
                    EventStory = EventStory
                };
                OrganizationEventStories.Add(organizationEventStory);
            }
        }

        private void FillEventOrganizationList(EventStoryResult eventStoryResult, IEnumerable<IGrouping<string, WarParticipant>> organizationsParticipants)
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

        private enEventOrganizationType GetEventOrganizationType(IGrouping<string, WarParticipant> organizationsParticipant)
        {
            if (Command.OrganizationId == organizationsParticipant.Key)
                return enEventOrganizationType.Agressor;
            if (Command.TargetOrganizationId == organizationsParticipant.Key)
                return enEventOrganizationType.Defender;
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
            var targetOrganization = context.Organizations
                .Include(o => o.Commands)
                .Include(o => o.ToOrganizationCommands)
                .Include("ToOrganizationCommands.Organization")
                .Single(o => o.Id == Command.TargetOrganizationId);

            var warParticipants = new List<WarParticipant>();

            var agressorUnit = new WarParticipant(Command);
            warParticipants.Add(agressorUnit);

            var targetTaxUnit = new WarParticipant(targetOrganization);
            warParticipants.Add(targetTaxUnit);

            var targetSupportUnits = targetOrganization.ToOrganizationCommands
                .Where(c => c.Type == enCommandType.WarSupportDefense)
                .Select(c => new WarParticipant(c));
            warParticipants.AddRange(targetSupportUnits);

            return warParticipants;
        }

        private class WarParticipant
        {            
            public Command Command { get; }
            public Organization Organization { get; }
            public int AllWarriorsBeforeWar { get; }
            public int WarriorsOnStart { get; }
            public int WarriorLosses { get; private set; }
            public enTypeOfWarrior Type { get; }
            public bool IsAgressor { get; }

            public WarParticipant(Command command)
            {
                Command = command;
                Organization = command.Organization;
                WarriorsOnStart = command.Warriors;
                AllWarriorsBeforeWar = command.Organization.Warriors;
                Type = GetType(command.Type);
                IsAgressor = command.Type == enCommandType.War;
            }

            public WarParticipant(Organization organizationTarget)
            {
                Command = null;
                Organization = organizationTarget;
                WarriorsOnStart =
                    organizationTarget.Warriors -
                    organizationTarget.Commands
                        .Where(c => c.Type == enCommandType.War || c.Type == enCommandType.WarSupportDefense)
                        .Sum(c => c.Warriors);
                AllWarriorsBeforeWar = organizationTarget.Warriors;
                Type = enTypeOfWarrior.TargetTax;
                IsAgressor = false;
            }

            private enTypeOfWarrior GetType(enCommandType commandType)
            {
                switch(commandType)
                {
                    case enCommandType.War:
                        return enTypeOfWarrior.Agressor;
                    case enCommandType.WarSupportDefense:
                        return enTypeOfWarrior.TargetSupport;
                    default:
                        return enTypeOfWarrior.TargetTax;
                }
            }

            public double GetPower(int fortifications)
            {
                switch (Type)
                {
                    case enTypeOfWarrior.TargetTax:
                        return WarriorsOnStart * FortificationsHelper.GetWariorDefenseCoeficient(WarConstants.WariorDefenseTax, fortifications);
                    case enTypeOfWarrior.TargetSupport:
                        return WarriorsOnStart * FortificationsHelper.GetWariorDefenseCoeficient(WarConstants.WariorDefenseSupport, fortifications);
                    default:
                    case enTypeOfWarrior.Agressor:
                        return WarriorsOnStart;
                }
            }

            public void SetLost(double percentLosses)
            {
                WarriorLosses = (int)Math.Round(WarriorsOnStart * percentLosses);
                if (Command != null)
                    Command.Warriors -= WarriorLosses;
                Organization.Warriors -= WarriorLosses;
            }

            internal void SetExecuted()
            {
                var random = new Random();
                var executed = Math.Min(WarriorsOnStart - WarriorLosses, 10 + random.Next(10));
                WarriorLosses += executed;
                Command.Warriors -= WarriorLosses;
                Organization.Warriors -= WarriorLosses;
            }
        }

        private enum enTypeOfWarrior
        {
            Agressor = 1,

            TargetTax = 11,
            TargetSupport = 12
        }
    }
}
