using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class Slide
    {
        public string Title { get; }
        public string ImageName { get; }
        public string[] Text { get; }
        public IReadOnlyList<KeyValueParameter> Parameters { get; }
        public TextInput? TextInput { get; }
        public string? ContinueButtonName { get; }

        public Slide(
            string title,
            string imageName,
            string[] text,
            IReadOnlyList<KeyValueParameter> parameters,
            TextInput? textInput = null,
            string? continueButtonName = null)
        {
            Title = title;
            ImageName = imageName;
            Text = text;
            Parameters = parameters;
            TextInput = textInput;
            ContinueButtonName = continueButtonName;
        }
    }
}
