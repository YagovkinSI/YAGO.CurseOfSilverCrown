using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class Episode
    {
        public IReadOnlyList<Slide> Slides { get; }
        public Dilemma? Dilemma { get; }

        /// <summary>
        /// Изменения колонии сразу при отработки события, если нет дилеммы
        /// </summary>
        public IReadOnlyList<KeyValueParameter>? ChangesWithoutChoice => Dilemma != null
            ? null
            : Slides[Slides.Count - 1].Parameters;

        public Episode(
            IReadOnlyList<Slide> slides,
            Dilemma? dilemma)
        {
            Slides = slides;
            Dilemma = dilemma;
        }
    }
}
