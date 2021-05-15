using System;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Parameters;
using YSI.CurseOfSilverCrown.Core.Commands;

namespace YSI.CurseOfSilverCrown.Core.Actions
{
    internal abstract partial class WarBaseAction
    {
        internal class WarParticipant
        {            
            public Unit Unit { get; }
            public Domain Organization { get; }
            public int AllWarriorsBeforeWar { get; }
            public int WarriorsOnStart { get; }
            public int WarriorLosses { get; private set; }
            public enTypeOfWarrior Type { get; }
            public bool IsAgressor { get; }

            public WarParticipant(Unit army)
            {
                Unit = army;
                Organization = army.Domain;
                WarriorsOnStart = army.Warriors;
                AllWarriorsBeforeWar = army.Domain.Warriors;
                Type = GetType(army.Type);
                IsAgressor = army.Type == enArmyCommandType.War ||
                    army.Type == enArmyCommandType.Rebellion ||
                    army.Type == enArmyCommandType.WarSupportAttack;
            }

            public WarParticipant(Domain organizationTarget)
            {
                Unit = null;
                Organization = organizationTarget;
                WarriorsOnStart =
                    organizationTarget.Warriors -
                    organizationTarget.Units
                        .Where(c => c.Type == enArmyCommandType.War || c.Type == enArmyCommandType.Rebellion || c.Type == enArmyCommandType.WarSupportDefense)
                        .Sum(c => c.Warriors);
                AllWarriorsBeforeWar = organizationTarget.Warriors;
                Type = enTypeOfWarrior.TargetTax;
                IsAgressor = false;
            }

            private enTypeOfWarrior GetType(enArmyCommandType commandType)
            {
                switch(commandType)
                {
                    case enArmyCommandType.War:
                    case enArmyCommandType.Rebellion:
                        return enTypeOfWarrior.Agressor;
                    case enArmyCommandType.WarSupportAttack:
                        return enTypeOfWarrior.AgressorSupport;
                    case enArmyCommandType.WarSupportDefense:
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
                    case enTypeOfWarrior.AgressorSupport:
                        return WarriorsOnStart;
                }
            }

            public void SetLost(double percentLosses)
            {
                WarriorLosses = (int)Math.Round(WarriorsOnStart * percentLosses);
                if (Unit != null)
                    Unit.Warriors -= WarriorLosses;
                Organization.Warriors -= WarriorLosses;
            }

            internal void SetExecuted()
            {
                var random = new Random();
                var executed = Math.Min(WarriorsOnStart - WarriorLosses, 10 + random.Next(10));
                WarriorLosses += executed;
                Unit.Warriors -= executed;
                Organization.Warriors -= executed;
            }
        }
    }
}
