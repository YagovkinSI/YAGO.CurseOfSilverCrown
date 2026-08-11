using System;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Turns
{
    public static class TurnStartDateTimeCalculator
    {
        private const int TimeoutBetweenTurnsInSeconds = 12;

        public static DateTime CalcStartAtUtc(Turn? prevTurn)
        {
            if (prevTurn == null)
                return DateTime.UtcNow;

            if (!prevTurn.IsComplited)
                throw new YagoException("Прошлый цикл должен быть завершен.");
            if (prevTurn.RunAtUtc == null)
                throw new YagoException("Время запуска прошлого цикла не может быть NULL.");

            return prevTurn.RunAtUtc.Value + TimeSpan.FromSeconds(TimeoutBetweenTurnsInSeconds);
        }
    }
}
