using System.Collections.Generic;

namespace YAGO.World.Domain.GameEvents.Episodes
{
    public class Episode
    {
        public IReadOnlyList<Slide> Slides { get; }

        public Episode(
            IReadOnlyList<Slide> slides)
        {
            Slides = slides;
        }
    }
}
