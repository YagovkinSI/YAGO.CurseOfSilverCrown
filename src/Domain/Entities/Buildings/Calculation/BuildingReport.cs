namespace YAGO.World.Domain.Entities.Buildings.Calculation
{

    /// <summary>
    /// Отчёт о состоянии предприятия
    /// </summary>
    public class BuildingReport
    {
        public float Investment { get; set; }
        public float Revenue { get; set; }
        public float RawMaterialsCosts { get; set; }
        public float LaborCosts { get; set; }
        public float GrossProfit { get; set; }
        public float TaxAmount { get; set; }
        public float NetProfit { get; set; }
        public float TreasuryRevenue { get; set; }
        public float Headcount { get; set; }
        public float AverageSalary { get; set; }
        public float ROI { get; set; }
        public float EffectiveTaxRate { get; set; }
        public float AutomationEfficiency { get; set; }
        public float LaborCostMultiplier { get; set; }
        public float HeadcountMultiplier { get; set; }
        public float TaxBonusPercent { get; set; }

        public override string ToString()
        {
            return $@"
═══════════════════════════════════════
  ОТЧЁТ ПРЕДПРИЯТИЯ
═══════════════════════════════════════
  Инвестиции:           {Investment:F0} SOL
  Выручка:              {Revenue:F0} SOL
  Сырьё и материалы:    -{RawMaterialsCosts:F0} SOL
  Расходы на персонал:  -{LaborCosts:F0} SOL
  ─────────────────────────────────────
  Валовая прибыль:      {GrossProfit:F0} SOL
  Налоги:               -{TaxAmount:F0} SOL
  Чистая прибыль:       {NetProfit:F0} SOL
  ─────────────────────────────────────
  В казну:              {TreasuryRevenue:F0} SOL
  Рабочих мест:         {Headcount:F0}
  Средняя зарплата:     {AverageSalary:F2} SOL
  ROI:                  {ROI:F1}%
  Эффективная ставка:   {EffectiveTaxRate:F1}%
  ─────────────────────────────────────
  Множители:
    Эффективность:      {AutomationEfficiency:F2}x
    Затраты на труд:    {LaborCostMultiplier:F2}x
    Численность:        {HeadcountMultiplier:F2}x
    Бонус к налогу:     {TaxBonusPercent:F0}%
═══════════════════════════════════════
";
        }
    }
}