using System.Collections.Generic;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Domain.GameEvents.Episodes
{
    public class SlideButton
    {
        public string? Name { get; }
        public IReadOnlyList<RequirementsParameter> Requirements { get; }
        public SlideButtonAction? Action { get; }
        public SlideButtonNavigate? Navigate { get; }
        public SlideButtonToSlide? ToSlide { get; }
        public string? InfoSlideId { get; }

        public SlideButton(
            string? name,
            IReadOnlyList<RequirementsParameter> requirements,
            SlideButtonAction? action,
            SlideButtonNavigate? navigate,
            SlideButtonToSlide? toSlide,
            string? infoSlideId)
        {
            Name = name;
            Requirements = requirements;
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
                requirements: [],
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
                requirements: [],
                action,
                navigate: null,
                toSlide: null,
                infoSlideId);
        }

        public static SlideButton GetSetChoiceButton(
            string eventId,
            string dilemmaResolving,
            string? name = null,
            IReadOnlyList<RequirementsParameter>? requirements = null,
            string? infoSlideId = null)
        {
            return new(
                name ?? "Выбрать",
                requirements: requirements ?? [],
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
                requirements: [],
                action: null,
                navigate: null,
                toSlide: new SlideButtonToSlide(slideId),
                infoSlideId);
        }
    }
}
