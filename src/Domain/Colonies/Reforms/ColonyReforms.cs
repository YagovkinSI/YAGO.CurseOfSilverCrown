namespace YAGO.World.Domain.Colonies.Reforms
{
    /// <summary>
    /// Реформы колонии
    /// </summary>
    public class ColonyReforms
    {
        public CorporateTaxRate CorporateTaxRate { get; }
        public double SocialGuaranteesLevel { get; private set; }
        public double PublicDebt { get; private set; }

        public double Humanism => SocialGuaranteesLevel - CorporateTaxRate.Value / 7;

        public LawsType LawsType => Humanism switch
        {
            > 1 => LawsType.Humanist,
            < -1 => LawsType.Corporate,
            _ => LawsType.Standard,
        };

        public ColonyReforms(
            CorporateTaxRate corporateTaxRate,
            double socialGuaranteesLevel,
            double publicDebt)
        {
            CorporateTaxRate = corporateTaxRate;
            SocialGuaranteesLevel = socialGuaranteesLevel;
            PublicDebt = publicDebt;
        }

        internal static ColonyReforms CreateNew()
        {
            return new ColonyReforms(
                CorporateTaxRate.CreateNew(),
                socialGuaranteesLevel: 3,
                publicDebt: 0);
        }

        internal void SetSocialGuaranteesLevel(double value)
        {
            SocialGuaranteesLevel = value;
        }

        internal void AddPublicDebt(double delta)
        {
            PublicDebt += delta;
        }
    }
}