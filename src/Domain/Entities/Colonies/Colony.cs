using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Common.Entities;

namespace YAGO.World.Domain.Entities.Colonies
{
    /// <summary>
    /// Колония
    /// </summary>
    public class Colony : IEntity
    {
        /// <summary>
        /// Идентифиикатор колонии
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Идентифиикатор пользователя владельца
        /// </summary>
        public long UserId { get; }

        /// <summary>
        /// Идентифиикатор корабля
        /// </summary>
        public long ShipId { get; private set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Установленные законы
        /// </summary>
        public CodeOfLaws CodeOfLaws { get; }

        /// <summary>
        /// Солары
        /// </summary>
        public double Solars { get; private set; }

        /// <summary>
        /// Эффект от праздника
        /// </summary>
        public double FestivalEffect { get; private set; }

        /// <summary>
        /// Текущая неделя
        /// </summary>
        public int CurrentWeek { get; private set; }

        /// <summary>
        /// была ли первая свадьба
        /// </summary>
        public bool FirstWedding { get; private set; }

        /// <summary>
        /// Флаг деактивации колонии игроком
        /// </summary>
        public bool Deactivated { get; private set; }

        /// <summary>
        /// Время деактивации колонии игроком
        /// </summary>
        public DateTime? DeactivateAtUtc { get; private set; }

        /// <summary>
        /// Содержание станции
        /// </summary>
        public int Maintenance { get; }

        /// <summary>
        /// Максимальная прощадь под застройку
        /// </summary>
        public int Zones { get; }

        /// <summary>
        /// Отрасль добычи ресурсов
        /// </summary>
        public Industry MinningIndustry { get; }

        /// <summary>
        /// Отрасль производства продукции
        /// </summary>
        public Industry ProductionIndustry { get; }

        /// <summary>
        /// Отрасль оказания услуг
        /// </summary>
        public Industry ServiceIndustry { get; }

        public IReadOnlyCollection<Industry> Industries => [ MinningIndustry, ProductionIndustry, ServiceIndustry ];

        public Colony(
            long id,
            long userId,
            long shipId,
            string name,
            CodeOfLaws codeOfLaws,
            double solars,
            double festivalEffect,
            int currentWeek,
            bool firstWedding,
            bool deactivated,
            DateTime? deactivateAtUtc,
            int maintenance,
            int zones,
            Industry minningIndustry,
            Industry productionIndustry,
            Industry serviceIndustry)
        {
            Id = id;
            UserId = userId;
            ShipId = shipId;
            Name = name;
            CodeOfLaws = codeOfLaws;
            Solars = solars;
            FestivalEffect = festivalEffect;
            CurrentWeek = currentWeek;
            FirstWedding = firstWedding;
            Deactivated = deactivated;
            DeactivateAtUtc = deactivateAtUtc;
            Maintenance = maintenance;
            Zones = zones;
            MinningIndustry = minningIndustry;
            ProductionIndustry = productionIndustry;
            ServiceIndustry = serviceIndustry;
        }

        public static Colony CreateNew(
            long userId,
            string name,
            CodeOfLaws gavernorType)
        {
            return new Colony(
                id: default,
                userId: userId,
                shipId: 1,
                name: name,
                codeOfLaws: gavernorType,
                solars: 1000,
                festivalEffect: 0,
                currentWeek: 0,
                firstWedding: false,
                deactivated: false,
                deactivateAtUtc: null,
                maintenance: 100,
                zones: 140,
                minningIndustry: Industry.CreateNewMinning(),
                productionIndustry: Industry.CreateNewProduction(),
                serviceIndustry: Industry.CreateNewService());
        }

        public void AddSolars(double value)
        {
            Solars += value;
        }

        public void AddCompany(string industryName, int count, int zonesOccupied, int solarIncome, int population)
        {
            var industry = Industries.Single(x => x.Name == industryName);
            industry.AddCompany(count, zonesOccupied, solarIncome, population);
        }

        public void Deactivate()
        {
            Deactivated = true;
            DeactivateAtUtc = DateTime.UtcNow;
        }

        public void AddFestivalEffect(double effect)
        {
            FestivalEffect += effect;
        }

        internal void AddWeek()
        {
            CurrentWeek++;
        }

        internal void SetFirstWedding()
        {
            FirstWedding = true;
        }
    }
}
