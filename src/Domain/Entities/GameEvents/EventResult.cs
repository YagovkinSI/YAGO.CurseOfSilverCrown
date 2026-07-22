using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Domain.Entities.GameEvents
{
    public class EventResult
    {
        public string Title { get; }
        public string? ImageName { get; }
        public string[] Text { get; }
        public IReadOnlyList<KeyValueParameter> MainParametersBefore { get; private set; }
        public IReadOnlyList<KeyValueParameter> MainParametersAfter { get; private set; }
        public IReadOnlyList<KeyValuePair<string, double[]>> MainParametersResult { get; private set; }

        public bool Show => _showForce || MainParametersResult.Any();
        private bool _showForce = false;

        public EventResult(
            string title,
            string? imageName,
            string[] text,
            IReadOnlyList<KeyValueParameter> mainParametersBefore,
            IReadOnlyList<KeyValueParameter> mainParametersAfter,
            IReadOnlyList<KeyValuePair<string, double[]>> mainParametersResult,
            bool? showForce)
        {
            Title = title;
            ImageName = imageName;
            Text = text;
            MainParametersBefore = mainParametersBefore;
            MainParametersAfter = mainParametersAfter;
            MainParametersResult = mainParametersResult;
            _showForce = showForce ?? false;
        }

        public void SetMainParametersBefore(Colony colony)
        {
            MainParametersBefore = GetMainParameters(colony);
        }

        public void SetMainParametersAfter(Colony colony)
        {
            MainParametersAfter = GetMainParameters(colony);
            CalcMainParametersResult();
        }

        private void CalcMainParametersResult()
        {
            var result = new List<KeyValuePair<string, double[]>>(StateKeys.MainParameters.Count);
            foreach (var param in StateKeys.MainParameters)
            {
                var before = MainParametersBefore.Single(x => x.Name == param);
                var after = MainParametersAfter.Single(x => x.Name == param);
                if (before.Value == after.Value)
                    continue;
                result.Add(new(param, [ before.Value, after.Value ]));
            }
            MainParametersResult = result;
        }

        private IReadOnlyList<KeyValueParameter> GetMainParameters(Colony colony)
        {
            var result = new List<KeyValueParameter>(StateKeys.MainParameters.Count);
            foreach (var param in StateKeys.MainParameters)
            {
                result.Add(new(param, colony.States.GetGameParameter(param)));
            }
            return result;
        }

        public static EventResult CreateNew(
            string? title = null,
            string? imageName = null,
            string[]? text = null,
            bool showForce = false)
        {
            return new EventResult(
                title ?? "Результат события",
                imageName,
                text ?? [],
                mainParametersBefore: [],
                mainParametersAfter: [],
                mainParametersResult: [],
                showForce);
        }
    }
}
