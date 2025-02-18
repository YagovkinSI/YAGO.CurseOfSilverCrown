using YSI.CurseOfSilverCrown.Web.Database.Events;
using YSI.CurseOfSilverCrown.Web.Helpers;

namespace YSI.CurseOfSilverCrown.Web.Helpers.Events
{
    internal class EventJsonParametrChangeHelper
    {
        public static EventParticipantParameterChange Create(EventParticipantParameterType parameter, int startParameter, int endParametr)
        {
            switch (parameter)
            {
                case EventParticipantParameterType.Fortifications:
                    return CreateFortificationParametrChange(parameter, startParameter, endParametr);
                default:
                    return CreateDefaultParametrChange(parameter, startParameter, endParametr);
            }
        }

        private static EventParticipantParameterChange CreateDefaultParametrChange(EventParticipantParameterType parameter, int startParameter, int endParametr)
        {
            return new EventParticipantParameterChange
            {
                Type = parameter,
                Before = startParameter,
                After = endParametr
            };
        }

        private static EventParticipantParameterChange CreateFortificationParametrChange(EventParticipantParameterType parameter, int startParameter, int endParametr)
        {
            return new EventParticipantParameterChange
            {
                Type = EventParticipantParameterType.Fortifications,
                Before = FortificationsHelper.GetFortCoef(startParameter),
                After = FortificationsHelper.GetFortCoef(endParametr),
            };
        }
    }
}
