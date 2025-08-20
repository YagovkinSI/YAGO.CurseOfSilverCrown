using System.Collections.Generic;
using YAGO.World.Domain.Fragments;
using YAGO.World.Domain.Fragments.Enums;
using YAGO.World.Domain.Slides;

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
                    SlideDatabase.Slides[0]
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
                    SlideDatabase.Slides[1]
                },
                nextFragmentIds: new long[] { 19, 27, 20 }
            ),

            [2] = new Fragment
            (
                id: 2,
                "Прислушаться к словам проповедника",
                new Slide[]
                {
                    SlideDatabase.Slides[7],
                    SlideDatabase.Slides[8],
                    SlideDatabase.Slides[9],
                    SlideDatabase.Slides[10],
                    SlideDatabase.Slides[11]
                },
                nextFragmentIds: new long[] { 4, 5, 6 }
            ),

            [3] = new Fragment
            (
                id: 3,
                "Протиснуться сквозь толпу к торговцам специями",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[28],
                    SlideDatabase.Slides[12],
                    SlideDatabase.Slides[13],
                    SlideDatabase.Slides[38]
                },
                nextFragmentIds: new long[] { 25, 26 }
            ),

            [4] = new Fragment
            (
                id: 4,
                "[Мысли] Это бред! Эльниры принесли нам знания и порядок",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[29],
                    SlideDatabase.Slides[12],
                    SlideDatabase.Slides[13],
                    SlideDatabase.Slides[38]
                },
                nextFragmentIds: new long[] { 25, 26 }
            ),

            [5] = new Fragment
            (
                id: 5,
                "[Мысли] Боги... Я действительно служу темным силам?",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[30],
                    SlideDatabase.Slides[46],
                    SlideDatabase.Slides[12],
                    SlideDatabase.Slides[13],
                    SlideDatabase.Slides[38]
                },
                nextFragmentIds: new long[] { 25, 26 }
            ),

            [6] = new Fragment
            (
                id: 6,
                "[Мысли] Нужно узнать больше, прежде чем судить",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[31],
                    SlideDatabase.Slides[12],
                    SlideDatabase.Slides[13],
                    SlideDatabase.Slides[38]
                },
                nextFragmentIds: new long[] { 25, 26 }
            ),

            [7] = new Fragment
            (
                id: 7,
                "Сушёные лимоны",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[36]
                },
                nextFragmentIds: new long[] { 21, 22, 23, 24, 8, 9 },
                GetSpiceCondition(7)
            ),

            [8] = new Fragment
            (
                id: 8,
                "Корица",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[37]
                },
                nextFragmentIds: new long[] { 21, 22, 23, 24, 7, 9 },
                GetSpiceCondition(8)
            ),

            [9] = new Fragment
            (
                id: 9,
                "На этом всё",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[14],
                    SlideDatabase.Slides[26],
                    SlideDatabase.Slides[15],
                    SlideDatabase.Slides[16]
                },
                nextFragmentIds: new long[] { 10, 11, 12, 13 },
                new ConditionRule() { Condition = ConditionType.CountMoreThan, Count = 2, FragmentIds = new List<long> { 21, 22, 23, 24, 7, 8 } }
            ),

            [10] = new Fragment
            (
                id: 10,
                "Принять угощение",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[17],
                    SlideDatabase.Slides[27]
                },
                nextFragmentIds: new long[] { 14, 15, 16 }
            ),

            [11] = new Fragment
            (
                id: 11,
                "Вежливо отказаться",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[18],
                    SlideDatabase.Slides[27]
                },
                nextFragmentIds: new long[] { 14, 15, 16 }
            ),

            [12] = new Fragment
            (
                id: 12,
                "Нахмуриться и отступить",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[19],
                    SlideDatabase.Slides[27]
                },
                nextFragmentIds: new long[] { 14, 15, 16 }
            ),

            [13] = new Fragment
            (
                id: 13,
                "Быстро уйти",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[20]
                },
                nextFragmentIds: new long[] { 17 }
            ),

            [14] = new Fragment
            (
                id: 14,
                "Хорошо, я что-нибудь принесу",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[21],
                    SlideDatabase.Slides[44]
                },
                nextFragmentIds: new long[] { 17 }
            ),

            [15] = new Fragment
            (
                id: 15,
                "Я не буду заниматься воровством!",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[22],
                    SlideDatabase.Slides[45]
                },
                nextFragmentIds: new long[] { 17 }
            ),

            [16] = new Fragment
            (
                id: 16,
                "Я... пожалуй, пойду",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[23]
                },
                nextFragmentIds: new long[] { 17 }
            ),

            [17] = new Fragment
            (
                id: 17,
                "Далее",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[24],
                    SlideDatabase.Slides[25],
                },
                nextFragmentIds: new long[] { 18 }
            ),

            [18] = FragmentInProgress(18, "Далее"),

            [19] = new Fragment
            (
                id: 19,
                choiceText: "Продолжить путь к рынку",
                new Slide[]
                {
                    SlideDatabase.Slides[2]
                },
                nextFragmentIds: new long[] { 29, 28 }
            ),

            [20] = new Fragment
            (
                id: 20,
                choiceText: "Подробнее: Вулкан",
                new Slide[]
                {
                    SlideDatabase.Slides[40],
                    SlideDatabase.Slides[41],
                    SlideDatabase.Slides[42],
                    SlideDatabase.Slides[43],
                },
                nextFragmentIds: new long[] { 19, 27 },
                GetOnceCondition(20)
            ),

            [21] = new Fragment
            (
                id: 21,
                "Куркума",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[32]
                },
                nextFragmentIds: new long[] { 22, 23, 24, 7, 8, 9 },
                GetSpiceCondition(21)
            ),

            [22] = new Fragment
            (
                id: 22,
                "Шафран",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[33]
                },
                nextFragmentIds: new long[] { 21, 23, 24, 7, 8, 9 },
                GetSpiceCondition(22)
            ),

            [23] = new Fragment
            (
                id: 23,
                "Кардамон",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[34]
                },
                nextFragmentIds: new long[] { 21, 22, 24, 7, 8, 9 },
                GetSpiceCondition(23)
            ),

            [24] = new Fragment
            (
                id: 24,
                "Сушёные апельсины",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[35]
                },
                nextFragmentIds: new long[] { 21, 22, 23, 7, 8, 9 },
                GetSpiceCondition(24)
            ),

            [25] = new Fragment
            (
                id: 25,
                "Купить специи",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[39]
                },
                nextFragmentIds: new long[] { 21, 22, 23, 24, 7, 8, 9 }
            ),

            [26] = new Fragment
            (
                id: 26,
                choiceText: "Подробнее: Беженцы",
                new Slide[]
                {
                    SlideDatabase.Slides[47],
                    SlideDatabase.Slides[48],
                    SlideDatabase.Slides[49],
                },
                nextFragmentIds: new long[] { 25 }
            ),

            [27] = new Fragment
            (
                id: 27,
                choiceText: "Подробнее: Дари",
                new Slide[]
                {
                    SlideDatabase.Slides[50],
                    SlideDatabase.Slides[51],
                    SlideDatabase.Slides[52],
                },
                nextFragmentIds: new long[] { 19, 20 },
                GetOnceCondition(27)
            ),

            [28] = new Fragment
            (
                id: 28,
                choiceText: "Подробнее: Эльниры",
                new Slide[]
                {
                    SlideDatabase.Slides[54],
                    SlideDatabase.Slides[55],
                    SlideDatabase.Slides[56]
                },
                nextFragmentIds: new long[] { 29 }
            ),

            [29] = new Fragment
            (
                id: 29,
                choiceText: "Продолжить путь к рынку",
                new Slide[]
                {
                    SlideDatabase.Slides[3],
                    SlideDatabase.Slides[4],
                    SlideDatabase.Slides[5],
                    SlideDatabase.Slides[6]
                },
                nextFragmentIds: new long[] { 2, 3 }
            ),
        };

        private static ConditionRule GetOnceCondition(long id)
        {
            return new ConditionRule()
            {
                Condition = ConditionType.NotContainsAny,
                FragmentIds = new List<long> { id }
            };
        }

        private static ConditionRule GetSpiceCondition(long id)
        {
            return new ConditionRule()
            {
                Condition = ConditionType.AND,
                Rules = new List<ConditionRule>
                    {
                        new ConditionRule() { Condition = ConditionType.NotContainsAny, FragmentIds = new List<long> { id } },
                        new ConditionRule() { Condition = ConditionType.CountLessThan, FragmentIds = new List<long> { 21, 22, 23, 24, 7, 8 }, Count = 3 }
                    }
            };
        }
    }
}
