using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Database.PregenDatas
{
    internal static class PregenData
    {
        private static Turn firstTurn = new Turn 
        { 
            Id = 1,
            Started = DateTime.UtcNow,
            IsActive = true
        };

        private static BaseProvince[] BaseProvinces = new BaseProvince[]
        {
            new BaseProvince(1, "Сумеречная башня", "TwilightTower", new [] { 9, 4, 2 }),
            new BaseProvince(2, "Чёрный замок", "BlackCastle", new [] { 1, 4, 3 }),
            new BaseProvince(3, "Восточный дозор", "EasternWatch", new [] { 2, 4, 5 }),
            new BaseProvince(4, "Новый дар", "NewGift", new [] { 1, 2, 3, 7, 8 }),
            new BaseProvince(5, "Скагос", "Skagos", new [] { 3, 4, 7, 6 }),
            new BaseProvince(6, "Кархолд", "Carhold", new [] { 5, 7, 9 }),
            new BaseProvince(7, "Последний очаг", "TheLastHearth", new [] { 8, 4, 5, 6, 15, 14 }),
            new BaseProvince(8, "Вершина", "TheTop", new [] { 9, 4, 7, 14, 13 }),
            new BaseProvince(9, "Каменный холм", "StoneHill", new [] { 10, 1, 8, 13, 12 }),
            new BaseProvince(10, "Медвежий остров", "BearIsland", new [] { 9, 12, 11 }),
            new BaseProvince(11, "Мыс морского дракона", "SeaDragonCape", new [] { 10, 12, 20, 21, 22 }),
            new BaseProvince(12, "Темнолесье", "Darkshire", new [] { 11, 10, 9, 13, 20 }),
            new BaseProvince(13, "Железный холм", "IronHill", new [] { 9, 8, 14, 20, 12 }),
            new BaseProvince(14, "Винтерфелл", "MouthOfPolaima", new [] { 8, 7, 15, 17, 18, 19, 20, 13 }), //Winterfell
            new BaseProvince(15, "Дредфорт", "CapeRaptor", new [] { 7, 6, 16, 17, 14 }), //Dreadfort
            new BaseProvince(16, "Вдовий дозор", "WidowsWatch", new [] { 15, 17, 26 }),
            new BaseProvince(17, "Хорнвуд", "TinMines", new [] { 14,15, 16, 26, 25, 18 }), //Hornwood
            new BaseProvince(18, "Чёрная заводь", "BlackBackwater", new [] { 14, 17, 25, 19 }),
            new BaseProvince(19, "Замок Сервинов", "ServinCastle", new [] { 14, 18, 25, 24, 21, 20 }),
            new BaseProvince(20, "Волчий лес", "WolfForest", new [] { 12, 13, 14, 19, 21, 11 }),
            new BaseProvince(21, "Торхенов удел", "TorchensInheritance", new [] { 20, 19, 24, 23, 22, 11 }),
            new BaseProvince(22, "Каменный берег", "StoneCoast", new [] { 11, 21, 23 }),
            new BaseProvince(23, "Родники", "Springs", new [] { 22, 21, 24, 30 }),
            new BaseProvince(24, "Барроутон", "Barrowton", new [] { 21, 19, 25, 28, 30, 23 }),
            new BaseProvince(25, "Белая гавань", "WhiteHarbor", new [] { 18, 17, 26, 27, 28, 24, 19 }),
            new BaseProvince(26, "Бараньи ворота", "SheepsGate", new [] { 17, 16, 27, 25 }),
            new BaseProvince(27, "Старый замок", "OldCastle", new [] { 25, 26, 47, 46, 45, 29, 28 }),
            new BaseProvince(28, "Ров Кайлин", "KailinMoat", new [] { 25, 27, 29, 24 }),
            new BaseProvince(29, "Перешеек", "Isthmus", new [] { 28, 27, 45, 66 }),
            new BaseProvince(30, "Кремневый палец", "FlintFinger", new [] { 23, 24, 67, 31 }),

            new BaseProvince(31, "Чёрная волна", "BlackWave", new [] { 30, 67, 39, 38, 32 }),
            new BaseProvince(32, "Старый Вик", "OldVic", new [] { 31, 38, 37, 33 }),
            new BaseProvince(33, "Хаммерхорн", "Hammerhorn", new [] { 32, 37, 36, 35, 34 }),
            new BaseProvince(34, "Одинокий светоч", "LonelyBeacon", new [] { 33 }),
            new BaseProvince(35, "Солёный утёс", "SaltCliff", new [] { 32, 36, 43 }),
            new BaseProvince(36, "Гольцы", "Loaches", new [] { 33, 37, 42, 43, 35 }),
            new BaseProvince(37, "Пебблтон", "Pebbleton", new [] { 32, 38, 39, 41, 36, 33 }),
            new BaseProvince(38, "Оркмонт", "Orkmont", new [] { 31, 39, 37, 32 }),
            new BaseProvince(39, "Десять башен", "TenTowers", new [] { 31, 67, 68, 70, 85, 86, 44, 40, 41, 37, 38 }),
            new BaseProvince(40, "Камнедрев", "Stonewood", new [] { 39, 44, 42, 41 }),
            new BaseProvince(41, "Железная роща", "IronGrove", new [] { 37, 39, 40, 42 }),
            new BaseProvince(42, "Лордпорт", "Lordport", new [] { 41, 40, 44, 43, 36 }),
            new BaseProvince(43, "Пайк", "SummerCoast", new [] { 36, 42, 86, 35 }), //Pike
            new BaseProvince(44, "Волмарк", "Walmark", new [] { 40, 39, 86, 42 }),

            new BaseProvince(45, "Длинная сестра", "LongSister", new [] { 29, 27, 46, 47, 66 }),
            new BaseProvince(46, "Милая сестра", "SweetSister", new [] { 27, 47, 45 }),
            new BaseProvince(47, "Малая сестра", "LittleSister", new [] { 46, 27, 48, 50, 66 }),
            new BaseProvince(48, "Галечный остров", "PebbleIsland", new [] { 47, 50 }),
            new BaseProvince(49, "Сосцы", "Socks", new [] { 50 }),
            new BaseProvince(50, "Персты", "Fingers", new [] { 66, 47, 48, 49, 54, 53, 51 }),
            new BaseProvince(51, "Ледяноый ручей", "IceCreek", new [] { 50, 53 }),
            new BaseProvince(52, "Суровая песнь", "HarshSong", new [] { 54, 59 }),
            new BaseProvince(53, "Змеиный лес", "SnakeForest", new [] { 51, 50, 54 }),
            new BaseProvince(54, "Дом сердец", "HeartsHouse", new [] { 53, 50, 55, 59 }),
            new BaseProvince(55, "Длинный лук", "Longbow", new [] { 54, 56, 57, 58, 59 }),
            new BaseProvince(56, "Старый якорь", "OldAnchor", new [] { 55, 62, 57 }),
            new BaseProvince(57, "Девять звёзд", "NineStars", new [] { 55, 56, 62, 58 }),
            new BaseProvince(58, "Железная дубрава", "IronOakwood", new [] { 55, 57, 61, 60, 59 }),
            new BaseProvince(59, "Орлиное гнездо", "LimestoneRidges", new [] { 52, 54, 55, 58, 60, 75 }), //EaglesNest
            new BaseProvince(60, "Редфорт", "DimmoriaFarms", new [] { 59, 58, 61, 64 }), //Redfort
            new BaseProvince(61, "Серая лощина", "GrayHollow", new [] { 58, 62, 64, 60 }),
            new BaseProvince(62, "Рунный камень", "Runestone", new [] { 57, 56, 63, 64, 61 }),
            new BaseProvince(63, "Ведьмин остров", "WitchIsland", new [] { 62, 64 }),
            new BaseProvince(64, "Чаячий город", "TeaCity", new [] { 60, 61, 62, 63, 65 }), //КЗ!!!
            new BaseProvince(65, "Фитили", "Wicks", new [] { 64, 77, 76, 75 }), //КЗ!!!

            new BaseProvince(66, "Близнецы", "Gemini", new [] { 29, 45, 47, 50, 74, 69, 68, 67 }),
            new BaseProvince(67, "Сигард", "Seagard", new [] { 66, 68, 39, 31, 30 }),
            new BaseProvince(68, "Старые камни", "OlStones", new [] { 67, 66, 69, 70, 39 }),
            new BaseProvince(69, "Добрая ярмарка", "GoodFair", new [] { 68, 66, 74, 70 }),
            new BaseProvince(70, "Вранодрев", "HeatherOfDimmoria", new [] { 68, 69, 74, 73, 72, 71, 85, 39 }), //Vranodrev
            new BaseProvince(71, "Риверран", "DimmoriaValley", new [] { 70, 72, 80, 81, 89, 88, 85 }), //Riverrun
            new BaseProvince(72, "Каменный оплот", "StoneBulwark", new [] { 70, 73, 79, 80, 71 }),
            new BaseProvince(73, "Замок Личестеров", "LeicesterCastle", new [] { 70, 74, 78, 79, 72 }),
            new BaseProvince(74, "Город Харровея", "HarrowayCity", new [] { 66, 75, 78, 73, 70, 69 }),
            new BaseProvince(75, "Дарри", "Darry", new [] { 74, 59, 65, 76, 77, 78 }),
            new BaseProvince(76, "Солеварни", "Saltworks", new [] { 75, 65, 77 }), //КЗ!!!
            new BaseProvince(77, "Девичий пруд", "MaidenPond", new [] { 74, 76, 78 }), //КЗ!!!
            new BaseProvince(78, "Харренхол", "Harrenhal", new [] { 73, 74, 75, 77, 84, 83, 79 }), //КЗ!!!
            new BaseProvince(79, "Жёлудь", "Acorn", new [] { 80, 72, 73, 78, 83, 82 }),
            new BaseProvince(80, "Атранта", "Atranta", new [] { 71, 72, 79, 82, 81 }),
            new BaseProvince(81, "Розовая дева", "PinkMaiden", new [] { 71, 80, 82, 84, 96, 95, 89 }),
            new BaseProvince(82, "Приют странника", "WanderersRefuge", new [] { 80, 79, 83, 84, 81 }),
            new BaseProvince(83, "Каслвуд", "Castlewood", new [] { 79, 78, 84, 82 }),
            new BaseProvince(84, "Каменная септа", "StoneSepta", new [] { 82, 83, 78, 108, 97, 96, 81 }), //КЗ!!!

            new BaseProvince(85, "Виндхолл", "Windhall", new [] { 39, 70, 71, 88, 86 }),
            new BaseProvince(86, "Гибельная крепость", "BaneFortress", new [] { 43, 42, 44, 39, 85, 88, 87 }),
            new BaseProvince(87, "Скала", "Rock", new [] { 86, 88, 91, 92 }),
            new BaseProvince(88, "Кастамере", "Castamere", new [] { 86, 85, 71, 89, 90, 91 }),
            new BaseProvince(89, "Золотой зуб", "GoldenTooth", new [] { 88, 71, 81, 95, 90 }),
            new BaseProvince(90, "Эшмарк", "Ashmark", new [] { 91, 88, 89, 95, 94 }),
            new BaseProvince(91, "Тарбекхолл", "Tarbekhall", new [] { 87, 88, 90, 94, 92 }),
            new BaseProvince(92, "Светлый остров", "LightIsland", new [] { 87, 91, 93 }),
            new BaseProvince(93, "Пиршественные огни", "MemorableLights", new [] { 92, 91, 94, 100, 101, 105 }),
            new BaseProvince(94, "Сарсфилд", "Sarsfield", new [] { 93, 91, 90, 95, 100 }),
            new BaseProvince(95, "Хорнваль", "Hornval", new [] { 94, 90, 89, 81, 96, 100 }),
            new BaseProvince(96, "Глубокая нора", "DeepBurrow", new [] { 95, 81, 84, 97, 98, 99, 100 }),
            new BaseProvince(97, "Ключи", "Keys", new [] { 96, 84, 108, 107, 98 }), //Простор
            new BaseProvince(98, "Серебрянный холм", "SilverHill", new [] { 96, 97, 107, 106, 101, 99 }),
            new BaseProvince(99, "Замок Клиганов", "CleansCastle", new [] { 100, 96, 98, 101 }),
            new BaseProvince(100, "Утёс Кастерли", "ChalRocks", new [] { 93, 94, 95, 96, 99, 101 }), //CasterlyCliff
            new BaseProvince(101, "Ланниспорт", "Lannisport", new [] { 100, 99, 98, 106, 105, 93 }),
            new BaseProvince(105, "Крейкхолл", "Crackhall", new [] { 93, 101, 106 }), //Простор
            new BaseProvince(106, "Корнфилд", "Cornfield", new [] { 105, 101, 98, 107 }), //Простор
            new BaseProvince(107, "Гринфилд", "Greenfield", new [] { 98, 97, 106 }), //Простор
            new BaseProvince(108, "Золотая дорога", "GoldenRoad", new [] { 97, 84 }) //Простор, КЗ
        };

        public static Province[] Provinces =>
            BaseProvinces
                .Select(p => new Province
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToArray();

        public static Organization[] Organizations =>
            BaseProvinces
                .Select(p => new Organization
                {
                    Id = p.OrganizationId,
                    Name = p.Name,
                    OrganizationType = enOrganizationType.Lord,
                    ProvinceId = p.Id,
                    TurnOfDefeat = int.MinValue,
                    Warriors = 100, // RandomHelper.AddRandom(Constants.StartWarriors, randomNumber: (p.Id * p.Id) % 10 / 10.0),
                    Coffers = 4000, //RandomHelper.AddRandom(Constants.StartCoffers, randomNumber: ((p.Id + 1) * p.Id) % 10 / 10.0, roundRequest: -1)
                    Fortifications = RandomHelper.AddRandom(FortificationsParameters.StartCount, randomNumber: ((p.Id + 2) * p.Id) % 10 / 10.0, roundRequest: -1)
                })
                .ToArray();

        internal static Command[] Commands =>
            BaseProvinces
                .Select(p => new Command
                {
                    Id = Guid.NewGuid().ToString(),
                    OrganizationId = p.OrganizationId,
                    Type = enCommandType.Idleness
                })
                .ToArray();

        internal static Turn GetFirstTurn()
        {
            return firstTurn;
        }

        internal static Route[] Routes =>        
            BaseProvinces
                .SelectMany(b => b.RoutesToProvinces
                    .Select(r => new Route
                    {
                        FromProvinceId = b.Id,
                        ToProvinceId = r
                    }))
                .ToArray();        

        private class BaseProvince
        {
            public int Id { get; set; }
            public string OrganizationId { get; set; }
            public string Name { get; set; }
            public int[] RoutesToProvinces { get; set; }


            public BaseProvince(int id, string name, string organizationId, int[] routesToProvinces)
            {
                Id = id;
                OrganizationId = organizationId;
                Name = name;
                RoutesToProvinces = routesToProvinces;
            }
        }
    }
}
