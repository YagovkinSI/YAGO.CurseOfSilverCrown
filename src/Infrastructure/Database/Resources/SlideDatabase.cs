using System.Collections.Generic;
using YAGO.World.Domain.Story;

namespace YAGO.World.Infrastructure.Database.Resources
{
    public static class SlideDatabase
    {
        public static Dictionary<long, Slide> Slides { get; } = new()
        {
            [0] = new(0, "Продолжение следует... Ожидайте обновлений.", "home"),

            //Глава 1. Обычное поручение
            [1] = new(1, StoryResources.StoryNode_0_0, "UpperTown"),
            [2] = new(2, StoryResources.StoryNode_0_1, "UpperTownResidents"),
            [3] = new(3, StoryResources.StoryNode_0_2, "EirusTemple"),
            [4] = new(4, StoryResources.StoryNode_0_3, "Market"),
            [5] = new(5, StoryResources.StoryNode_0_4, "Prophet"),
            [6] = new(6, StoryResources.StoryNode_0_5, "WorriedPeople"),

            [7] = new(7, StoryResources.StoryNode_10_0, "Prophet"),
            [8] = new(8, StoryResources.StoryNode_10_1, "PictureDemonShip"),
            [9] = new(9, StoryResources.StoryNode_10_2, "PictureDemonElf"),
            [10] = new(10, StoryResources.StoryNode_10_3, "PitureVulcano"),
            [11] = new(11, StoryResources.StoryNode_10_4, "Prophet"),

            [12] = new(12, StoryResources.StoryNode_20_0, "Market"),
            [13] = new(13, StoryResources.StoryNode_20_1, "Haruf"),

            [14] = new(14, StoryResources.StoryNode_30_0, "Haruf"),
            [15] = new(15, StoryResources.StoryNode_30_1, "CandyMerchant"),
            [16] = new(16, StoryResources.StoryNode_30_2, "CandyMerchant"),

            [17] = new(17, StoryResources.StoryNode_40_0, "CandyMerchant"),
            [18] = new(18, StoryResources.StoryNode_41_0, "CandyMerchant"),
            [19] = new(19, StoryResources.StoryNode_42_0, "CandyMerchant"),

            [20] = new(20, StoryResources.StoryNode_53_0, "CandyMerchant"),
            [21] = new(21, StoryResources.StoryNode_50_0, "CandyMerchant"),
            [22] = new(22, StoryResources.StoryNode_51_0, "CandyMerchant"),
            [23] = new(23, StoryResources.StoryNode_52_0, "CandyMerchant"),

            [24] = new(24, StoryResources.StoryNode_60_0, "UpperTown"),
            [25] = new(25, StoryResources.StoryNode_60_1, "Lira"),
            [26] = new(26, StoryResources.StoryNode_60_2, "Iltarin"),

            [27] = new(27, "\"Я... пожалуй, пойду\" - проборматал Дари, поняв что от него хочет торговец и побежал дальше.", "CandyMerchant"),
            [28] = new(28, "Дари решил не отвлекаться на проповедника.", "Prophet"),
            [29] = new(29, "Дари посчитал слова праповедника бредом и был разочарован тем сколько много людей верили ему.", "Prophet"),
            [30] = new(30, "Теперь Дари понял почему пробудился вулкан. Это всё магия эльниров!", "Prophet"),
            [31] = new(31, "Слова проповедника были интересны. Дейвительно ли магия эльниров вызвала гнев богов?", "Prophet"),
            [32] = new(32, "\"Шафран, корица и... кажется, сушёные лимоны\" - неуверенно проборматал Дари.", "Haruf"),
            [33] = new(33, "\"Куркума, тмин и сушёные лимоны\" - неуверенно проборматал Дари.", "Haruf"),
            [34] = new(34, "\"Шафран, кардамон и сушёные апельсины\" - неуверенно проборматал Дари.", "Haruf"),
            [35] = new(35, "Дари с удовольствием принял угощение. Не часто ему приходилось есть такие сладости.", "CandyMerchant"),
            [36] = new(36, "Дари отказался от предложения. Он не привык к таким дарам от незнакомцев.", "CandyMerchant"),
            [37] = new(37, "Дари недоверчево посмотрел на торговца и сделал шаг назад.", "CandyMerchant"),
            [38] = new(38, "Дари недоверчево посмотрел на торговца и побежал дальше по своим делам, даже не подойдя к нему.", "CandyMerchant"),
            [39] = new(39, "\"Хорошо, я что-нибудь принесу\" - немного подумав произнём Дари.", "CandyMerchant"),
            [40] = new(40, "\"Я не буду заниматься воровством!\" - резко ответил догадливый Дари.", "CandyMerchant"),
        };
    }
}
