using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Domain.GameActions
{
    public class GameActionResult
    {
        public string Title { get; }
        public string? ImageName { get; }
        public string[] Text { get; }

        public IReadOnlyList<KeyValuePair<GameParameterType, double[]>> MainParametersResult { get; private set; }
        private IReadOnlyList<GameParameterNumberValue> _mainParametersBefore;
        private IReadOnlyList<GameParameterNumberValue> _mainParametersAfter;

        public bool Show => _showForce || MainParametersResult.Any();
        private readonly bool _showForce = false;

        public GameActionResult(
            string title,
            string? imageName,
            string[] text,
            bool? showForce)
        {
            Title = title;
            ImageName = imageName;
            Text = text;
            _showForce = showForce ?? false;
        }

        public void SetMainParametersBefore(Colony colony)
        {
            _mainParametersBefore = GetMainParameters(colony);
        }

        public void SetMainParametersAfter(Colony colony)
        {
            _mainParametersAfter = GetMainParameters(colony);
            CalcMainParametersResult();
        }

        private void CalcMainParametersResult()
        {
            var result = new List<KeyValuePair<GameParameterType, double[]>>(mainParameters.Count);
            foreach (var param in mainParameters)
            {
                var before = _mainParametersBefore.Single(x => x.ParameterType == param);
                var after = _mainParametersAfter.Single(x => x.ParameterType == param);
                if (before.Value == after.Value)
                    continue;
                result.Add(new(param, [before.Value, after.Value]));
            }
            MainParametersResult = result;
        }

        private IReadOnlyList<GameParameterNumberValue> GetMainParameters(Colony colony)
        {
            var result = new List<GameParameterNumberValue>(mainParameters.Count);
            foreach (var parameter in mainParameters)
            {
                var value = colony.State.GetValue(parameter);
                var colonyParameter = new GameParameterNumberValue(parameter, value);
                result.Add(colonyParameter);
            }
            return result;
        }

        public static GameActionResult CreateNew(
            string? title = null,
            string? imageName = null,
            string[]? text = null,
            bool showForce = false)
        {
            return new GameActionResult(
                title ?? "Результат",
                imageName,
                text ?? [],
                showForce);
        }

        private static IReadOnlyList<GameParameterType> mainParameters =>
        [
            GameParameterType.SolarsCurrent,
            GameParameterType.SolarsDelta,
            GameParameterType.MoodCurrent,
            GameParameterType.ModulesUsed,
            GameParameterType.Population
        ];
    }
}
