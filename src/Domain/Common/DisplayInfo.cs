namespace YAGO.World.Domain.Common
{
    public class DisplayInfo
    {
        public string Name { get; }
        public string ImageName { get; }
        public string[] Description { get; }

        public DisplayInfo(
            string name,
            string imageName,
            string[] description)
        {
            Name = name;
            ImageName = imageName;
            Description = description;
        }
    }
}
