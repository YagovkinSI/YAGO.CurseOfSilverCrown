using YAGO.World.Domain.Colonies.Councils;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal static class CouncilEntityMapper
    {
        public static ColonyCouncilEntity ToEntity(Council council)
        {
            return new ColonyCouncilEntity(
                ToAdvisorEntity(council.Administrator),
                ToAdvisorEntity(council.Engineer),
                ToAdvisorEntity(council.Financier),
                ToAdvisorEntity(council.Social));
        }

        public static Council ToDomain(ColonyCouncilEntity council)
        {
            return new Council(
                ToAdvisor(council.Administrator),
                ToAdvisor(council.Engineer),
                ToAdvisor(council.Financier),
                ToAdvisor(council.Social));
        }

        private static ColonyCouncilAdvisorEntity? ToAdvisorEntity(CouncilAdvisor? advisor)
        {
            return advisor == null
                ? null
                : new ColonyCouncilAdvisorEntity(advisor.Loyalty);
        }

        private static CouncilAdvisor? ToAdvisor(ColonyCouncilAdvisorEntity? advisor)
        {
            return advisor == null
                ? null
                : new CouncilAdvisor(advisor.Loyalty);
        }
    }
}
