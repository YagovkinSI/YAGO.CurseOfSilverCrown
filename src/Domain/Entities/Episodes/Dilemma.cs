namespace YAGO.World.Domain.Entities.Episodes
{
    public abstract class Dilemma
    {
        public abstract DilemmaType DilemmaType { get; }

        protected Dilemma()
        {
        }
    }
}
