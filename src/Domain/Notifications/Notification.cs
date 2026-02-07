using System.Collections.Generic;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Domain.Notifications
{
    public class Notification
    {
        public string Title { get; }
        public string Illustration { get; }
        public string[] Text { get; }
        public IReadOnlyList<ColonyParameter> Parameters { get; }

        public Notification(
            string title,
            string illustration,
            string[] text,
            IReadOnlyList<ColonyParameter> parameters)
        {
            Title = title;
            Illustration = illustration;
            Text = text;
            Parameters = parameters;
        }
    }
}
