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
                IsAgressor = command.Type == enCommandType.War ||
                    command.Type == enCommandType.Rebellion ||
                    command.Type == enCommandType.WarSupportAttack;
            }

            public WarParticipant(Organization organizationTarget)
            {
                Command = null;
                Organization = organizationTarget;
                WarriorsOnStart =
                    organizationTarget.Warriors -
                    organizationTarget.Commands
                        .Where(c => c.Type == enCommandType.War || c.Type == enCommandType.Rebellion || c.Type == enCommandType.WarSupportDefense)
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
                    case enCommandType.Rebellion:
                        return enTypeOfWarrior.Agressor;
                    case enCommandType.WarSupportAttack:
                        return enTypeOfWarrior.AgressorSupport;
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
                    case enTypeOfWarrior.AgressorSupport:
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
    }
}
