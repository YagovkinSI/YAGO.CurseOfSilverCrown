using System.Data.Common;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class SlideButton
    {
        public string? Name { get; }
        public bool IsAvailable { get; }
        public SlideButtonAction? Action { get; }
        public SlideButtonNavigate? Navigate { get; }
        public SlideButtonToSlide? ToSlide { get; }

        public SlideButton(
            string? name,
            bool isAvailable,
            SlideButtonAction? action,
            SlideButtonNavigate? navigate,
            SlideButtonToSlide? toSlide)
        {
            Name = name;
            IsAvailable = isAvailable;
            Action = action;
            Navigate = navigate;
            ToSlide = toSlide;
        }

        public static SlideButton GetRunCycleButton(string? name = null)
        {
            return new(
                name ?? "Далее", 
                isAvailable: true, 
                new SlideButtonAction(EpisodeActionNames.RunCycle, []), 
                navigate: null,
                toSlide: null);
        }

        public static SlideButton GetSetChoiceButtonForTextInput(string eventId, string? name = null)
        {
            return new(
                name ?? "Выбрать",
                isAvailable: true,
                new SlideButtonAction(EpisodeActionNames.SetChoice, [eventId]),
                navigate: null,
                toSlide: null);
        }

        public static SlideButton GetSetChoiceButton(string eventId, string dilemmaResolving, string? name = null, bool isAvailable = true)
        {
            return new(
                name ?? "Выбрать",
                isAvailable: isAvailable,
                new SlideButtonAction(EpisodeActionNames.SetChoice, [eventId, dilemmaResolving]),
                navigate: null,
                toSlide: null);
        }

        public static SlideButton GetButtonToSlide(string slideId, string? name = null, bool isAvailable = true)
        {
            return new(
                name ?? "Далее",
                isAvailable: isAvailable,
                action: null,
                navigate: null,
                toSlide: new SlideButtonToSlide(slideId));
        }
    }
}
