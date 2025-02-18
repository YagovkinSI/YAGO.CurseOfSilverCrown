using System.Collections.Generic;

namespace YAGO.World.Host.Models
{
    public class Card
    {
        public bool IsLeftSide { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Time { get; set; }
        public int SpecialOperation { get; set; }
        public List<ILink> Links { get; set; } = new List<ILink>();
    }
}