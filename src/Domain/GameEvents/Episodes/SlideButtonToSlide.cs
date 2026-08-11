namespace YAGO.World.Domain.GameEvents.Episodes
{
    public class SlideButtonToSlide
    {
        public string SlideId { get; }

        public SlideButtonToSlide(
            string slideId)
        {
            SlideId = slideId;
        }
    }
}
