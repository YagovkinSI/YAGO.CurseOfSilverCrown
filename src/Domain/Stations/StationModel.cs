namespace YAGO.World.Domain.Stations
{
    public class StationModel
    {
        public StationModelId Id { get; }
        public string Name { get; }
        public int ModulesTotal { get; }

        public StationModel(
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
