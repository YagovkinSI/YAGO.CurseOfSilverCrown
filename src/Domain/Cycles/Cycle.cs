using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Budgets;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Companies;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.Notifications;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Domain.Cycles
{
    public class Cycle : IEntity
    {
        /// <summary>
        /// Идентификатор цикла
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Идентификатор колонии владельца
        /// </summary>
        public long ColonyId { get; }

        /// <summary>
        /// Шаг цикла
        /// </summary>
        public int StepNumber { get; private set; }

        /// <summary>
        /// Дата и время запуска цикла
        /// </summary>
        public DateTime? RunAtUtc { get; private set; }

        /// <summary>
        /// Статус цикла
        /// </summary>
        public CycleState State { get; private set; }

        public Cycle(
            long id,
            long colonyId,
            int stepNumber,
            DateTime? runAtUtc,
            CycleState cycleState)
        {
            Id = id;
            ColonyId = colonyId;
            StepNumber = stepNumber;
            RunAtUtc = runAtUtc;
            State = cycleState;
        }

        public Notification RunCycle(Colony colony, ColonyCompanies companies, Ship ship)
        {
            if (!companies.Companies.Any())
                throw new YagoException("Не пройзведено найма колонистов.");

            if (State == CycleState.Ready)
                State = CycleState.InProgress;

            if (RunAtUtc == null)
                RunAtUtc = DateTime.UtcNow;

            Notification? notification;
            var challenges = GameEventsDataset.Get();
            for (var i = StepNumber; i < challenges.Length; i++)
            {
                var challenge = challenges[i];
                if (challenge.Check(colony, companies, ship))
                {
                    notification = challenge.ToNotification();
                    SetParameters(colony, challenge.ColonyParameters);
                    var newCompanies = CompanyDataset.GetCompanies(colony.CompanyIds);
                    companies.Update(newCompanies); 
                    StepNumber = i + 1;
                    return notification;
                }
            }

            StepNumber = challenges.Length;
            State = CycleState.Completed;
            var budget = new Budget(
                colony,
                companies,
                ship);
            colony.AddSolars(budget.Balance);
            return CycleCompletedNotification(budget);
        }

        public void SetParameters(Colony colony, IReadOnlyList<ColonyParameter> colonyParameters)
        {
            var solars = colonyParameters.FirstOrDefault(x => x.Type == ColonyParameterType.Solars);
            if (solars != null)
                colony.AddSolars((int)solars.Value);

            var engineeringTeam = colonyParameters.FirstOrDefault(x => x.Type == ColonyParameterType.EngineeringTeam);
            if (engineeringTeam != null)
                colony.AddCompany(1);

            var miningBrigade = colonyParameters.FirstOrDefault(x => x.Type == ColonyParameterType.MiningBrigade);
            if (miningBrigade != null)
                colony.AddCompany(2);

            var rehabilitationContingent = colonyParameters.FirstOrDefault(x => x.Type == ColonyParameterType.RehabilitationContingent);
            if (rehabilitationContingent != null)
                colony.AddCompany(3);
        }

        private static Notification CycleCompletedNotification(Budget budget)
        {
            var colonyParameters = new List<ColonyParameter>()
            {
                new(ColonyParameterType.Solars, budget.Balance)
            };

            return new Notification(
                "Успешное завершение цикла",
                IllustrationRunCycle.RegularCycle,
                new string[]
                {
                    "В трюмах ритмично гудят дробилки, на мостике горят зелёные лампочки систем. " +
                    "Рудокопы в своих сменах монотонно, но эффективно откалывают породу.",
                    "Цикл успешно завершен, прибыль получена.",
                },
                colonyParameters);
        }
    }
}
