using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels.Events;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Helpers
{
    internal class EventParametrChangeHelper
    {
        public static EventParametrChange Create(enEventParameterType parameter, int startParameter, int endParametr)
        {
            switch (parameter)
            {
                case enEventParameterType.Fortifications:
                    return CreateFortificationParametrChange(parameter, startParameter, endParametr);
                default:
                    return CreateDefaultParametrChange(parameter, startParameter, endParametr);
            }
        }

        private static EventParametrChange CreateDefaultParametrChange(enEventParameterType parameter, int startParameter, int endParametr)
        {
            return new EventParametrChange
            {
                Type = parameter,
                Before = startParameter,
                After = endParametr
            };
        }

        private static EventParametrChange CreateFortificationParametrChange(enEventParameterType parameter, int startParameter, int endParametr)
        {
            return new EventParametrChange
            {
                Type = enEventParameterType.Fortifications,
                Before = FortificationsHelper.GetFortCoef(startParameter),
                After = FortificationsHelper.GetFortCoef(endParametr),
            };
        }
    }
}
