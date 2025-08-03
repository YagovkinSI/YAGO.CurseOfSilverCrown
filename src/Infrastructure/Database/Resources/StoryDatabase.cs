using System.Collections.Generic;
using YAGO.World.Domain.Story;

namespace YAGO.World.Infrastructure.Database.Resources
{
    public static class StoryDatabase
    {
        private static Fragment FragmentInProgress(long id, string choiceText)
        {
            return new Fragment
            (
                id,
                choiceText,
                new Slide[]
                {
                    new(0, "Продолжение следует... Ожидайте обновлений.", "home")
                },
                nextFragmentIds: new long[0]
            );
        }

        public static Dictionary<long, Fragment> Fragments { get; } = new()
        {
            [1] = new Fragment
            (
                id: 1,
                choiceText: string.Empty,
                new Slide[]
                {
                    new(0, StoryResources.StoryNode_0_0, "UpperTown"),
                    new(1, StoryResources.StoryNode_0_1, "UpperTownResidents"),
                    new(2, StoryResources.StoryNode_0_2, "EirusTemple"),
                    new(3, StoryResources.StoryNode_0_3, "Market"),
                    new(4, StoryResources.StoryNode_0_4, "Prophet"),
                    new(5, StoryResources.StoryNode_0_5, "WorriedPeople"),
                },
                nextFragmentIds: new long[] { 2 , 3 }
            ),

            [2] = new Fragment
            (
                id: 2,
                "Прислушаться к словам проповедника",
                new Slide[]
                {
                    new(0, StoryResources.StoryNode_10_0, "Prophet"),
                    new(1, StoryResources.StoryNode_10_1, "PictureDemonShip"),
                    new(2, StoryResources.StoryNode_10_2, "PictureDemonElf"),
                    new(3, StoryResources.StoryNode_10_3, "PitureVulcano"),
                    new(4, StoryResources.StoryNode_10_4, "Prophet"),
                },
                nextFragmentIds: new long[] { 4, 5, 6 }
            ),

            [3] = new Fragment
            (
                id: 3,
                "Протиснуться сквозь толпу к торговцам специями",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_20_0, "Market"),
                    new(1, StoryResources.StoryNode_20_1, "Haruf"),
                },
                nextFragmentIds: new long[] { 7, 8, 9 }
            ),

            [4] = new Fragment
            (
                id: 4,
                "[Мысли] Это бред! Эльниры принесли нам знания и порядок",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_20_0, "Market"),
                    new(1, StoryResources.StoryNode_20_1, "Haruf"),
                },
                nextFragmentIds: new long[] { 7, 8, 9 }
            ),

            [5] = new Fragment
            (
                id: 5,
                "[Мысли] Боги... Я действительно служу темным силам?",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_20_0, "Market"),
                    new(1, StoryResources.StoryNode_20_1, "Haruf"),
                },
                nextFragmentIds: new long[] { 7, 8, 9 }
            ),

            [6] = new Fragment
            (
                id: 6,
                "[Мысли] Нужно узнать больше, прежде чем судить",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_20_0, "Market"),
                    new(1, StoryResources.StoryNode_20_1, "Haruf"),
                },
                nextFragmentIds: new long[] { 7, 8, 9 }
            ),

            [7] = new Fragment
            (
                id: 7,
                "Шафран, корица и... кажется, сушёные лимоны",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_30_0, "Haruf"),
                    new(1, StoryResources.StoryNode_30_1, "CandyMerchant"),
                    new(2, StoryResources.StoryNode_30_2, "CandyMerchant"),
                },
                nextFragmentIds: new long[] { 10, 11, 12, 13 }
            ),

            [8] = new Fragment
            (
                id: 8,
                "Куркума, тмин и сушёные лимоны",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_30_0, "Haruf"),
                    new(1, StoryResources.StoryNode_30_1, "CandyMerchant"),
                    new(2, StoryResources.StoryNode_30_2, "CandyMerchant"),
                },
                nextFragmentIds: new long[] { 10, 11, 12, 13 }
            ),


            [9] = new Fragment
            (
                id: 9,
                "Шафран, кардамон и сушёные апельсины",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_30_0, "Haruf"),
                    new(1, StoryResources.StoryNode_30_1, "CandyMerchant"),
                    new(2, StoryResources.StoryNode_30_2, "CandyMerchant"),
                },
                nextFragmentIds: new long[] { 10, 11, 12, 13 }
            ),

            [10] = new Fragment
            (
                id: 10,
                "Принять угощение",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_40_0, "CandyMerchant"),
                },
                nextFragmentIds: new long[] { 14, 15, 16 }
            ),

            [11] = new Fragment
            (
                id: 11,
                "Вежливо отказаться",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_41_0, "CandyMerchant"),
                },
                nextFragmentIds: new long[] { 14, 15, 16 }
            ),

            [12] = new Fragment
            (
                id: 12,
                "Нахмуриться и отступить",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_42_0, "CandyMerchant"),
                },
                nextFragmentIds: new long[] { 14, 15, 16 }
            ),

            [13] = new Fragment
            (
                id: 13,
                "Быстро уйти",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_53_0, "CandyMerchant"),
                },
                nextFragmentIds: new long[] { 17 }
            ),

            [14] = new Fragment
            (
                id: 14,
                "Хорошо, я что-нибудь принесу",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_50_0, "CandyMerchant"),
                },
                nextFragmentIds: new long[] { 17 }
            ),

            [15] = new Fragment
            (
                id: 15,
                "Я не буду заниматься воровством!",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_51_0, "CandyMerchant"),
                },
                nextFragmentIds: new long[] { 17 }
            ),

            [16] = new Fragment
            (
                id: 16,
                "Я... пожалуй, пойду",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_52_0, "CandyMerchant"),
                },
                nextFragmentIds: new long[] { 17 }
            ),

            [17] = new Fragment
            (
                id: 17,
                "Далее",
                slides: new Slide[]
                {
                    new(0, StoryResources.StoryNode_60_0, "UpperTown"),
                    new(1, StoryResources.StoryNode_60_1, "Lira"),
                    new(2, StoryResources.StoryNode_60_2, "Iltarin")
                },
                nextFragmentIds: new long[] { 18 }
            ),

            [18] = FragmentInProgress(18, "Далее")
        };
    }
}
