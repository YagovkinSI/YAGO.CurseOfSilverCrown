using System.Collections.Generic;

namespace YAGO.World.Domain.Entities.Colonies.Buildings
{
    public abstract class ColonyBuilding
    {
        public abstract ColonyBuildingType Type { get; }
        public int PrivateCount { get; private set; }
        public int StateCount { get; private set; }
        public int Total => PrivateCount + StateCount;

        protected ColonyBuilding(int privateCount, int stateCount)
        {
            PrivateCount = privateCount;
            StateCount = stateCount;
        }

        internal void AddPrivate(int delta)
        {
            PrivateCount += delta;
        }

        internal void AddState(int delta)
        {
            StateCount += delta;
        }

        internal static List<ColonyBuilding> CreateNew()
        {
            return
            [
                new ColonyAdministrative(privateCount: 0, stateCount: 0),
                new ColonyMining(privateCount: 0, stateCount: 0),
                new ColonyProduction(privateCount: 0, stateCount: 0),
                new ColonyService(privateCount: 0, stateCount: 0),
            ];
        }

        public abstract BuildingSettings GetSettings();

        public abstract (bool isBuildAvailable, string? reason) IsBuildAvailable(bool isPrivate, ColonyState colonyState);
    }
}
