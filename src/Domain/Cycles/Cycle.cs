using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.Notifications;

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

        public Notification RunCycle(ColonyWithShipAndContracts colonyWithShipAndContracts)
        {
            if (!colonyWithShipAndContracts.Contracts.Any())
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
                if (challenge.Check(colonyWithShipAndContracts.Parameters))
                {
                    notification = challenge.ToNotification();
                    colonyWithShipAndContracts.Colony.AddSolars(challenge.SolarChange);
                    StepNumber = i + 1;
                    return notification;
                }
            }

            StepNumber = challenges.Length;
            State = CycleState.Completed;
            colonyWithShipAndContracts.Colony.AddSolars(colonyWithShipAndContracts.SolarIncome);
            return CycleCompletedNotification(colonyWithShipAndContracts);

        }

        private static Notification CycleCompletedNotification(ColonyWithShipAndContracts colonyWithShipAndContracts)
        {
            var colonyParameters = new List<ColonyParameter>()
            {
                new(ColonyParameterType.Solars, colonyWithShipAndContracts.SolarIncome)
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
