namespace YAGO.World.Domain.GameEvents.Episodes
{
    public class SlideButtonAction
    {
        public SlideButtonActionType Type { get; set; }
        public string ActionName { get; }
        public string DilemmaResolving { get; }

        public SlideButtonAction(
            string actionName,
            string dilemmaResolving,
            SlideButtonActionType type = SlideButtonActionType.Default)
        {
            Type = type;
            ActionName = actionName;
            DilemmaResolving = dilemmaResolving;
        }
    }
}
