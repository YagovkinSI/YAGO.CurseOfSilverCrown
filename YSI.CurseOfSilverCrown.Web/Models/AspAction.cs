namespace YSI.CurseOfSilverCrown.Web.Models
{
    public class AspAction : ILink
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string DisplayText { get; set; }

        public AspAction(string controller, string action, string displayText)
        {
            Controller = controller;
            Action = action;
            DisplayText = displayText;
        }
    }
}