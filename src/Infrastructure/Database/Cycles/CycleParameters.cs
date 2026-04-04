namespace YAGO.World.Infrastructure.Database.Cycles
{
    public class CycleParameters
    {
        public string? ActiveEventId { get; private set; }

        public CycleParameters(
            string? activeEventId)
        {
            ActiveEventId = activeEventId;
        }
    }
}
