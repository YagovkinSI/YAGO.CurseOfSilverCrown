namespace YAGO.World.Domain.Colonies
{
    public class CouncilAdvisor
    {
        public string Code { get; }
        public int Loyalty { get; }

        public CouncilAdvisor(
            string code,
            int loyalty)
        {
            Code = code;
            Loyalty = loyalty;
        }
    }
}