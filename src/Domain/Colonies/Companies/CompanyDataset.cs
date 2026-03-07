using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies.Parameters;

namespace YAGO.World.Domain.Colonies.Companies
{
    public static class CompanyDataset
    {
        public static Company[] Get()
        {
            return
            [
                EngineeringTeam,
                MiningBrigade,
                RehabilitationContingent,
                ProductionCompany,
                ServiceCompany
            ];
        }

        public static readonly Company EngineeringTeam = new(
            id: 1,
            zonesOccupied: 3,
            solarsIncome: 60,
            population: 10);

        public static readonly Company MiningBrigade = new(
            id: 2,
            zonesOccupied: 3,
            solarsIncome: 40,
            population: 15);

        public static readonly Company RehabilitationContingent = new(
            id: 3,
            zonesOccupied: 5,
            solarsIncome: 70,
            population: 30);

        public static readonly Company ProductionCompany = new(
            id: 4,
            zonesOccupied: 5,
            solarsIncome: 20,
            population: 25);

        public static readonly Company ServiceCompany = new(
            id: 5,
            zonesOccupied: 3,
            solarsIncome: 20,
            population: 10);

        public static ColonyCompanies GetCompanies(IReadOnlyList<long> colonyCompanies)
        {
            var allCompanies = Get();

            var companies = colonyCompanies
                .Select(x => allCompanies.Single(c => c.Id == x))
                .ToList();

            return new ColonyCompanies(companies);
        }
    }
}
