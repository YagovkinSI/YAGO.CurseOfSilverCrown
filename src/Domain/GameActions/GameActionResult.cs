using System.Numerics;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Slots;
using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.GameActions
{
    public class GameActionResultValue<T>
        where T : INumber<T>
    {
        public T Before { get; private set; } = default;
        public T After { get; private set; } = default;
        public T Delta => After - Before;

        public void Set(T value, bool isBefore)
        {
            if (isBefore)
                Before = value;
            else
                After = value;
        }
    }

    public class GameActionResult
    {
        public DisplayInfo DisplayInfo { get; }

        public GameActionResultValue<double> SolarsCurrent { get; } = new GameActionResultValue<double>();
        public GameActionResultValue<double> SolarsDelta { get; } = new GameActionResultValue<double>();
        public GameActionResultValue<double> MoodCurrent { get; } = new GameActionResultValue<double>();
        public GameActionResultValue<int> ModulesUsed { get; } = new GameActionResultValue<int>();
        public GameActionResultValue<int> Population { get; } = new GameActionResultValue<int>();
        private bool _hasDelta;
        public bool Show => _showForce || _hasDelta;
        private readonly bool _showForce = false;

        public GameActionResult(
            DisplayInfo displayInfo,
            bool? showForce)
        {
            DisplayInfo = displayInfo;
            _showForce = showForce ?? false;
        }

        public void SetMainParametersBefore(Colony colony)
        {
            SetMainParameters(colony, isBefore: true);
        }

        public void SetMainParametersAfter(Colony colony)
        {
            SetMainParameters(colony, isBefore: false);
            _hasDelta = SolarsCurrent.Delta != default ||
                SolarsDelta.Delta != default ||
                MoodCurrent.Delta != default ||
                ModulesUsed.Delta != default ||
                Population.Delta != default;
        }

        private void SetMainParameters(Colony colony, bool isBefore)
        {
            var solarsCurrent = colony.State.Resources.Solars.Value;
            SolarsCurrent.Set(solarsCurrent, isBefore);

            var solarDelta = colony.GetSolarDelta();
            SolarsDelta.Set(solarDelta, isBefore);

            var moodCurrent = colony.State.Resources.Mood.Value;
            MoodCurrent.Set(moodCurrent, isBefore);

            var modulesUsed = colony.State.Slots[ColonySlotType.Modules].GetUsed(colony.State);
            ModulesUsed.Set(modulesUsed, isBefore);

            var population = colony.State.GetPopulation();
            Population.Set(population, isBefore);
        }

        public static GameActionResult CreateNew(
            DisplayInfo? displayInfo = null,
            bool? showForce = null)
        {
            var hasUniqueInfo = displayInfo != null;
            displayInfo ??= new DisplayInfo("Результат", imageName: null, description: []);
            return new GameActionResult(
                displayInfo,
                showForce ?? hasUniqueInfo);
        }
    }
}
