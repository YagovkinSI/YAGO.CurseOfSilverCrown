namespace YSI.CurseOfSilverCrown.Web.Models
{
    public class AspPage : ILink
    {
        public string Area { get; set; }
        public string Page { get; set; }
        public string DisplayText { get; set; }

        public AspPage(string area, string page, string displayText)
        {
            Area = area;
            Page = page;
            DisplayText = displayText;
        }
    }
}