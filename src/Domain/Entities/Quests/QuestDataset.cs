using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;

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
            QuestType.Completed,
            new PrologueSlide(
                "Приглашение на пост правителя",
                ImageSet.Feature,
                ["Ура. Вы получили пост правителя."],
                parameters: [],
                continueButtonName: "Согласиться"));

        private static Quest OpenColony => new(
            Guid.Parse("00000000-0000-0000-0000-000000000002"),
            "Первая станция",
            QuestType.Required,
            new PrologueSlide(
                "Первая станция",
                ImageSet.Feature,
                ["Станция ещё строится. Придётся подождать."],
                parameters: [
                    new KeyValueParameter(ColonyStatNames.EpisodeCount, 12)],
                continueButtonName: "Согласиться"));

        private static Quest Camilla => new(
            Guid.Parse("00000000-0000-0000-0000-000000000003"),
            "Найм основного советника",
            QuestType.Completed,
            new PrologueSlide(
                "Найм основного советника",
                ImageSet.Feature,
                ["Одному не справиться."],
                parameters: [
                    new KeyValueParameter(ColonyStatNames.ActionPoints_Resourses, 1)],
                continueButtonName: "Найти опытного советника."));
    }
}
