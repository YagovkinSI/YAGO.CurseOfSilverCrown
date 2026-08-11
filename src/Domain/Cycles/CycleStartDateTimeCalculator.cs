using System;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Cycles
{
    public static class CycleStartDateTimeCalculator
    {
        private const int TimeoutBetweenCyclesInSeconds = 12;

        public static DateTime CalcStartAtUtc(Cycle? prevCycle)
        {
            if (prevCycle == null)
                return DateTime.UtcNow;

            if (!prevCycle.IsComplited)
                throw new YagoException("Прошлый цикл должен быть завершен.");
            if (prevCycle.RunAtUtc == null)
                throw new YagoException("Время запуска прошлого цикла не может быть NULL.");

            return prevCycle.RunAtUtc.Value + TimeSpan.FromSeconds(TimeoutBetweenCyclesInSeconds);
        }
    }
}
