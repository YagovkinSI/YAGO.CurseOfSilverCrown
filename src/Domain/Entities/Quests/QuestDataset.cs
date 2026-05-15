using System;
using System.Collections.Generic;
using System.Linq;

namespace YAGO.World.Domain.Entities.Quests
{
    public static class QuestDataset
    {
        public static IReadOnlyList<Quest> All => [
            WhoAmI,
            OpenColony,
            Camilla
            ];

        public static Quest Get(Guid id) => All.Single(x => x.Id == id);

        private static Quest WhoAmI => new(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "Приглашение на пост правителя",
            QuestType.Completed);

        private static Quest OpenColony => new(
            Guid.Parse("00000000-0000-0000-0000-000000000002"),
            "Первая станция",
            QuestType.Required);

        private static Quest Camilla => new(
            Guid.Parse("00000000-0000-0000-0000-000000000003"),
            "Найм основного советника",
            QuestType.Completed);
    }
}
