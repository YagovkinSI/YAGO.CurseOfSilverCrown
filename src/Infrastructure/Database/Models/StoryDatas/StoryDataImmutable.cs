using System.Collections.Generic;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Infrastructure.Database.Models.StoryDatas
{
    public class StoryDataImmutable
    {
        public List<long> FragmentIds { get; set; }

        public StoryDataImmutable(
            List<long> fragmentIds)
        {
            FragmentIds = fragmentIds ?? new List<long>();
        }

        public static StoryDataImmutable New =>
            new StoryDataImmutable(new List<long>() { 1 });

        public void AddFragment(long fragmentId)
        {
            if (FragmentIds == null)
                FragmentIds = new List<long>();

            if (FragmentIds.Contains(fragmentId))
                throw new YagoException("Данный фрагмент уже есть в истории.");
            else
                FragmentIds.Add(fragmentId);
        }
    }
}
