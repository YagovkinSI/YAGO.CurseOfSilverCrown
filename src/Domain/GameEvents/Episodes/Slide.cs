using System.Collections.Generic;
using YAGO.World.Domain.GameActions;

namespace YAGO.World.Domain.GameEvents.Episodes
{
    public class Slide
    {
        public string Id { get; }
        public string Title { get; }
        public string ImageName { get; }
        public string[] Text { get; }
        public IReadOnlyList<GameEffect> ParameterChanges { get; }
        public IReadOnlyList<SlideButton> Buttons { get; }
        public SlideTextInput? TextInput { get; }

        public Slide(
            string id,
            string title,
            string imageName,
            string[] text,
            IReadOnlyList<GameEffect> parameterChanges,
            IReadOnlyList<SlideButton> buttons,
            SlideTextInput? textInput = null)
        {
            Id = id;
            Title = title;
            ImageName = imageName;
            Text = text;
            ParameterChanges = parameterChanges;
            Buttons = buttons;
            TextInput = textInput;
        }
    }
}
