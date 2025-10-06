using System.Collections.Generic;

namespace YAGO.World.Infrastructure.Database.Cities
{
    internal class CityNamesData
    {
        public string Version { get; set; } = string.Empty;
        public string LastUpdated { get; set; } = string.Empty;
        public List<string> Names { get; set; } = [];
    }
}
