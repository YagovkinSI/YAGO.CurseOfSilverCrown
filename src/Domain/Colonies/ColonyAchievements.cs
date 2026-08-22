namespace YAGO.World.Domain.Colonies
{
    public class ColonyAchievements
    {
        public bool RulerContractSigned { get; private set; }
        public bool FirstWedding { get; private set; }

        public ColonyAchievements(
            bool rulerContractSigned,
            bool firstWedding)
        {
            RulerContractSigned = rulerContractSigned;
            FirstWedding = firstWedding;
        }

        internal static ColonyAchievements CreateNew()
        {
            return new ColonyAchievements(
                rulerContractSigned: false,
                firstWedding: false);
        }

        public void SetRulerContractSigned() => RulerContractSigned = true;
        public void SetFirstWedding() => FirstWedding = true;
    }
}
