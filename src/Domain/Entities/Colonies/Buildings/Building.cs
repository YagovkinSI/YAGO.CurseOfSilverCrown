using System;
using YAGO.World.Domain.Entities.Colonies.Industries;
using YAGO.World.Domain.Entities.Colonies.Resources;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Entities.Colonies.Buildings
{
    public abstract class Building
    {
        public abstract ColonyIndustryType Type { get; }
        public bool IsPrivate { get; }
        public BuildingContext Context { get; }

        public abstract string Name { get; }
        public abstract string ImageName { get; }
        public abstract string[] Description { get; }

        public abstract double Investment { get; }

        public double Gdp => Investment * _gdpBaseFactor * GdpTypeFactor;
        private const double _gdpBaseFactor = 0.35;
        public abstract double GdpTypeFactor { get; }

        public int ModulesUsed => (int)Math.Ceiling(Investment * _modulesUsedBaseFactor * ModulesUsedTypeFactor);
        private const double _modulesUsedBaseFactor = 0.0025;
        public abstract double ModulesUsedTypeFactor { get; }

        public int Population => (int)Math.Ceiling(Investment * _populationBaseFactor * PopulationTypeFactor);
        private const double _populationBaseFactor = 0.012;
        public abstract double PopulationTypeFactor { get; }

        public double Expenses => Investment * _expensesBaseFactor;
        private const double _expensesBaseFactor = 0.2;

        public double Profit => (Gdp - Expenses) / 52.0;
        public double SolarProfit => (Gdp * SolarsDeltaFactor - Expenses) / 52.0;
        protected abstract double SolarsDeltaFactor { get; }

        private const int _tempFactorDemo = 15;

        public double SolarsDelta => IsPrivate
            ? _tempFactorDemo * SolarProfit * (Context.EffectiveTaxRate / 100f)
            : _tempFactorDemo * SolarProfit;

        protected Building(
            bool isPrivate,
            BuildingContext context)
        {
            IsPrivate = isPrivate;
            Context = context;
        }

        public void Build(ColonyState colonyState)
        {
            var industry = colonyState.Industries[Type];
            var (isBuildAvailable, reason) = IsBuildAvailable(IsPrivate, colonyState);
            if (!isBuildAvailable)
                throw new YagoException(reason!);

            if (IsPrivate)
            {
                colonyState.Resources[ColonyResourceType.Solars].Add(-Investment / 5);
                industry.AddPrivate(1);
            }
            else
            {
                colonyState.Resources[ColonyResourceType.Solars].Add(-Investment);
                industry.AddState(1);
            }
        }

        public abstract (bool isBuildAvailable, string? reason) IsBuildAvailable(bool isPrivate, ColonyState colonyState);
    }
}
