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
            [25] = new(25, StoryResources.Slide_00025, "UpperTown"),
            [26] = new(26, StoryResources.Slide_00026, "Haruf"),

            [27] = new(23, StoryResources.Slide_00027, "CandyMerchant"),
            [28] = new(28, StoryResources.Slide_00028, "Prophet"),
            [29] = new(29, StoryResources.Slide_00029, "Prophet"),
            [30] = new(30, StoryResources.Slide_00030, "Prophet"),
            [31] = new(31, StoryResources.Slide_00031, "Prophet"),
            [32] = new(32, StoryResources.Slide_00032, "Haruf"),
            [33] = new(33, StoryResources.Slide_00033, "Haruf"),
            [34] = new(34, StoryResources.Slide_00034, "Haruf"),
            [35] = new(35, StoryResources.Slide_00035, "Haruf"),
            [36] = new(36, StoryResources.Slide_00036, "Haruf"),
            [37] = new(37, StoryResources.Slide_00037, "Haruf"),
            [38] = new(38, StoryResources.Slide_00038, "Haruf"),
            [39] = new(39, StoryResources.Slide_00039, "Haruf"),

            [40] = new(40, StoryResources.Slide_00040, "Vulcano"),
            [41] = new(41, StoryResources.Slide_00041, "Vulcano"),
            [42] = new(42, StoryResources.Slide_00042, "PearlHarborRuins"),
            [43] = new(43, StoryResources.Slide_00043, "UpperTown"),

            [44] = new(44, StoryResources.Slide_00044, "CandyMerchant"),
            [45] = new(45, StoryResources.Slide_00045, "CandyMerchant"),
            [46] = new(46, StoryResources.Slide_00046, "Lira"),

            [47] = new(47, StoryResources.Slide_00047, "PearlHarborRuins"),
            [48] = new(48, StoryResources.Slide_00048, "PearlHarborRuins"),
            [49] = new(49, StoryResources.Slide_00049, "PearlHarborRuins"),

            [50] = new(50, StoryResources.Slide_00050, "UpperTown"),
            [51] = new(51, StoryResources.Slide_00051, "Lira"),
            [52] = new(52, StoryResources.Slide_00052, "UpperTown"),
            [53] = new(53, StoryResources.Slide_00053, "Rok"),

            [54] = new(54, StoryResources.Slide_00054, "UpperTownResidents"),
            [55] = new(55, StoryResources.Slide_00055, "UpperTown"),
            [56] = new(56, StoryResources.Slide_00056, "UpperTownResidents"),
            [57] = new(57, StoryResources.Slide_00057, "Rok"),
            [58] = new(58, StoryResources.Slide_00058, "Rok"),
            [59] = new(59, StoryResources.Slide_00059, "Rok"),
            [60] = new(60, StoryResources.Slide_00060, "Rok"),
            [61] = new(61, StoryResources.Slide_00061, "Rok"),
            [62] = new(62, StoryResources.Slide_00062, "Rok"),
        };
    }
}
