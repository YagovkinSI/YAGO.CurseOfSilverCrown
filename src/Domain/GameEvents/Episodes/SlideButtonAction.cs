namespace YAGO.World.Domain.GameEvents.Episodes
{
    public class SlideButtonAction
    {
        public SlideButtonActionType Type { get; set; }
        public string ActionName { get; }
        public string[] Arguments { get; }

        public SlideButtonAction(
            string actionName,
            string[] arguments,
            SlideButtonActionType type = SlideButtonActionType.Default)
        {
            Type = type;
            ActionName = actionName;
            Arguments = arguments;
        }
    }
}
