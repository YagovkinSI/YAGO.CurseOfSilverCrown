using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class Choice : Slide
    {
        public IReadOnlyList<ChoiceRequirement> Requirements { get; }
        public string ChoiceButtonName { get; }

        public Choice(
            string id,
            string title,
            string imageName,
            string[] text,
            IReadOnlyList<KeyValueParameter> parameters,
            IReadOnlyList<ChoiceRequirement>? requirements = null,
            string? buttonName = null,
            IReadOnlyList<SlideButton>? buttons = null)
            : base(id, title, imageName, text, parameters, buttonName ?? "Выбрать", buttons ?? [])
        {
            Requirements = requirements ?? new List<ChoiceRequirement>();
            ChoiceButtonName = buttonName ?? "Выбрать";
        }

        public (bool IsAvailable, string ButtonName) CheckAvailability(ColonyStats colonyStats)
        {
            foreach (var choiceRequirement in Requirements)
            {
                var requirement = choiceRequirement.Parameter;
                if (!requirement.Check(colonyStats))
                    return (false, choiceRequirement.Message);
            }
            return (true, ChoiceButtonName);
        }
    }
}
