using YAGO.World.Domain.Colonies.Choices;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal static class RulerGoalMapping
    {
        public static string? ToEntityValue(RulerGoal? rulerGoal)
        {
            if (rulerGoal == null)
                return null;
            return rulerGoal switch
            {
                RulerGoal.Efficiency => RulerGoalConstants.Efficiency,
                RulerGoal.QualityOfLife => RulerGoalConstants.QualityOfLife,
                RulerGoal.BecomeTheBest => RulerGoalConstants.BecomeTheBest,
                RulerGoal.DontKnow => RulerGoalConstants.DontKnow,
                _ => throw new YagoException($"Неизвестное значение цели правителя: {rulerGoal}.")
            };
        }

        public static RulerGoal? ToDomainValue(string? code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;
            return code switch
            {
                RulerGoalConstants.Efficiency => RulerGoal.Efficiency,
                RulerGoalConstants.QualityOfLife => RulerGoal.QualityOfLife,
                RulerGoalConstants.BecomeTheBest => RulerGoal.BecomeTheBest,
                RulerGoalConstants.DontKnow => RulerGoal.DontKnow,
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}
