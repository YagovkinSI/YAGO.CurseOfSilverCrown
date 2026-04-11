using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class Episode
    {
        public string? Id { get; }
        public IReadOnlyList<Slide> PrologSlides { get; }
        public IReadOnlyList<Choice> Choices { get; }
        public ChoiceType ChoiceType { get; }
        public string[] ChoiceLabel { get; }

        public bool HasChoice => Choices.Any();
        public IReadOnlyList<KeyValueParameter>? ChangesWithoutChoice => HasChoice
            ? null
            : PrologSlides[PrologSlides.Count - 1].Parameters;

        public Episode(
            string? id,
            IReadOnlyList<Slide> prologSlides,
            IReadOnlyList<Choice> choice,
            ChoiceType choiceType = ChoiceType.Select,
            string[]? choiceLabel = null)
        {
            Id = id;
            PrologSlides = prologSlides;
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
