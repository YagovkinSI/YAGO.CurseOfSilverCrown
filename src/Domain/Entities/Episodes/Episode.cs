using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class Episode
    {
        public string? Id { get; }
        public IReadOnlyList<PrologueSlide> PrologueSlides { get; }
        public Dilemma? Dilemma { get; }

        /// <summary>
        /// Изменения колонии сразу при отработки события, если нет дилеммы
        /// </summary>
        public IReadOnlyList<KeyValueParameter>? ChangesWithoutChoice => Dilemma != null
            ? null
            : PrologueSlides[PrologueSlides.Count - 1].Parameters;

        public Episode(
            string? id,
            IReadOnlyList<PrologueSlide> prologSlides,
            Dilemma? dilemma)
        {
            Id = id;
            PrologueSlides = prologSlides;
            Dilemma = dilemma;
        }
    }
}
