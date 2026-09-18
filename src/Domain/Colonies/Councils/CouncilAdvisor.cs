namespace YAGO.World.Domain.Colonies.Councils
{
    public class CouncilAdvisor
    {
        public int Loyalty { get; private set; }

        public CouncilAdvisor(
            int loyalty)
        {
            Loyalty = loyalty;
        }

        public void AddLoyalty(int delta)
        {
            Loyalty += delta;
        }
    }
}
