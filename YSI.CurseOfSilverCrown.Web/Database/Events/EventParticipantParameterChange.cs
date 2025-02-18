namespace YSI.CurseOfSilverCrown.Web.Database.Events
{
    public class EventParticipantParameterChange
    {
        public EventParticipantParameterType Type { get; set; }
        public int Before { get; set; }
        public int After { get; set; }
    }
}
