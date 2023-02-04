using System;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.EndOfTurn.Actions;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Game.War
{
    internal class WarActionMember
    {
        public Unit Unit { get; set; }
        public Domain Organization { get; set; }
        public int AllWarriorsBeforeWar { get; set; }
        public int WarriorsOnStart { get; set; }
        public int WarriorLosses { get; private set; }
        public enTypeOfWarrior Type { get; set; }
        public bool IsAgressor { get; set; }

        public WarActionMember()
        { }

        public WarActionMember(Unit army, int allDomainWarriors, enTypeOfWarrior type)
        {
            Unit = army;
            Organization = army.Domain;
            WarriorsOnStart = army.Warriors;
            AllWarriorsBeforeWar = allDomainWarriors;
            Type = type;
            IsAgressor = type == enTypeOfWarrior.Agressor || type == enTypeOfWarrior.AgressorSupport;
        }

        public void SetLost(double percentLosses)
        {
            WarriorLosses += (int)Math.Round(WarriorsOnStart * percentLosses);
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
