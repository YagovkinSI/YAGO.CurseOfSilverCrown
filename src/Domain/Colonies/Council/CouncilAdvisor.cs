namespace YAGO.World.Domain.Colonies
{
    public class CouncilAdvisor
    {
        public string Code { get; }
        public double Loyalty { get; }

        public CouncilAdvisor(
            string code,
            double loyalty)
        {
            Code = code;
            Loyalty = loyalty;
        }
    }
}