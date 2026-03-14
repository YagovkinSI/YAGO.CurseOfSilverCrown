using System.Collections.Generic;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class Episode
    {
        public long? Id { get; }
        public IReadOnlyList<Slide> Slides { get; }
        public string? ChoiceLabel { get; }
        public IReadOnlyList<Slide>? Choice { get; }

        public Episode(
            long? id,
            IReadOnlyList<Slide> slides,
            string? сhoiceLabel,
            IReadOnlyList<Slide>? сhoice)
        {
            Id = id;
            Slides = slides;
            ChoiceLabel = сhoiceLabel;
            Choice = сhoice;
        }
    }
}
