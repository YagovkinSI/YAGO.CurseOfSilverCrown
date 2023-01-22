using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Helpers
{
    internal class EventParametrChangeHelper
    {
        public static EventParametrChange Create(enActionParameter parameter, int startParameter, int endParametr)
        {
            switch (parameter)
            {
                case enActionParameter.Fortifications:
                    return CreateFortificationParametrChange(parameter, startParameter, endParametr);
                default:
                    return CreateDefaultParametrChange(parameter, startParameter, endParametr);
            }
        }

        private static EventParametrChange CreateDefaultParametrChange(enActionParameter parameter, int startParameter, int endParametr)
        {
            return new EventParametrChange
            {
                Type = parameter,
                Before = startParameter,
                After = endParametr
            };
        }

        private static EventParametrChange CreateFortificationParametrChange(enActionParameter parameter, int startParameter, int endParametr)
        {
            return new EventParametrChange
            {
                Type = enActionParameter.Fortifications,
                Before = FortificationsHelper.GetDefencePercent(startParameter),
                After = FortificationsHelper.GetDefencePercent(endParametr),
            };
        }
    }
}
