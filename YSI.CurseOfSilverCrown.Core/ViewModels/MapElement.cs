using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;

namespace YSI.CurseOfSilverCrown.Core.ViewModels
{
    public class MapElement
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public string Aplpha { get; set; }
        public List<string> Units { get; set; }
        public string ColorStr => $"rgba({Color.R}, {Color.G}, {Color.B}, {Aplpha})";

        public MapElement(string name, Color color, string aplpha, List<string> units)
        {
            Name = name;
            Color = color;
            Aplpha = aplpha;
            Units = units;
        } 
    }
}
