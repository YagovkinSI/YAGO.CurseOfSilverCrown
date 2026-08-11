using System;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Turns
{
    public class Turn : IEntity<Guid>
    {
        /// <summary>
        /// Идентификатор цикла
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Идентификатор колонии владельца
        /// </summary>
        public Guid ColonyId { get; }

        /// <summary>
        /// Дата и время начала цикла (раньше запусить нельзя)
        /// </summary>
        public DateTime StartAtUtc { get; }

        /// <summary>
        /// Дата и время запуска цикла
        /// </summary>
        public DateTime? RunAtUtc { get; private set; }

        /// <summary>
        /// Статус цикла
        /// </summary>
        public bool IsComplited { get; private set; }

        public Turn(
            Guid id,
            Guid colonyId,
            DateTime startAtUtc,
            DateTime? runAtUtc,
            bool isComplited)
        {
            Id = id;
            ColonyId = colonyId;
            StartAtUtc = startAtUtc;
            RunAtUtc = runAtUtc;
            IsComplited = isComplited;
        }

        public static Turn CreateNew(
            Guid colonyId,
            Turn? prevTurn)
        {
            var startAtUtc = TurnStartDateTimeCalculator.CalcStartAtUtc(prevTurn);
            return new Turn(
                id: Guid.NewGuid(),
                colonyId: colonyId,
                startAtUtc: startAtUtc,
                runAtUtc: null,
                isComplited: false);
        }

        public void RunTurn()
        {
            if (IsComplited)
                throw new YagoException("Цикл уже завершен.");
            if (StartAtUtc > DateTime.UtcNow + TimeSpan.FromSeconds(2))
                throw new YagoException("Цикл не готов к запуску. Дождитесь готовности не более двух минут.");

            if (RunAtUtc == null)
                RunAtUtc = DateTime.UtcNow;
        }

        public void SetCompleted()
        {
            IsComplited = true;
        }
    }
}
