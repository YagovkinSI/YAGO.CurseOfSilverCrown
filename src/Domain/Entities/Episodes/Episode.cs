using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class Episode
    {
        public string? Id { get; }
        public IReadOnlyList<Slide> PrologSlides { get; }
        public IReadOnlyList<Slide> ChoiceSlides { get; }
        public string ChoiceLabel { get; }
        public bool HasChoice => ChoiceSlides.Count > 1;
        public IReadOnlyList<KeyValueParameter>? ChangesWithoutChoice => HasChoice
            ? null
            : ChoiceSlides.Single().Parameters;

        public Episode(
            string? id,
            IReadOnlyList<Slide> prologSlides,
            IReadOnlyList<Slide> choice,
            string? choiceLabel = null)
        {
            if (choice.Count == 0)
                throw new YagoException("Эпизод должен иметь хотя бы один завершающий слайд.");

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
