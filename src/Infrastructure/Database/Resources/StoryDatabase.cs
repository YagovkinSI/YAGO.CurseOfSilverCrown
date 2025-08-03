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
                    SlideDatabase.Slides[1],
                    SlideDatabase.Slides[2],
                    SlideDatabase.Slides[3],
                    SlideDatabase.Slides[4],
                    SlideDatabase.Slides[5],
                    SlideDatabase.Slides[6]
                },
                nextFragmentIds: new long[] { 2 , 3 }
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
                    SlideDatabase.Slides[13]
                },
                nextFragmentIds: new long[] { 7, 8, 9 }
            ),

            [4] = new Fragment
            (
                id: 4,
                "[Мысли] Это бред! Эльниры принесли нам знания и порядок",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[29],
                    SlideDatabase.Slides[12],
                    SlideDatabase.Slides[13]
                },
                nextFragmentIds: new long[] { 7, 8, 9 }
            ),

            [5] = new Fragment
            (
                id: 5,
                "[Мысли] Боги... Я действительно служу темным силам?",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[30],
                    SlideDatabase.Slides[12],
                    SlideDatabase.Slides[13]
                },
                nextFragmentIds: new long[] { 7, 8, 9 }
            ),

            [6] = new Fragment
            (
                id: 6,
                "[Мысли] Нужно узнать больше, прежде чем судить",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[31],
                    SlideDatabase.Slides[12],
                    SlideDatabase.Slides[13]
                },
                nextFragmentIds: new long[] { 7, 8, 9 }
            ),

            [7] = new Fragment
            (
                id: 7,
                "Шафран, корица и... кажется, сушёные лимоны",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[32],
                    SlideDatabase.Slides[14],
                    SlideDatabase.Slides[15],
                    SlideDatabase.Slides[16]
                },
                nextFragmentIds: new long[] { 10, 11, 12, 13 }
            ),

            [8] = new Fragment
            (
                id: 8,
                "Куркума, тмин и сушёные лимоны",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[33],
                    SlideDatabase.Slides[14],
                    SlideDatabase.Slides[15],
                    SlideDatabase.Slides[16]
                },
                nextFragmentIds: new long[] { 10, 11, 12, 13 }
            ),


            [9] = new Fragment
            (
                id: 9,
                "Шафран, кардамон и сушёные апельсины",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[34],
                    SlideDatabase.Slides[14],
                    SlideDatabase.Slides[15],
                    SlideDatabase.Slides[16]
                },
                nextFragmentIds: new long[] { 10, 11, 12, 13 }
            ),

            [10] = new Fragment
            (
                id: 10,
                "Принять угощение",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[35],
                    SlideDatabase.Slides[17]
                },
                nextFragmentIds: new long[] { 14, 15, 16 }
            ),

            [11] = new Fragment
            (
                id: 11,
                "Вежливо отказаться",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[36],
                    SlideDatabase.Slides[18]
                },
                nextFragmentIds: new long[] { 14, 15, 16 }
            ),

            [12] = new Fragment
            (
                id: 12,
                "Нахмуриться и отступить",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[37],
                    SlideDatabase.Slides[19]
                },
                nextFragmentIds: new long[] { 14, 15, 16 }
            ),

            [13] = new Fragment
            (
                id: 13,
                "Быстро уйти",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[38],
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
                    SlideDatabase.Slides[39],
                    SlideDatabase.Slides[21]
                },
                nextFragmentIds: new long[] { 17 }
            ),

            [15] = new Fragment
            (
                id: 15,
                "Я не буду заниматься воровством!",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[40],
                    SlideDatabase.Slides[22]
                },
                nextFragmentIds: new long[] { 17 }
            ),

            [16] = new Fragment
            (
                id: 16,
                "Я... пожалуй, пойду",
                slides: new Slide[]
                {
                    SlideDatabase.Slides[27],
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
                    SlideDatabase.Slides[26]
                },
                nextFragmentIds: new long[] { 18 }
            ),

            [18] = FragmentInProgress(18, "Далее")
        };
    }
}
