namespace YAGO.World.Domain.Entities.Episodes
{
    public class SlideButtonAction
    {
        public string ActionName { get; }
        public string[] Arguments { get; }

        public SlideButtonAction(
            string actionName,
            string[] arguments)
        {
            ActionName = actionName;
            Arguments = arguments;
        }
    }
}
