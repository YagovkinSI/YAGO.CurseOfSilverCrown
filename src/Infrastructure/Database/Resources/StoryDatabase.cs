using System.Collections.Generic;
using YAGO.World.Domain.Story;

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
                new Slide[]
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
                slides: new Slide[]
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
                    new(1, "Прислушаться к словам проповедника", 10),
                    new(2, "Протиснуться сквозь толпу к торговцам специями", 20)
                }
            ),

            [10] = new StoryNodeWithResults
            (
                id: 10,
                title: "Обычное поручение",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_10_0, "Prophet"),
                    new(1, StoryResources.StoryNode_10_1, "PictureDemonShip"),
                    new(2, StoryResources.StoryNode_10_2, "PictureDemonElf"),
                    new(3, StoryResources.StoryNode_10_3, "PitureVulcano"),
                    new(4, StoryResources.StoryNode_10_4, "Prophet"),
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(1, "[Мысли] Это бред! Эльниры принесли нам знания и порядок", 20),
                    new(2, "[Мысли] Боги... Я действительно служу темным силам?", 20),
                    new(2, "[Мысли] Нужно узнать больше, прежде чем судить", 20)
                }
            ),

            [20] = new StoryNodeWithResults
            (
                id: 20,
                title: "Обычное поручение",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_20_0, "Market"),
                    new(1, StoryResources.StoryNode_20_1, "Haruf"),
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(1, "Шафран, корица и... кажется, сушёные лимоны", 30),
                    new(2, "Куркума, тмин и сушёные лимоны", 30),
                    new(3, "Шафран, кардамон и сушёные апельсины", 30),
                }
            ),

            [30] = new StoryNodeWithResults
            (
                id: 30,
                title: "Обычное поручение",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_30_0, "Haruf"),
                    new(1, StoryResources.StoryNode_30_1, "CandyMerchant"),
                    new(2, StoryResources.StoryNode_30_2, "CandyMerchant"),
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(1, "Принять угощение", 40),
                    new(2, "Вежливо отказаться", 41),
                    new(3, "Нахмуриться и отступить", 42),
                    new(4, "Быстро уйти", 53),
                }
            ),

            [40] = new StoryNodeWithResults
            (
                id: 40,
                title: "Обычное поручение",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_40_0, "CandyMerchant"),
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(1, "Хорошо, я что-нибудь принесу", 50),
                    new(2, "Я не буду заниматься воровством!", 51),
                    new(3, "Я... пожалуй, пойду", 52)
                }
            ),

            [41] = new StoryNodeWithResults
            (
                id: 41,
                title: "Обычное поручение",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_41_0, "CandyMerchant"),
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(1, "Хорошо, я что-нибудь принесу", 50),
                    new(2, "Я не буду заниматься воровством!", 51),
                    new(3, "Я... пожалуй, пойду", 52)
                }
            ),

            [42] = new StoryNodeWithResults
            (
                id: 42,
                title: "Обычное поручение",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_42_0, "CandyMerchant"),
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(1, "Хорошо, я что-нибудь принесу", 50),
                    new(2, "Я не буду заниматься воровством!", 51),
                    new(3, "Я... пожалуй, пойду", 52)
                }
            ),

            [50] = new StoryNodeWithResults
            (
                id: 50,
                title: "Обычное поручение",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_50_0, "CandyMerchant"),
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(1, "Далее", 60)
                }
            ),

            [51] = new StoryNodeWithResults
            (
                id: 51,
                title: "Обычное поручение",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_51_0, "CandyMerchant"),
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(1, "Далее", 60)
                }
            ),

            [52] = new StoryNodeWithResults
            (
                id: 52,
                title: "Обычное поручение",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_52_0, "CandyMerchant"),
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(1, "Далее", 60)
                }
            ),

            [53] = new StoryNodeWithResults
            (
                id: 53,
                title: "Обычное поручение",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_53_0, "CandyMerchant"),
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(1, "Далее", 60)
                }
            ),

            [60] = new StoryNodeWithResults
            (
                id: 60,
                title: "Обычное поручение",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_60_0, "UpperTown"),
                    new(1, StoryResources.StoryNode_60_1, "Lira"),
                    new(2, StoryResources.StoryNode_60_2, "Iltarin")
                },
                choices: new StoryChoiceWithResult[]
                {
                    new(1, "Далее", 70)
                }
            ),

            [70] = NodeInProgress(70)
        };
    }
}
