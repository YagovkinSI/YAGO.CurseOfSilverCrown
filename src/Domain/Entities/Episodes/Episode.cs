using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class Episode
    {
        public IReadOnlyList<Slide> Slides { get; }
        public Dilemma? Dilemma { get; }
        public IReadOnlyList<KeyValueParameter>? ChangesWithoutChoice { get; }

        public Episode(
            IReadOnlyList<Slide> slides,
            Dilemma? dilemma,
            IReadOnlyList<KeyValueParameter>? changesWithoutChoice = null)
        {
            Slides = slides;
            Dilemma = dilemma;
            ChangesWithoutChoice = changesWithoutChoice;
        }
    }
}
