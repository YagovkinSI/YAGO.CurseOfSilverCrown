using System.Collections.Generic;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class Episode
    {
        public string? Id { get; }
        public IReadOnlyList<Slide> Slides { get; }
        public string? ChoiceLabel { get; }
        public IReadOnlyList<Slide>? Choice { get; }

        public Episode(
            string? id,
            IReadOnlyList<Slide> slides,
            string? choiceLabel = null,
            IReadOnlyList<Slide>? choice = null)
        {
            Id = id;
            Slides = slides;
            ChoiceLabel = choiceLabel;
            Choice = choice;
        }
    }
}
