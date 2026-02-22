using System.Collections.Generic;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Domain.Episodes
{
    public class Slide
    {
        public string Title { get; }
        public string Illustration { get; }
        public string[] Text { get; }
        public IReadOnlyList<KeyValueParameter> Parameters { get; }

        public Slide(
            string title,
            string illustration,
            string[] text,
            IReadOnlyList<KeyValueParameter> parameters)
        {
            Title = title;
            Illustration = illustration;
            Text = text;
            Parameters = parameters;
        }
    }
}
