using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Domain.Colonies.Choices
{
    public class ColonyChoices
    {
        public RulerGoal? RulerGoal { get; private set; }

        public ColonyChoices(RulerGoal? rulerGoal)
        {
            RulerGoal = rulerGoal;
        }

        internal static ColonyChoices CreateNew()
        {
            return new ColonyChoices(rulerGoal: null);
        }

        internal void SetChoice(string questCode, int value)
        {
            RulerGoal = questCode switch
            {
                GameEventConstants.MeetTheTeam => (RulerGoal)value,
                _ => throw new YagoException($"Неизвестный код квеста для выбора: {questCode}."),
            };
        }
    }
}