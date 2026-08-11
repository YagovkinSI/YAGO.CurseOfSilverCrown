using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.Stations
{
    public class StationModel
    {
        public StationModelId Id { get; }
        public DisplayInfo DisplayInfo { get; }
        public int ModulesTotal { get; }

        public StationModel(
            StationModelId modelId, 
            DisplayInfo displayInfo, 
            int modulesTotal)
        {
            Id = modelId;
            DisplayInfo = displayInfo;
            ModulesTotal = modulesTotal;
        }

    }
}
