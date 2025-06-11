using System;
using System.Collections.Generic;
using System.Drawing;
using YAGO.World.Domain.YagoEntities;

namespace YAGO.World.Infrastructure.APIModels
{
    public class MapElement
    {
        public YagoEntity YagoEntity { get; set; }

        [Obsolete("Entity.Name")]
        public string Name => YagoEntity.Name;
        public Color Color { get; set; }
        public string Aplpha { get; set; }
        public List<string> Info { get; set; }
        public string ColorStr => $"rgba({Color.R}, {Color.G}, {Color.B}, {Aplpha})";

        public MapElement(YagoEntity yagoEntity, Color color, string aplpha, List<string> info)
        {
            YagoEntity = yagoEntity;
            Color = color;
            Aplpha = aplpha;
            Info = info;
        }
    }
}
