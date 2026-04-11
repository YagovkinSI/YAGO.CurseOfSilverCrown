using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class Episode
    {
        public string? Id { get; }
        public IReadOnlyList<Slide> PrologSlides { get; }
        public Dilemma? Dilemma { get; }

        public bool HasChoice => Dilemma?.HasChoice ?? false;
        public IReadOnlyList<KeyValueParameter>? ChangesWithoutChoice => HasChoice
            ? null
            : PrologSlides[PrologSlides.Count - 1].Parameters;

        public Episode(
            string? id,
            IReadOnlyList<Slide> prologSlides,
            Dilemma? dilemma)
        {
            Id = id;
            PrologSlides = prologSlides;
            Dilemma = dilemma;
        }
    }
}
