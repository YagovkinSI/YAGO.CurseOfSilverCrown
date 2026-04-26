using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.GameEvents
{
    public class DaysPassedOptions
    {
        public int DaysPassed { get; }
        public string[] Text { get; }
        public string Immage { get; }

        public DaysPassedOptions(
            int daysPassed,
            string[]? text = null,
            string? immage = null)
        {
            DaysPassed = daysPassed;
            Text = text ?? [GetDefaultText(daysPassed)];
            Immage = immage ?? ImageSet.RegularCycle;
        }

        private string GetDefaultText(int daysPassed)
        {
            return daysPassed switch
            {
                1 => "Прошёл день.",
                2 => "Спустя пару дней.",
                3 or 4 => $"Прошло {daysPassed} дня.",
                _ => $"Спустя {daysPassed} спокойных дней."
            };
        }
    }
}
