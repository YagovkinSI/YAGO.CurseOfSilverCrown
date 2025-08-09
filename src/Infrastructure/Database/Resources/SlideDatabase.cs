using System.Collections.Generic;
using YAGO.World.Domain.Slides;

namespace YAGO.World.Infrastructure.Database.Resources
{
    public static class SlideDatabase
    {
        public static Dictionary<long, Slide> Slides { get; } = new()
        {
            [0] = new(0, "Продолжение следует... Ожидайте обновлений.", "home"),

            //Глава 1. Обычное поручение
            [1] = new(1, StoryResources.Slide_00001, "UpperTown"),
            [2] = new(2, StoryResources.Slide_00002, "UpperTownResidents"),
            [3] = new(3, StoryResources.Slide_00003, "EirusTemple"),
            [4] = new(4, StoryResources.Slide_00004, "Market"),
            [5] = new(5, StoryResources.Slide_00005, "Prophet"),
            [6] = new(6, StoryResources.Slide_00006, "WorriedPeople"),

            [7] = new(7, StoryResources.Slide_00007, "Prophet"),
            [8] = new(8, StoryResources.Slide_00008, "PictureDemonShip"),
            [9] = new(9, StoryResources.Slide_00009, "PictureDemonElf"),
            [10] = new(10, StoryResources.Slide_00010, "PitureVulcano"),
            [11] = new(11, StoryResources.Slide_00011, "Prophet"),

            [12] = new(12, StoryResources.Slide_00012, "Market"),
            [13] = new(13, StoryResources.Slide_00013, "Haruf"),

            [14] = new(14, StoryResources.Slide_00014, "Haruf"),
            [15] = new(15, StoryResources.Slide_00015, "CandyMerchant"),
            [16] = new(16, StoryResources.Slide_00016, "CandyMerchant"),

            [17] = new(17, StoryResources.Slide_00017, "CandyMerchant"),
            [18] = new(18, StoryResources.Slide_00018, "CandyMerchant"),
            [19] = new(19, StoryResources.Slide_00019, "CandyMerchant"),

            [20] = new(20, StoryResources.Slide_00020, "CandyMerchant"),
            [21] = new(21, StoryResources.Slide_00021, "CandyMerchant"),
            [22] = new(22, StoryResources.Slide_00022, "CandyMerchant"),
            [23] = new(23, StoryResources.Slide_00023, "CandyMerchant"),

            [24] = new(24, StoryResources.Slide_00024, "UpperTown"),
            [25] = new(25, StoryResources.Slide_00025, "Lira"),
            [26] = new(26, StoryResources.Slide_00026, "Iltarin"),

            [27] = new(27, "\"Я... пожалуй, пойду\" - пробормотал Дари, поняв что от него хочет торговец и побежал дальше.", "CandyMerchant"),
            [28] = new(28, "Дари решил не отвлекаться на проповедника.", "Prophet"),
            [29] = new(29, "Дари посчитал слова проповедника бредом и был разочарован тем как много людей верили ему.", "Prophet"),
            [30] = new(30, "Теперь Дари понял, почему пробудился вулкан. Это всё магия эльниров!", "Prophet"),
            [31] = new(31, "Слова проповедника были интересны. Действительно ли магия эльниров вызвала гнев богов?", "Prophet"),
            [32] = new(32, "\"Шафран, корица и... кажется, сушёные лимоны\" - неуверенно пробормотал Дари.", "Haruf"),
            [33] = new(33, "\"Куркума, тмин и сушёные лимоны\" - неуверенно пробормотал Дари.", "Haruf"),
            [34] = new(34, "\"Шафран, кардамон и сушёные апельсины\" - неуверенно пробормотал Дари.", "Haruf"),
            [35] = new(35, "Дари с удовольствием принял угощение. Не часто ему приходилось есть такие сладости.", "CandyMerchant"),
            [36] = new(36, "Дари отказался от предложения. Он не привык к таким дарам от незнакомцев.", "CandyMerchant"),
            [37] = new(37, "Дари недоверчиво посмотрел на торговца и сделал шаг назад.", "CandyMerchant"),
            [38] = new(38, "Дари недоверчиво посмотрел на торговца и побежал дальше по своим делам, даже не подойдя к нему.", "CandyMerchant"),
            [39] = new(39, "\"Хорошо, я что-нибудь принесу\" - немного подумав произнёс Дари.", "CandyMerchant"),
            [40] = new(40, "\"Я не буду заниматься воровством!\" - резко ответил догадливый Дари.", "CandyMerchant"),

            [41] = new(41, StoryResources.Slide_00041, "Vulcano"),
            [42] = new(42, StoryResources.Slide_00042, "PearlHarborRuins"),
            [43] = new(43, StoryResources.Slide_00043, "UpperTown"),
        };
    }
}
