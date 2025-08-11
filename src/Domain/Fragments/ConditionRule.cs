using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Fragments.Enums;

namespace YAGO.World.Domain.Fragments
{
    public class ConditionRule
    {
        public ConditionType Condition { get; set; }
        public List<long>? FragmentIds { get; set; }
        public List<ConditionRule>? Rules { get; set; }
        public int? Count { get; set; }

        public bool CheckConditions(long[] fragmentIds)
        {
            return Condition switch
            {
                ConditionType.Unknown => throw new YagoException("Незивестный тип условия"),

                ConditionType.AND => Rules!.TrueForAll(r => r.CheckConditions(fragmentIds)),
                ConditionType.OR => Rules!.Exists(r => r.CheckConditions(fragmentIds)),

                ConditionType.CountMoreThan => FragmentIds!.Count(f => fragmentIds.Contains(f)) > Count,
                ConditionType.CountLessThan => FragmentIds!.Count(f => fragmentIds.Contains(f)) < Count,

                ConditionType.ContainsAny => FragmentIds!.Exists(f => fragmentIds.Contains(f)),
                ConditionType.ContainsAll => FragmentIds!.TrueForAll(f => fragmentIds.Contains(f)),
                ConditionType.NotContainsAny => !FragmentIds!.Exists(f => fragmentIds.Contains(f)),

                _ => throw new YagoException("Незивестный тип условия"),
            };
        }
    }
}
