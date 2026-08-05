using YAGO.World.Domain.Entities.Colonies.Industries;

namespace YAGO.World.Domain.Entities.Buildings.Calculation
{

    /// <summary>
    /// Тип предприятия (влияет на базовые пропорции формулы)
    /// </summary>
    public class BuildingTypeSettings
    {
        /// <summary>
        /// Название типа
        /// </summary>
        public ColonyIndustryType Type { get; set; } = ColonyIndustryType.Production;

        /// <summary>
        /// Доля выручки, идущая на сырьё и иные расходы (в долях от 0 до 1)
        /// </summary>
        public float RawMaterialsShare { get; set; } = 0.286f; // 1000/3500

        /// <summary>
        /// Доля выручки, идущая на зарплаты (в долях от 0 до 1)
        /// </summary>
        public float LaborShare { get; set; } = 0.286f; // 1000/3500

        /// <summary>
        /// Доля выручки, идущая на налоги (до применения льгот)
        /// </summary>
        public float TaxShare { get; set; } = 0.143f; // 500/3500

        /// <summary>
        /// Доля выручки, остающаяся как чистая прибыль
        /// </summary>
        public float ProfitShare { get; set; } = 0.286f; // 1000/3500

        /// <summary>
        /// Средняя зарплата одного работника (в Солар/год)
        /// </summary>
        public float AverageSalary { get; set; } = 10f; // 1000/100

        /// <summary>
        /// Количество работников на 10000 Солар инвестиций (базовое)
        /// </summary>
        public float BaseHeadcountPerInvestment { get; set; } = 0.01f; // 100/10000

        /// <summary>
        /// Фабричный метод для стандартного типа (из твоего примера)
        /// </summary>
        public static BuildingTypeSettings CreateDefault()
        {
            return new BuildingTypeSettings
            {
                Type = ColonyIndustryType.Production,
                RawMaterialsShare = 1000f / 3500f, // 0.286
                LaborShare = 1000f / 3500f,        // 0.286
                TaxShare = 500f / 3500f,           // 0.143
                ProfitShare = 1000f / 3500f,       // 0.286
                AverageSalary = 10f,
                BaseHeadcountPerInvestment = 0.01f // 100 человек на 10000 SOL
            };
        }

        /// <summary>
        /// Фабричный метод для добывающего предприятия (с ускоренной амортизацией)
        /// </summary>
        public static BuildingTypeSettings CreateMining()
        {
            var type = CreateDefault();
            type.Type = ColonyIndustryType.Mining;
            // В добыче больше расходов на сырьё, но выше прибыль
            type.RawMaterialsShare = 0.11f;      // 75/700
            type.TaxShare = 0.25f;               // 175/700
            type.LaborShare = 0.21f;             // 150/700
            type.ProfitShare = 0.43f;            // 300/700
            return type;
        }
    }
}