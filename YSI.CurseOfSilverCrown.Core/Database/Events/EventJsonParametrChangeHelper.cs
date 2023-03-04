using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.Core.Database.Events
{
    internal class EventJsonParametrChangeHelper
    {
        public static EventJsonParametrChange Create(enEventParameterType parameter, int startParameter, int endParametr)
        {
            switch (parameter)
            {
                case enEventParameterType.Fortifications:
                    return CreateFortificationParametrChange(parameter, startParameter, endParametr);
                default:
                    return CreateDefaultParametrChange(parameter, startParameter, endParametr);
            }
        }

        private static EventJsonParametrChange CreateDefaultParametrChange(enEventParameterType parameter, int startParameter, int endParametr)
        {
            return new EventJsonParametrChange
            {
                Type = parameter,
                Before = startParameter,
                After = endParametr
            };
        }

        private static EventJsonParametrChange CreateFortificationParametrChange(enEventParameterType parameter, int startParameter, int endParametr)
        {
            return new EventJsonParametrChange
            {
                Type = enEventParameterType.Fortifications,
                Before = FortificationsHelper.GetFortCoef(startParameter),
                After = FortificationsHelper.GetFortCoef(endParametr),
            };
        }
    }
}
