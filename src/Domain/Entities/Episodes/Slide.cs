using System;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class Slide
    {
        public Guid Id { get; }
        public string Title { get; }
        public string ImageName { get; }
        public string[] Text { get; }
        public IReadOnlyList<KeyValueParameter> Parameters { get; }

        public Slide(
            Guid id,
            string title,
            string imageName,
            string[] text,
            IReadOnlyList<KeyValueParameter> parameters)
        {
            Id = id;
            Title = title;
            ImageName = imageName;
            Text = text;
            Parameters = parameters;
        }
    }
}
