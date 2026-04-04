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
        public IReadOnlyList<Choice> ChoiceSlides { get; }
        public string ChoiceLabel { get; }

        public bool HasChoice => ChoiceSlides.Any();
        public IReadOnlyList<KeyValueParameter>? ChangesWithoutChoice => HasChoice
            ? null
            : PrologSlides[PrologSlides.Count - 1].Parameters;

        public Episode(
            string? id,
            IReadOnlyList<Slide> prologSlides,
            IReadOnlyList<Choice> choice,
            string? choiceLabel = null)
        {
            Id = id;
            PrologSlides = prologSlides;
            ChoiceSlides = choice;
            ChoiceLabel = choiceLabel ?? "Сделайте свой выбор?";
        }

        public Slide GetChoice(Guid choiceId)
        {
            return ChoiceSlides.Single(x => x.Id == choiceId);
        }
    }
}
