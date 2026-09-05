namespace YAGO.World.Domain.Colonies
{
    public class Council
    {
        public CouncilAdvisor? Administrator { get; }
        public CouncilAdvisor? Engineer { get; }
        public CouncilAdvisor? Financier { get; }
        public CouncilAdvisor? Social { get; }

        public Council(
            CouncilAdvisor? administrator,
            CouncilAdvisor? engineer,
            CouncilAdvisor? financier,
            CouncilAdvisor? social)
        {
            Administrator = administrator;
            Engineer = engineer;
            Financier = financier;
            Social = social;
        }

        internal static Council CreateNew()
        {
            return new Council(
                administrator: null,
                engineer: null,
                financier: null,
                social: null);
        }
    }
}