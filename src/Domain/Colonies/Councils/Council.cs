using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Colonies.Councils
{
    public class Council
    {
        private const int InitialLoyalty = 50;

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
                administrator: new CouncilAdvisor(InitialLoyalty),
                engineer: new CouncilAdvisor(InitialLoyalty),
                financier: new CouncilAdvisor(InitialLoyalty),
                social: new CouncilAdvisor(InitialLoyalty));
        }

        internal void AddLoyalty(CouncilAdvisorRole councilRole, int delta)
        {
            switch (councilRole)
            {
                case CouncilAdvisorRole.Ruler:
                    throw new YagoException("Нельзя изменить параметры лояльности правителя.");
                case CouncilAdvisorRole.Administrator:
                    Administrator?.AddLoyalty(delta);
                    break;
                case CouncilAdvisorRole.Engineer:
                    Engineer?.AddLoyalty(delta);
                    break;
                case CouncilAdvisorRole.Financier:
                    Financier?.AddLoyalty(delta);
                    break;
                case CouncilAdvisorRole.Social:
                    Social?.AddLoyalty(delta);
                    break;
            }
        }
    }
}
