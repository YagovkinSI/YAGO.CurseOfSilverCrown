using System;

namespace YAGO.World.Domain.Colonies.Parameters
{
    public class Attractiveness
    {
        public double Extraction { get; private set; }

        public Attractiveness(Colony colony, ColonyCompanies companies)
        {
            colony.ValidateContracts(companies);

            var extraction = 150.0;

            var companyCount = companies.Companies.Count;
            extraction -= companyCount * 13;

            Extraction = Math.Clamp(extraction, 0, 100);
        }
    }
}
