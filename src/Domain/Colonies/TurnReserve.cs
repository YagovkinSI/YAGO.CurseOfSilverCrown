using System;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Colonies
{
    public class TurnReserve
    {
        public readonly int[] ReserveTimesInSeconds = [120, 360, 780, 1680, 3480, 7080, 12480, 23280, 44880, 80880];
        public int TurnsAvailableFixed { get; private set; }
        public DateTime LastTurnTimeAtUtc { get; private set; }

        public TurnReserve(
            int turnsAvailable, 
            DateTime lastTurnTimeAtUtc)
        {
            TurnsAvailableFixed = turnsAvailable;
            LastTurnTimeAtUtc = lastTurnTimeAtUtc;
        }

        internal static TurnReserve CreateNew()
        {
            return new TurnReserve(10, DateTime.UtcNow);
        }

        public DateTime GetNextTurnStartAtUtc(DateTime nowUtc)
        {
            if (TurnsAvailableFixed > 0)
                return LastTurnTimeAtUtc;

            return LastTurnTimeAtUtc + TimeSpan.FromSeconds(ReserveTimesInSeconds[0]);
        }

        internal void UseTurn(DateTime nowUtc)
        {
            var reserveTurnsCalculated = ReserveTurnsCalculate(nowUtc);
            if (reserveTurnsCalculated < 1)
                throw new YagoNotValidException("Ход пока не доступен. Повторите позднее.");

            reserveTurnsCalculated--;
            TurnsAvailableFixed = reserveTurnsCalculated;
            LastTurnTimeAtUtc = nowUtc;
        }

        private int ReserveTurnsCalculate(DateTime nowUtc)
        {
            if (TurnsAvailableFixed >= ReserveTimesInSeconds.Length)
                return ReserveTimesInSeconds.Length;

            var timePassed = nowUtc - LastTurnTimeAtUtc;
            var secondsPassed = (long)timePassed.TotalSeconds;

            int gained = TurnsAvailableFixed;
            long cumulativeTime = 0;
            for (int i = TurnsAvailableFixed; i < ReserveTimesInSeconds.Length - TurnsAvailableFixed; i++)
            {
                cumulativeTime += ReserveTimesInSeconds[i];
                if (secondsPassed >= cumulativeTime)
                    gained = i + 1;
                else
                    break;
            }

            return gained;
        }
    }
}
