using System;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class Choice : Slide
    {
        public Guid Id { get; }
        public IReadOnlyList<ChoiceRequirement> Requirements { get; }

        public Choice(
            Guid id,
            string title,
            string imageName,
            string[] text,
            IReadOnlyList<KeyValueParameter> parameters,
            IReadOnlyList<ChoiceRequirement>? requirements = null)
            :base (title, imageName, text, parameters)
        {
            Id = id;
            Requirements = requirements ?? new List<ChoiceRequirement>();
        }

        public (bool IsAvailable, string ButtonName) CheckAvailability(ColonyStats colonyStats)
        {
            foreach (var choiceRequirement in Requirements)
            {
                var requirement = choiceRequirement.Parameter;
                if (!requirement.Check(colonyStats))
                    return (false, choiceRequirement.Message);
            }
            return (true, "Выбрать");
        }
    }
}
