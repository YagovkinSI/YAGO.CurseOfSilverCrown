using System;
using YAGO.World.Host.Database.Commands;
using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Database.Units;
using YAGO.World.Host.Helpers.War;

namespace YAGO.World.Host.Helpers.Actions.War
{
    internal class WarActionMember
    {
        public Unit Unit { get; set; }
        public Domain Organization { get; set; }
        public int AllWarriorsBeforeWar { get; set; }
        public int WarriorsOnStart { get; set; }
        public int WarriorLosses { get; private set; }
        public enTypeOfWarrior Type { get; set; }
        public int DistanceToCastle { get; set; }
        public int Morality { get; set; }
        public bool IsAgressor { get; set; }

        public WarActionMember(Unit army, int allDomainWarriors, enTypeOfWarrior type, int distanceToCastle, int morality)
        {
            Unit = army;
            Organization = army.Domain;
            WarriorsOnStart = army.Warriors;
            AllWarriorsBeforeWar = allDomainWarriors;
            Type = type;
            IsAgressor = type == enTypeOfWarrior.Agressor || type == enTypeOfWarrior.AgressorSupport;
            DistanceToCastle = distanceToCastle;
            Morality = morality;
        }

        public void SetLost(double percentLosses)
        {
            var currentWarriorsCount = WarriorsOnStart - WarriorLosses;
            var currenLosses = (int)Math.Round(currentWarriorsCount * percentLosses);
            WarriorLosses += currenLosses;
            Unit.Warriors -= currenLosses;
            if (Unit.Warriors <= 0)
            {
                Unit.Warriors = 0;
                Unit.Status = CommandStatus.Destroyed;
            }
        }

        internal void SetExecuted()
        {
            var random = new Random();
            var executed = Math.Min(WarriorsOnStart - WarriorLosses, 10 + random.Next(10));
            WarriorLosses += executed;
            Unit.Warriors -= executed;
        }

        internal bool IsReadyToBattle(int dayOfWar)
        {
            if (Morality <= 0)
                return false;
            if (Unit.Status == CommandStatus.Destroyed || Unit.Status == CommandStatus.Retreat)
                return false;
            if (WarriorsOnStart <= WarriorLosses)
                return false;
            if (DistanceToCastle > dayOfWar / 7)
                return false;

            return true;
        }
    }
}
