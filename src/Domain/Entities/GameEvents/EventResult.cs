using System.Collections.Generic;

namespace YAGO.World.Domain.Entities.GameEvents
{
    public class EventResult
    {
        public string Title { get; set; }
        public string? ImageName { get; set; }
        public string[] Text { get; set; }
        public IReadOnlyList<KeyValueParameter> Parameters { get; set; }

        public EventResult(
            string title,
            string? imageName,
            string[] text,
            IReadOnlyList<KeyValueParameter> parameters)
        {
            Title = title;
            ImageName = imageName;
            Text = text;
            Parameters = parameters;
        }
    }
}
