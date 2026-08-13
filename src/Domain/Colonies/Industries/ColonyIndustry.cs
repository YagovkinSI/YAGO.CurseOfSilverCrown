using System.Collections.Generic;
using YAGO.World.Domain.Colonies.Buildings;

namespace YAGO.World.Domain.Colonies.Industries
{
    public abstract class ColonyIndustry
    {
        public abstract ColonyIndustryType Type { get; }
        public int PrivateCount { get; private set; }
        public int StateCount { get; private set; }
        public int Total => PrivateCount + StateCount;

        protected ColonyIndustry(int privateCount, int stateCount)
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

        internal static List<ColonyIndustry> CreateNew()
        {
            return
            [
                new ColonyAdministrative(privateCount: 0, stateCount: 0),
                new ColonyMining(privateCount: 0, stateCount: 0),
                new ColonyProduction(privateCount: 0, stateCount: 0),
                new ColonyService(privateCount: 0, stateCount: 0),
            ];
        }

        public abstract Building GetBuilding(bool isPrivate, BuildingContext buildingContext);

    }
}
