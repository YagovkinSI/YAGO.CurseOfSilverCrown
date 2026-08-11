using System.Collections.Generic;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Domain.GameEvents.Episodes
{
    public class Slide
    {
        public string Id { get; }
        public string Title { get; }
        public string ImageName { get; }
        public string[] Text { get; }
        public IReadOnlyList<KeyValueParameter> Parameters { get; }
        public IReadOnlyList<SlideButton> Buttons { get; }
        public SlideTextInput? TextInput { get; }

        public Slide(
            string id,
            string title,
            string imageName,
            string[] text,
            IReadOnlyList<KeyValueParameter> parameters,
            IReadOnlyList<SlideButton> buttons,
            SlideTextInput? textInput = null)
        {
            Id = id;
            Title = title;
            ImageName = imageName;
            Text = text;
            Parameters = parameters;
            Buttons = buttons;
            TextInput = textInput;
        }
    }
}
