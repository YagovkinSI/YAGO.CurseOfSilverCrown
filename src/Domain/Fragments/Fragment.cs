using YAGO.World.Domain.Slides;

namespace YAGO.World.Domain.Fragments
{
    public class Fragment
    {
        public long Id { get; }
        public string ChoiceText { get; }
        public Slide[] Slides { get; }
        public long[] NextFragmentIds { get; }

        public Fragment(
            long id,
            string choiceText,
            Slide[] slides,
            long[] nextFragmentIds)
        {
            Id = id;
            ChoiceText = choiceText;
            Slides = slides;
            NextFragmentIds = nextFragmentIds;
        }
    }
}
