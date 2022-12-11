namespace YSI.CurseOfSilverCrown.Web.Models
{
    public class UrlLink : ILink
    {
        public string Url { get; set; }
        public string DisplayText { get; set; }

        public UrlLink(string url, string displayText)
        {
            Url = url;
            DisplayText = displayText;
        }
    }
}