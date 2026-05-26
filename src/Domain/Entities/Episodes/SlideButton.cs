using System;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class SlideButton
    {
        public string? Name { get; }
        public IReadOnlyList<ButtonAvailableRequirement> AvailableRequirements { get; }
        public SlideButtonAction? Action { get; }
        public SlideButtonNavigate? Navigate { get; }
        public SlideButtonToSlide? ToSlide { get; }

        public SlideButton(
            string? name,
            IReadOnlyList<ButtonAvailableRequirement> availableRequirements,
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
            IReadOnlyList<ButtonAvailableRequirement>? availableRequirements = null)
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

        public (bool IsAvailable, string? ButtonName) CheckAvailability(ColonyStats colonyStats)
        {
            foreach (var requirement in AvailableRequirements)
            {
                var parameter = requirement.Parameter;
                if (!parameter.Check(colonyStats))
                    return (false, requirement.Message);
            }
            return (true, null);
        }
    }
}
