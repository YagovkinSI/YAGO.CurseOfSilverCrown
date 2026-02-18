using System;

namespace YAGO.World.Domain.Colonies.Parameters
{
    public class Attractiveness
    {
        public double Extraction { get; private set; }

        public Attractiveness(Colony colony, ColonyCompanies companies)
        {
            colony.ValidateContracts(companies);

            var extraction = 103.0;

            var companyCount = companies.Companies.Count;
            extraction -= companyCount * 9.2;

            Extraction = Math.Clamp(extraction, 0, 100);
        }
    }
}
