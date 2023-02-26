namespace YSI.CurseOfSilverCrown.Core.MainModels.Events
{
    public class EventJsonParametrChange
    {
        public enEventParameterType Type { get; set; }
        public int Before { get; set; }
        public int After { get; set; }
    }
}
