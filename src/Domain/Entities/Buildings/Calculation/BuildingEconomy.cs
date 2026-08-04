namespace YAGO.World.Domain.Entities.Buildings.Calculation
{
    /// <summary>
    /// Модель экономики предприятия
    /// </summary>
    public class BuildingEconomy
    {
        /// <summary>
        /// Инвестиции (масштаб) в Солар
        /// </summary>
        public float Investment { get; private set; }

        /// <summary>
        /// Текущие законы
        /// </summary>
        public EconomicLaws Laws { get; private set; }

        /// <summary>
        /// Тип предприятия
        /// </summary>
        public BuildingTypeSettings Type { get; private set; }

        // Базовые показатели (рассчитываются при создании)
        private readonly float _baseRevenue;
        private readonly float _baseRawCosts;
        private readonly float _baseLaborCosts;
        private readonly float _baseTax;
        private readonly float _baseProfit;
        private readonly float _baseHeadcount;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="investment">Инвестиции в Солар</param>
        /// <param name="laws">Действующие законы</param>
        /// <param name="type">Тип предприятия</param>
        public BuildingEconomy(float investment, EconomicLaws laws, BuildingTypeSettings type)
        {
            Investment = investment;
            Laws = laws;
            Type = type;

            // Расчитываем базовые показатели
            var scaleFactor = Investment / 10000f; // Масштабирование относительно 10000 SOL

            _baseRevenue = 3500f * scaleFactor;
            _baseRawCosts = _baseRevenue * Type.RawMaterialsShare;
            _baseLaborCosts = _baseRevenue * Type.LaborShare;
            _baseTax = _baseRevenue * Type.TaxShare;
            _baseProfit = _baseRevenue * Type.ProfitShare;
            _baseHeadcount = Investment * Type.BaseHeadcountPerInvestment;
        }

        // ==================== ОСНОВНЫЕ ПОКАЗАТЕЛИ ====================

        /// <summary>
        /// Валовая выручка предприятия (с учётом влияния автоматизации)
        /// </summary>
        public float GetRevenue()
        {
            // Автоматизация влияет на эффективность: роботы → выше выручка, ручной труд → ниже
            return _baseRevenue * Laws.AutomationEfficiencyMultiplier;
        }

        /// <summary>
        /// Расходы на сырьё и материалы (с учётом эффективности)
        /// </summary>
        public float GetRawMaterialsCosts()
        {
            var revenue = GetRevenue();
            // Доля расходов на сырьё зависит от эффективности (автоматизация снижает отходы)
            var efficiencyFactor = 1f / Laws.AutomationEfficiencyMultiplier;
            return revenue * Type.RawMaterialsShare * efficiencyFactor;
        }

        /// <summary>
        /// Расходы на оплату труда (с учётом автоматизации)
        /// </summary>
        public float GetLaborCosts()
        {
            var revenue = GetRevenue();
            var baseLaborCost = revenue * Type.LaborShare;
            // Автоматизация меняет структуру затрат: роботы дороже в обслуживании, люди дешевле
            return baseLaborCost * Laws.LaborCostMultiplier;
        }

        /// <summary>
        /// Сумма налога (с учётом льгот)
        /// </summary>
        public float GetTaxAmount()
        {
            var profit = GetGrossProfit();
            var effectiveRate = Laws.EffectiveTaxRate / 100f; // Переводим проценты в доли
            return profit * effectiveRate;
        }

        /// <summary>
        /// Валовая прибыль (до вычета налогов)
        /// </summary>
        public float GetGrossProfit()
        {
            var revenue = GetRevenue();
            var rawCosts = GetRawMaterialsCosts();
            var laborCosts = GetLaborCosts();
            return revenue - rawCosts - laborCosts;
        }

        /// <summary>
        /// Чистая прибыль предприятия (после уплаты налогов)
        /// </summary>
        public float GetNetProfit()
        {
            var grossProfit = GetGrossProfit();
            var tax = GetTaxAmount();
            return grossProfit - tax;
        }

        /// <summary>
        /// Доход казны (налоги, собранные с предприятия)
        /// </summary>
        public float GetTreasuryRevenue()
        {
            return GetTaxAmount();
        }

        /// <summary>
        /// Средняя зарплата одного работника
        /// </summary>
        public float GetAverageSalary()
        {
            var headcount = GetHeadcount();
            if (headcount <= 0) return 0;
            return GetLaborCosts() / headcount;
        }

        /// <summary>
        /// Количество рабочих мест
        /// </summary>
        public float GetHeadcount()
        {
            // Автоматизация меняет численность персонала
            return _baseHeadcount * Laws.HeadcountMultiplier;
        }

        /// <summary>
        /// Общий доход работника (зарплата + налоги с зарплаты, если нужно)
        /// </summary>
        public float GetWorkerTotalIncome()
        {
            return GetAverageSalary();
        }

        // ==================== ДОПОЛНИТЕЛЬНЫЕ ПОКАЗАТЕЛИ ====================

        /// <summary>
        /// Рентабельность (чистая прибыль / инвестиции) в процентах
        /// </summary>
        public float GetROI()
        {
            if (Investment <= 0) return 0;
            return GetNetProfit() / Investment * 100f;
        }

        /// <summary>
        /// Эффективная налоговая ставка (фактическая, с учётом всех льгот)
        /// </summary>
        public float GetEffectiveTaxRate()
        {
            var grossProfit = GetGrossProfit();
            if (grossProfit <= 0) return 0;
            return GetTaxAmount() / grossProfit * 100f;
        }

        /// <summary>
        /// Получение всех основных показателей в одном объекте
        /// </summary>
        public BuildingReport GetReport()
        {
            return new BuildingReport
            {
                Investment = Investment,
                Revenue = GetRevenue(),
                RawMaterialsCosts = GetRawMaterialsCosts(),
                LaborCosts = GetLaborCosts(),
                GrossProfit = GetGrossProfit(),
                TaxAmount = GetTaxAmount(),
                NetProfit = GetNetProfit(),
                TreasuryRevenue = GetTreasuryRevenue(),
                Headcount = GetHeadcount(),
                AverageSalary = GetAverageSalary(),
                ROI = GetROI(),
                EffectiveTaxRate = GetEffectiveTaxRate(),
                AutomationEfficiency = Laws.AutomationEfficiencyMultiplier,
                LaborCostMultiplier = Laws.LaborCostMultiplier,
                HeadcountMultiplier = Laws.HeadcountMultiplier,
                TaxBonusPercent = Laws.TaxBonusPercent
            };
        }
    }
}