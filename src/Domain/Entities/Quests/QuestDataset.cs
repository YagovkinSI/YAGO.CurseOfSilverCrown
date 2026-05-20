using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Entities.GameEvents.Dataset.Prologue;

namespace YAGO.World.Domain.Entities.Quests
{
    public static class QuestDataset
    {
        public static IReadOnlyList<Quest> All => [
            ColonyNameQuest.Get(),
            WhoAmI(),
            OpenColony(),
            Camilla()];

        public static Quest Get(string id) => All.Single(x => x.Id == id);

        private static Quest WhoAmI()
        {
            var id = nameof(WhoAmI);
            var name = "Приглашение на пост правителя";
            return new(
                id,
                name,
                QuestType.Completed,
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: [],
                new PrologueSlide(
                    name,
                    ImageSet.Feature,
                    ["Ура. Вы получили пост правителя."],
                    parameters: [],
                    continueButtonName: "Согласиться"),
                new Episode(
                    id.ToString(),
                    name,
                    prologSlides: [
                        new PrologueSlide(
                            title: name,
                            imageName: ImageSet.Feature,
                            text: ["Молодец"],
                            parameters: [],
                            continueButtonName: "Готово")],
                    dilemma: null));
        }

        private static Quest OpenColony()
        {
            var id = nameof(OpenColony);
            var name = "Первая станция";
            return new Quest(
                id,
                name,
                QuestType.Required,
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: [],
                new PrologueSlide(
                    name,
                    ImageSet.Feature,
                    ["Станция ещё строится. Придётся подождать."],
                    parameters: [
                        new KeyValueParameter(ColonyStatNames.EpisodeCount, 12)],
                    continueButtonName: "Согласиться"),
                new Episode(
                    id.ToString(),
                    name,
                    prologSlides: [
                        new PrologueSlide(
                            title: name,
                            imageName: ImageSet.Feature,
                            text: ["Молодец"],
                            parameters: [],
                            continueButtonName: "Готово")],
                    dilemma: null));
        }

        private static Quest Camilla()
        {
            var id = nameof(Camilla);
            var name = "Найм основного советника";
            return new Quest(
                id,
                name,
                QuestType.Completed,
                requirements: [],
                chanceDefault: 0,
                chanceModifiers: [],
                new PrologueSlide(
                    name,
                    ImageSet.Feature,
                    ["Одному не справиться."],
                    parameters: [
                        new KeyValueParameter(ColonyStatNames.ActionPoints_Resourses, 1)],
                    continueButtonName: "Найти опытного советника"),
                    new Episode(
                        id.ToString(),
                        name,
                        prologSlides: [
                            new PrologueSlide(
                                title: name,
                                imageName: ImageSet.Camilla,
                                text: [
                                    "Вы нашли опытного советника в команду. Камилла Селезнёва.",
                                    "Она будет помогать вам в решении проблем."],
                                parameters: [
                                    new KeyValueParameter(ColonyStatNames.ActionPoints_Resourses, -1),
                                    new KeyValueParameter(ColonyStatNames.ActionPoints_Trend, 1)],
                                continueButtonName: "Готово")],
                        dilemma: null));
        }
    }
}
