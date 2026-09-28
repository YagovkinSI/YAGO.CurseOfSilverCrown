namespace YAGO.World.Domain.GameEvents.Episodes
{
    public class SlideLink
    {
        public SlideLinkType Type { get; }
        public string? Value { get; }

        public SlideLink(
            SlideLinkType type,
            string? value = null)
        {
            Type = type;
            Value = value;
        }
    }
}