namespace YAGO.World.Domain.Colonies.Councils
{
    public class CouncilAdvisor
    {
        public string Code { get; }
        public int Loyalty { get; private set; }

        public CouncilAdvisor(
            string code,
            int loyalty)
        {
            Code = code;
            Loyalty = loyalty;
        }

        public void AddLoyalty(int delta)
        {
            Loyalty += delta;
        }
    }
}
