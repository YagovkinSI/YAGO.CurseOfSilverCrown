using YSI.CurseOfSilverCrown.Core.MainModels.Events;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions.Organizations.Parameters
{
    internal abstract class ActionParameter
    {
        public enEventParameterType Type { get; protected set; }
        public int Before { get; protected set; }
        public int After { get; protected set; }

        public ActionParameter(int count)
        {
            Before = After = count;
        }
    }
}
