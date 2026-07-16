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
        public string? InfoSlideId { get; }

        public SlideButton(
            string? name,
            IReadOnlyList<ActionAvailableRequirement> availableRequirements,
            SlideButtonAction? action,
            SlideButtonNavigate? navigate,
            SlideButtonToSlide? toSlide,
            string? infoSlideId)
        {
            Name = name;
            AvailableRequirements = availableRequirements;
            Action = action;
            Navigate = navigate;
            ToSlide = toSlide;
            InfoSlideId = infoSlideId;
        }

        public static SlideButton GetCloseNewsButton(
            string eventId,
            string? name = null,
            string? infoSlideId = null)
        {
            return new(
                name ?? "ОК",
                availableRequirements: [],
                new SlideButtonAction(EpisodeActionNames.SetChoice, [eventId, string.Empty]),
                navigate: null,
                toSlide: null,
                infoSlideId);
        }

        public static SlideButton GetSetChoiceButtonForTextInput(
            string eventId,
            bool isInputCompleted,
            string? name = null,
            string? infoSlideId = null)
        {
            var action = new SlideButtonAction(
                EpisodeActionNames.SetChoice,
                [eventId],
                isInputCompleted ? SlideButtonActionType.InputCompleted : SlideButtonActionType.InputMissed);
            return new(
                name ?? "Выбрать",
                availableRequirements: [],
                action,
                navigate: null,
                toSlide: null,
                infoSlideId);
        }

        public static SlideButton GetSetChoiceButton(
            string eventId,
            string dilemmaResolving,
            string? name = null,
            IReadOnlyList<ActionAvailableRequirement>? availableRequirements = null,
            string? infoSlideId = null)
        {
            return new(
                name ?? "Выбрать",
                availableRequirements: availableRequirements ?? [],
                new SlideButtonAction(EpisodeActionNames.SetChoice, [eventId, dilemmaResolving]),
                navigate: null,
                toSlide: null,
                infoSlideId);
        }

        public static SlideButton GetButtonToSlide(
            string slideId,
            string? name = null,
            string? infoSlideId = null)
        {
            return new(
                name ?? "Далее",
                availableRequirements: [],
                action: null,
                navigate: null,
                toSlide: new SlideButtonToSlide(slideId),
                infoSlideId);
        }
    }
}
