using System;
using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Parameters;

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
            IsAgressor = type == enTypeOfWarrior.Agressor || type == enTypeOfWarrior.AgressorSupport;
        }

        public static IEnumerable<WarParticipant> GetTargetDefenseParticipants(Domain organizationTarget)
        {
            var defenseUnits = organizationTarget.UnitsHere
                .Where(c => c.Type != enArmyCommandType.CollectTax && c.Type != enArmyCommandType.ForDelete)
                .Where(c => c.Status != enCommandStatus.Retreat && c.Status != enCommandStatus.Destroyed);

            var warParticipants = new List<WarParticipant>();
            foreach (var unit in defenseUnits)
            {
                var warParticipant = new WarParticipant
                {
                    Unit = unit,
                    Organization = unit.Domain,
                    WarriorsOnStart = unit.Warriors,
                    AllWarriorsBeforeWar = unit.Domain.WarriorCount,
                    Type = enTypeOfWarrior.TargetDefense,
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
                case enTypeOfWarrior.TargetDefense:
                    return WarriorsOnStart * FortificationsHelper.GetWariorDefenseCoeficient(WarConstants.WariorDefenseSupport, fortifications);
                case enTypeOfWarrior.TargetSupport:
                    return WarriorsOnStart;
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
