using System;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Commands;
using System.Collections.Generic;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class WarParticipant
    {
        public Unit Unit { get; set; }
        public Domain Organization { get; set; }
        public int AllWarriorsBeforeWar { get; set; }
        public int WarriorsOnStart { get; set; }
        public int WarriorLosses { get; private set; }
        public enTypeOfWarrior Type { get; set; }
        public bool IsAgressor { get; set; }

        public WarParticipant()
        { }

        public WarParticipant(Unit army, int allDomainWarriors, enTypeOfWarrior type)
        {
            Unit = army;
            Organization = army.Domain;
            WarriorsOnStart = army.Warriors;
            AllWarriorsBeforeWar = allDomainWarriors;
            Type = type;
            IsAgressor = army.Type == enArmyCommandType.War ||
                army.Type == enArmyCommandType.WarSupportAttack;
        }

        public static IEnumerable<WarParticipant> CreateWarParticipants(Domain organizationTarget)
        {
            var allDomainUnits = organizationTarget.Units
                    .Where(c => c.InitiatorDomainId == c.DomainId)
                    .Sum(u => u.Warriors);
            var notDefenseUnits = organizationTarget.Units
                    .Where(c => c.InitiatorDomainId == c.DomainId)
                    .Where(c => c.PositionDomainId == organizationTarget.Id)
                    .Where(c => c.Type != enArmyCommandType.WarSupportDefense || c.TargetDomainId != organizationTarget.Id);

            var warParticipants = new List<WarParticipant>();
            foreach (var unit in notDefenseUnits)
            {
                var warParticipant = new WarParticipant
                {
                    Unit = unit,
                    Organization = organizationTarget,
                    WarriorsOnStart = unit.Warriors,
                    AllWarriorsBeforeWar = allDomainUnits,
                    Type = enTypeOfWarrior.TargetTax,
                    IsAgressor = false
                };
                warParticipants.Add(warParticipant);
            }

            return warParticipants;
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
                case enTypeOfWarrior.AgressorSupport:
                    return WarriorsOnStart;
            }
        }

        public void SetLost(double percentLosses)
        {
            WarriorLosses = (int)Math.Round(WarriorsOnStart * percentLosses);
            Unit.Warriors -= WarriorLosses;
            if (Unit.Warriors <= 0)
            {
                Unit.Warriors = 0;
                Unit.Status = enCommandStatus.Destroyed;
            }
        }

        internal void SetExecuted()
        {
            var random = new Random();
            var executed = Math.Min(WarriorsOnStart - WarriorLosses, 10 + random.Next(10));
            WarriorLosses += executed;
            Unit.Warriors -= executed;
        }
    }
}
