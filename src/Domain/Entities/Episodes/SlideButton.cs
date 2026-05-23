namespace YAGO.World.Domain.Entities.Episodes
{
    public class SlideButton
    {
        public string? Name { get; }
        public bool IsAvailable { get; }
        public SlideButtonAction? Action { get; }
        public SlideButtonNavigate? Navigate { get; }

        public SlideButton(
            string? name,
            bool isAvailable,
            SlideButtonAction? action,
            SlideButtonNavigate? navigate)
        {
            Name = name;
            IsAvailable = isAvailable;
            Action = action;
            Navigate = navigate;
        }

        public static SlideButton RunCycleButton(string? name = null)
        {
            return new(name ?? "Далее", isAvailable: true, new SlideButtonAction(EpisodeActionNames.RunCycle, string.Empty), navigate: null);
        }
    }
}
