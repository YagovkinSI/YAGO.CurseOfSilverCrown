using System;
using YAGO.World.Domain.Colonies.Industries;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Colonies.Buildings
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

        public double ProfitabilityPrivate => SolarProfit * (1.0 - (Context.EffectiveTaxRate / 100.0)) / Investment * 100.0;
        public double Cost => IsPrivate
            ? Math.Ceiling(Math.Max(0, Investment * (1 - ((ProfitabilityPrivate + Context.Stability) / 15.0))) / 10) * 10
            : Investment;

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

        public double Profit => Gdp - Expenses;
        public double SolarProfit => (Gdp * SolarsDeltaFactor) - Expenses;
        protected abstract double SolarsDeltaFactor { get; }

        public double SolarsDelta => IsPrivate
            ? SolarProfit * (Context.EffectiveTaxRate / 100f) / GameConstants.WeeksInYear
            : SolarProfit / GameConstants.WeeksInYear;

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

            colonyState.Resources.Solars.Add(-Cost);
            colonyState.Resources.ActionPoints.Add(-1);
            if (IsPrivate)
                industry.AddPrivate(1);
            else
                industry.AddState(1);
        }

        public abstract (bool isBuildAvailable, string? reason) IsBuildAvailable(bool isPrivate, ColonyState colonyState);
        public (bool isBuildAvailable, string? reason) IsBuildAvailableBase(bool isPrivate, ColonyState colonyState)
        {
            if (colonyState.Slots[Slots.ColonySlotType.Modules].GetFree(colonyState) < ModulesUsed)
                return (false, "Недостаточно модулей на станции.");

            if (colonyState.Resources.Solars.Value < Cost)
                return (false, "Недостаточно Солар.");

            if (colonyState.Resources.ActionPoints.Value < 1)
                return (false, "Кончились очки действия. Сделайте ход.");

            if (IsPrivate && ProfitabilityPrivate < 3)
                return (false, "Не рентабельно для частного сектора.");

            return (true, null);
        }
    }
}
