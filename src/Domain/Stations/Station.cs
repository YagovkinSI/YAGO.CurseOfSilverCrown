namespace YAGO.World.Domain.Stations
{
    public class Station
    {
        public StationModelId Id { get; }
        public string Name { get; }
        public int ModulesTotal { get; }

        public Station(
            StationModelId modelId,
            string name,
            int modulesTotal)
        {
            Id = modelId;
            Name = name;
            ModulesTotal = modulesTotal;
        }
    }
}
