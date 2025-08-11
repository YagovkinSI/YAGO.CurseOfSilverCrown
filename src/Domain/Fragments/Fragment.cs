using System;
using System.Data;
using YAGO.World.Domain.Fragments.Enums;
using YAGO.World.Domain.Slides;

namespace YAGO.World.Domain.Fragments
{
    public class Fragment
    {
        public long Id { get; }
        public string ChoiceText { get; }
        public Slide[] Slides { get; }
        public long[] NextFragmentIds { get; }
        public ConditionRule? Requirements { get; }

        public Fragment(
            long id,
            string choiceText,
            Slide[] slides,
            long[] nextFragmentIds,
            ConditionRule? requirements = null)
        {
            Id = id;
            ChoiceText = choiceText;
            Slides = slides;
            NextFragmentIds = nextFragmentIds;
            Requirements = requirements;
        }

        public bool CheckConditions(long[] fragmentIds)
        {
            return Requirements?.CheckConditions(fragmentIds) ?? true;
        }
    }
}
