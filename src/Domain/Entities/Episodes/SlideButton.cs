namespace YAGO.World.Domain.Entities.Episodes
{
    public class SlideButton
    {
        public string? Name { get; }
        public bool IsAvilable { get; }
        public SlideButtonAction? Action { get; }

        public SlideButton(
            string? name,
            bool isAvilable,
            SlideButtonAction? action)
        {
            Name = name;
            IsAvilable = isAvilable;
            Action = action;
        }


    }
}
