using System.Collections.Generic;
using YAGO.World.Domain.GameActions;

namespace YAGO.World.Domain.GameEvents.Episodes
{
    public class SlideButton
    {
        public string? Name { get; }
        public IReadOnlyList<GameRequirement> Requirements { get; }
        public SlideButtonAction? Action { get; }
        public SlideButtonNavigate? Navigate { get; }
        public SlideButtonToSlide? ToSlide { get; }
        public string? InfoSlideId { get; }
        public string? AdministratorSlideId { get; }
        public string? EngineerSlideId { get; }
        public string? FinancierSlideId { get; }
        public string? SocialSlideId { get; }
        public SlideButtonKind Kind { get; }

        public SlideButton(
            string? name,
            IReadOnlyList<GameRequirement> requirements,
            SlideButtonAction? action,
            SlideButtonNavigate? navigate,
            SlideButtonToSlide? toSlide,
            string? infoSlideId,
            SlideButtonKind kind = SlideButtonKind.Default,
            string? administratorSlideId = null,
            string? engineerSlideId = null,
            string? financierSlideId = null,
            string? socialSlideId = null)
        {
            Name = name;
            Requirements = requirements;
            Action = action;
            Navigate = navigate;
            ToSlide = toSlide;
            InfoSlideId = infoSlideId;
            AdministratorSlideId = administratorSlideId;
            EngineerSlideId = engineerSlideId;
            FinancierSlideId = financierSlideId;
            SocialSlideId = socialSlideId;
            Kind = kind;
        }

        public static SlideButton GetCloseNewsButton(
            string eventId,
            string? name = null,
            string? infoSlideId = null,
            SlideButtonKind kind = SlideButtonKind.Default,
            string? administratorSlideId = null,
            string? engineerSlideId = null,
            string? financierSlideId = null,
            string? socialSlideId = null)
        {
            return new(
                name ?? "ОК",
                requirements: [],
                new SlideButtonAction(EpisodeActionNames.SetChoice, string.Empty),
                navigate: null,
                toSlide: null,
                infoSlideId,
                kind,
                administratorSlideId,
                engineerSlideId,
                financierSlideId,
                socialSlideId);
        }

        public static SlideButton GetSetChoiceButtonForTextInput(
            bool isInputCompleted,
            string? name = null,
            string? infoSlideId = null,
            SlideButtonKind kind = SlideButtonKind.Default,
            string? administratorSlideId = null,
            string? engineerSlideId = null,
            string? financierSlideId = null,
            string? socialSlideId = null)
        {
            var action = new SlideButtonAction(
                EpisodeActionNames.SetChoice,
                dilemmaResolving: string.Empty,
                isInputCompleted ? SlideButtonActionType.InputCompleted : SlideButtonActionType.InputMissed);
            return new(
                name ?? "Выбрать",
                requirements: [],
                action,
                navigate: null,
                toSlide: null,
                infoSlideId,
                kind,
                administratorSlideId,
                engineerSlideId,
                financierSlideId,
                socialSlideId);
        }

        public static SlideButton GetSetChoiceButton(
            string dilemmaResolving,
            string? name = null,
            IReadOnlyList<GameRequirement>? requirements = null,
            string? infoSlideId = null,
            SlideButtonKind kind = SlideButtonKind.Default,
            string? administratorSlideId = null,
            string? engineerSlideId = null,
            string? financierSlideId = null,
            string? socialSlideId = null)
        {
            return new(
                name ?? "Выбрать",
                requirements: requirements ?? [],
                new SlideButtonAction(EpisodeActionNames.SetChoice, dilemmaResolving),
                navigate: null,
                toSlide: null,
                infoSlideId,
                kind,
                administratorSlideId,
                engineerSlideId,
                financierSlideId,
                socialSlideId);
        }

        public static SlideButton GetButtonToSlide(
            string slideId,
            string? name = null,
            string? infoSlideId = null,
            SlideButtonKind kind = SlideButtonKind.Default,
            string? administratorSlideId = null,
            string? engineerSlideId = null,
            string? financierSlideId = null,
            string? socialSlideId = null)
        {
            return new(
                name ?? "Далее",
                requirements: [],
                action: null,
                navigate: null,
                toSlide: new SlideButtonToSlide(slideId),
                infoSlideId,
                kind,
                administratorSlideId,
                engineerSlideId,
                financierSlideId,
                socialSlideId);
        }
    }
}
