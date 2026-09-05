namespace YAGO.World.Domain.Stations
{
    public class StationModel
    {
        public StationModelId Id { get; }
        public int ModulesTotal { get; }

        public StationModel(
            StationModelId modelId,
            int modulesTotal)
        {
            Id = modelId;
            ModulesTotal = modulesTotal;
        }

    }
}
