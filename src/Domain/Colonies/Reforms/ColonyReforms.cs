namespace YAGO.World.Domain.Colonies.Reforms
{
    /// <summary>
    /// Реформы колонии
    /// </summary>
    public class ColonyReforms
    {
        public CorporateTaxRate CorporateTaxRate { get; }
        public MedicalInsurance MedicalInsurance { get; }
        public double PublicDebt { get; private set; }

        public double Humanism => MedicalInsurance.Value + 3 - CorporateTaxRate.Value / 7;

        public LawsType LawsType => Humanism switch
        {
            > 1 => LawsType.Humanist,
            < -1 => LawsType.Corporate,
            _ => LawsType.Standard,
        };

        public ColonyReforms(
            CorporateTaxRate corporateTaxRate,
            MedicalInsurance medicalInsurance,
            double publicDebt)
        {
            CorporateTaxRate = corporateTaxRate;
            MedicalInsurance = medicalInsurance;
            PublicDebt = publicDebt;
        }

        internal static ColonyReforms CreateNew()
        {
            return new ColonyReforms(
                CorporateTaxRate.CreateNew(),
                MedicalInsurance.CreateNew(),
                publicDebt: 0);
        }

        internal void AddPublicDebt(double delta)
        {
            PublicDebt += delta;
        }
    }
}