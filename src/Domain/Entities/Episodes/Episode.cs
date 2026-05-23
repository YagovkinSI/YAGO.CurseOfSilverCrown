using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class Episode
    {
        public string? Id { get; }
        public string Title { get; }
        public IReadOnlyList<Slide> Slides { get; }
        public Dilemma? Dilemma { get; }

        /// <summary>
        /// Изменения колонии сразу при отработки события, если нет дилеммы
        /// </summary>
        public IReadOnlyList<KeyValueParameter>? ChangesWithoutChoice => Dilemma != null
            ? null
            : Slides[Slides.Count - 1].Parameters;

        public Episode(
            string? id,
            string title,
            IReadOnlyList<Slide> slides,
            Dilemma? dilemma)
        {
            Id = id;
            Title = title;
            Slides = slides;
            Dilemma = dilemma;
        }
    }
}
