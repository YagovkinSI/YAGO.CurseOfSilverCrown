namespace YAGO.World.Domain.Entities.Episodes
{
    public class SlideButtonAction
    {
        public string ActionName { get; }
        public string ActionParameters { get; }

        public SlideButtonAction(
            string actionName,
            string actionParameters)
        {
            ActionName = actionName;
            ActionParameters = actionParameters;
        }
    }
}
