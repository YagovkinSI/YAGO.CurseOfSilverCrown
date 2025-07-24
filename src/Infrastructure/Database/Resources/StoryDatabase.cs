using System.Collections.Generic;
using YAGO.World.Domain.Story;
using YAGO.World.Domain.Story.StoryEvents;

namespace YAGO.World.Infrastructure.Database.Resources
{
    public static class StoryDatabase
    {
        private static StoryNodeWithResults NodeInProgress(long id)
        {
            return new StoryNodeWithResults
            (
                id,
                "В разработке",
                new StoryCard[]
                {
                    new(
                        0,
                        "Продолжение следует... Ожидайте обновлений.",
                        "home"
                    )
                },
                new StoryChoiceWithResult[0]
            );
        }

        public static Dictionary<long, StoryNodeWithResults> Nodes { get; } = new()
        {
            [0] = new StoryNodeWithResults
            (
                id: 0,
                title: "Обычное поручение",
                cards: new StoryCard[]
                {
                    new(0, StoryResources.StoryNode_0_0, "UpperTown"),
                    new(1, StoryResources.StoryNode_0_1, "UpperTownResidents"),
                    new(2, StoryResources.StoryNode_0_2, "EirusTemple"),
                    new(3, StoryResources.StoryNode_0_3, "Market"),
                    new(4, StoryResources.StoryNode_0_4, "Prophet"),
                    new(5, StoryResources.StoryNode_0_5, "WorriedPeople"),
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(
                        1, "Прислушаться к словам проповедника", 1,
                        (data) => { data.Data.SetEvent(StoryDataEventsImmutable.ListenedToSermonAtMarket, true); }),
                    new(
                        2, "Протиснуться сквозь толпу к торговцам специями", 2,
                        (data) => { data.Data.SetEvent(StoryDataEventsImmutable.ListenedToSermonAtMarket, false); })
                }
            ),

            [1] = new StoryNodeWithResults
            (
                id: 1,
                title: "Обычное поручение",
                cards: new StoryCard[]
                {
                    new(0, StoryResources.StoryNode_1_0, "Prophet"),
                    new(1, StoryResources.StoryNode_1_1, "PictureDemonShip"),
                    new(2, StoryResources.StoryNode_1_2, "PictureDemonElf"),
                    new(3, StoryResources.StoryNode_1_3, "PitureVulcano"),
                    new(4, StoryResources.StoryNode_1_4, "Prophet"),
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(
                        1, "[Мысли] Это бред! Эльниры принесли нам знания и порядок", 2,
                        (data) => {
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.Elnirs, 100);
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.Magic, 60);
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.Tieflings, 10);
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.Dragons, 10);
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.PathOfLight, -100);
                        }),
                    new(
                        2, "[Мысли] Боги... Я действительно служу темным силам?", 2,
                        (data) => {
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.Elnirs, -50);
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.Magic, -50);
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.Tieflings, -50);
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.Dragons, -50);
                            data.Data.ChangePersonalOpinions(StoryDataPersonalOpinionsImmutable.PathOfLight, 30);
                        }),
                    new(
                        2, "[Мысли] Нужно узнать больше, прежде чем судить", 2,
                        (data) => { })
                }
            ),

            [2] = new StoryNodeWithResults
            (
                id: 2,
                title: "Обычное поручение",
                cards: new StoryCard[]
                {
                    new(0, StoryResources.StoryNode_2_0, "Market"),
                    new(1, StoryResources.StoryNode_2_1, "Haruf"),
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(
                        1, "Шафран, корица и... кажется, сушёные лимоны", 3,
                        (data) => { data.Data.SetEvent(StoryDataEventsImmutable.CompletedSimpleTaskCorrectly, true); }),
                    new(
                        2, "Куркума, тмин и сушёные лимоны", 3,
                        (data) => { data.Data.SetEvent(StoryDataEventsImmutable.CompletedSimpleTaskCorrectly, false); }),
                    new(
                        3, "Шафран, кардамон и сушёные апельсины", 3,
                        (data) => { data.Data.SetEvent(StoryDataEventsImmutable.CompletedSimpleTaskCorrectly, false); }),
                }
            ),

            [3] = new StoryNodeWithResults
            (
                id: 3,
                title: "Обычное поручение",
                cards: new StoryCard[]
                {
                    new(0, StoryResources.StoryNode_3_0, "Haruf"),
                    new(1, StoryResources.StoryNode_3_1, "CandyMerchant"),
                    new(2, StoryResources.StoryNode_3_2, "CandyMerchant"),
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(
                        1, "Принять угощение и поблагодарить", 4,
                        (data) => { data.Data.SetEvent(StoryDataEventsImmutable.CandyMerchantOffer, true); }),
                    new(
                        2, "Вежливо отказаться", 5,
                        (data) => { data.Data.SetEvent(StoryDataEventsImmutable.CandyMerchantOffer, true); }),
                    new(
                        3, "Нахмуриться и отступить", 6,
                        (data) => { data.Data.SetEvent(StoryDataEventsImmutable.CandyMerchantOffer, true); }),
                    new(
                        4, "Быстро уйти", 7,
                        (data) => { data.Data.SetEvent(StoryDataEventsImmutable.CandyMerchantOffer, false); }),
                }
            ),

            [4] = NodeInProgress(4),
            [5] = NodeInProgress(5),
            [6] = NodeInProgress(6),
            [7] = NodeInProgress(7),
        };
    }
}
