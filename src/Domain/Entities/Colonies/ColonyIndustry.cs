using System.Collections.Generic;
using YAGO.World.Domain.Entities.Buildings;

namespace YAGO.World.Domain.Entities.Colonies
{
    public class ColonyIndustry
    {
        public IndustryType Type { get; }
        public int PrivateCount { get; private set; }
        public int StateCount { get; private set; }
        public int Total => PrivateCount + StateCount;

        public ColonyIndustry(IndustryType type, int privateCount, int stateCount)
        {
            Type = type;
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
                new(IndustryType.Administrative, privateCount: 0, stateCount: 0),
                new(IndustryType.Mining, privateCount: 0, stateCount: 0),
                new(IndustryType.Production, privateCount: 0, stateCount: 0),
                new(IndustryType.Service, privateCount: 0, stateCount: 0),
            ];
        }
    }
}
