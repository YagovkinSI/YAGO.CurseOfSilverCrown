using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Event
{
    public class EventParametrChange
    {
        public enActionParameter Type { get; set; }
        public int Before { get; set; }
        public int After { get; set; }
    }
}
