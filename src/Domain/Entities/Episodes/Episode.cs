using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class Episode
    {
        public IReadOnlyList<Slide> Slides { get; }
        public IReadOnlyList<KeyValueParameter>? ChangesWithoutChoice { get; }

        public Episode(
            IReadOnlyList<Slide> slides,
            IReadOnlyList<KeyValueParameter>? changesWithoutChoice = null)
        {
            Slides = slides;
            ChangesWithoutChoice = changesWithoutChoice;
        }
    }
}
