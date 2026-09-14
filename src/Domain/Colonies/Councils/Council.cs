using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Colonies.Councils
{
    public class Council
    {
        public CouncilAdvisor? Administrator { get; private set; }
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

        internal void SetAdministrator(CouncilAdvisor administrator)
        {
            Administrator = administrator;
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

        public bool CanHireAdministrator()
        {
            return Administrator == null;
        }

        public bool CanHireEngineer()
        {
            return Engineer == null && Administrator != null;
        }

        public bool CanHireFinancier()
        {
            return Financier == null && Administrator != null;
        }

        public bool CanHireSocial()
        {
            return Social == null && Administrator != null;
        }
    }
}
