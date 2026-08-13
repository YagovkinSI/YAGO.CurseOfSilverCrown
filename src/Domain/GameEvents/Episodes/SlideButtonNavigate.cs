namespace YAGO.World.Domain.GameEvents.Episodes
{
    public class SlideButtonNavigate
    {
        public string ActionUrl { get; }

        public SlideButtonNavigate(
            string actionUrl)
        {
            ActionUrl = actionUrl;
        }
    }
}
