using System.Collections.Generic;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class SlideButton
    {
        public string? Name { get; }
        public IReadOnlyList<ActionAvailableRequirement> AvailableRequirements { get; }
        public SlideButtonAction? Action { get; }
        public SlideButtonNavigate? Navigate { get; }
        public SlideButtonToSlide? ToSlide { get; }

        public SlideButton(
            string? name,
            IReadOnlyList<ActionAvailableRequirement> availableRequirements,
            SlideButtonAction? action,
            SlideButtonNavigate? navigate,
            SlideButtonToSlide? toSlide)
        {
            Name = name;
            AvailableRequirements = availableRequirements;
            Action = action;
            Navigate = navigate;
            ToSlide = toSlide;
        }

        public static SlideButton GetRunCycleButton(string? name = null)
        {
            return new(
                name ?? "Далее",
                availableRequirements: [],
                new SlideButtonAction(EpisodeActionNames.RunCycle, []),
                navigate: null,
                toSlide: null);
        }

        public static SlideButton GetSetChoiceButtonForTextInput(string eventId, string? name = null)
        {
            return new(
                name ?? "Выбрать",
                availableRequirements: [],
                new SlideButtonAction(EpisodeActionNames.SetChoice, [eventId]),
                navigate: null,
                toSlide: null);
        }

        public static SlideButton GetSetChoiceButton(
            string eventId,
            string dilemmaResolving,
            string? name = null,
            IReadOnlyList<ActionAvailableRequirement>? availableRequirements = null)
        {
            return new(
                name ?? "Выбрать",
                availableRequirements: availableRequirements ?? [],
                new SlideButtonAction(EpisodeActionNames.SetChoice, [eventId, dilemmaResolving]),
                navigate: null,
                toSlide: null);
        }

        public static SlideButton GetButtonToSlide(
            string slideId,
            string? name = null)
        {
            return new(
                name ?? "Далее",
                availableRequirements: [],
                action: null,
                navigate: null,
                toSlide: new SlideButtonToSlide(slideId));
        }
    }
}
