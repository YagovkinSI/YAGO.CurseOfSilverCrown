using System.Collections.Generic;
using System.Drawing;

namespace YSI.CurseOfSilverCrown.Core.ViewModels
{
    public class MapElement
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public string Aplpha { get; set; }
        public List<string> Info { get; set; }
        public string ColorStr => $"rgba({Color.R}, {Color.G}, {Color.B}, {Aplpha})";

        public MapElement(string name, Color color, string aplpha, List<string> info)
        {
            Name = name;
            Color = color;
            Aplpha = aplpha;
            Info = info;
        }
    }
}
