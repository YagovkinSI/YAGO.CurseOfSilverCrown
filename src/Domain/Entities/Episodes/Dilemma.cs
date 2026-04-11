using System;
using System.Collections.Generic;
using System.Linq;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class Dilemma
    {
        public IReadOnlyList<Choice> Choices { get; }
        public ChoiceType ChoiceType { get; }
        public string[] ChoiceLabel { get; }

        public bool HasChoice => Choices.Any();

        public Dilemma(
            IReadOnlyList<Choice> choice,
            ChoiceType choiceType = ChoiceType.Select,
            string[]? choiceLabel = null)
        {
            Choices = choice;
            ChoiceType = choiceType;
            ChoiceLabel = choiceLabel ?? ["Сделай выбор"];
        }

        public Choice GetChoice(Guid choiceId)
        {
            return Choices.Single(x => x.Id == choiceId);
        }
    }
}
