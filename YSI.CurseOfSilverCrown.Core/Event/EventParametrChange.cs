using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.Core.Event
{
    public class EventParametrChange
    {
        public enEventParametrChange Type { get; set; }
        public int Before { get; set; }
        public int After { get; set; }
    }
}
