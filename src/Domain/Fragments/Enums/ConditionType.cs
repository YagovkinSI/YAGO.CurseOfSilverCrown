namespace YAGO.World.Domain.Fragments.Enums
{
    public enum ConditionType
    {
        Unknown = 0,

        AND = 1,
        OR = 2,

        CountMoreThan = 10,
        CountLessThan = 11,

        ContainsAny = 20,
        ContainsAll = 21,
        NotContainsAny = 22,
    }
}
