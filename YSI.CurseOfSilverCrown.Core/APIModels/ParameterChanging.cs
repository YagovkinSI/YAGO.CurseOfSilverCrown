namespace YSI.CurseOfSilverCrown.Core.APIModels
{
    public class ParameterChanging<T>
    {
        public T CurrentValue { get; set; }

        public T ExpectedValue { get; set; }

        public ParameterChanging(T current, T expected)
        {
            CurrentValue = current;
            ExpectedValue = expected;
        }
    }
}
