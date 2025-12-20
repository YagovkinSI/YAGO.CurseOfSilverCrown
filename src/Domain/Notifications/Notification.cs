using System.Collections.Generic;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.Notifications
{
    public class Notification
    {
        public string Title { get; }
        public IllustrationType Illustration { get; }
        public string Text { get; }
        public IReadOnlyList<ColonyParameter> Parameters { get; }

        public Notification(
            string title, 
            IllustrationType illustration, 
            string text, 
            IReadOnlyList<ColonyParameter> parameters)
        {
            Title = title;
            Illustration = illustration;
            Text = text;
            Parameters = parameters;
        }
    }
}
