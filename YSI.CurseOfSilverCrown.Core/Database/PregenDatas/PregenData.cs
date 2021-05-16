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
            Started = DateTime.MinValue,
            IsActive = true
        };

        private static BaseDomain[] BaseDomains = new BaseDomain[]
        {
            new BaseDomain(1, "Сумеречная башня", new [] { 9, 4, 2 }),
            new BaseDomain(2, "Чёрный замок", new [] { 1, 4, 3 }),
            new BaseDomain(3, "Восточный дозор", new [] { 2, 4, 5 }),
            new BaseDomain(4, "Новый дар", new [] { 1, 2, 3, 7, 8 }),
            new BaseDomain(5, "Скагос", new [] { 3, 4, 7, 6 }),
            new BaseDomain(6, "Кархолд", new [] { 5, 7, 15 }),
            new BaseDomain(7, "Последний очаг", new [] { 8, 4, 5, 6, 15, 14 }),
            new BaseDomain(8, "Вершина", new [] { 9, 4, 7, 14, 13 }),
            new BaseDomain(9, "Каменный холм", new [] { 10, 1, 8, 13, 12 }),
            new BaseDomain(10, "Медвежий остров", new [] { 9, 12, 11 }),
            new BaseDomain(11, "Мыс морского дракона", new [] { 10, 12, 20, 21, 22 }),
            new BaseDomain(12, "Темнолесье", new [] { 11, 10, 9, 13, 20 }),
            new BaseDomain(13, "Железный холм", new [] { 9, 8, 14, 20, 12 }),
            new BaseDomain(14, "Винтерфелл", new [] { 8, 7, 15, 17, 18, 19, 20, 13 }),
            new BaseDomain(15, "Дредфорт", new [] { 7, 6, 16, 17, 14 }),
            new BaseDomain(16, "Вдовий дозор", new [] { 15, 17, 26 }),
            new BaseDomain(17, "Хорнвуд", new [] { 14,15, 16, 26, 25, 18 }),
            new BaseDomain(18, "Чёрная заводь", new [] { 14, 17, 25, 19 }),
            new BaseDomain(19, "Замок Сервинов", new [] { 14, 18, 25, 24, 21, 20 }),
            new BaseDomain(20, "Волчий лес", new [] { 12, 13, 14, 19, 21, 11 }),
            new BaseDomain(21, "Торхенов удел", new [] { 20, 19, 24, 23, 22, 11 }),
            new BaseDomain(22, "Каменный берег", new [] { 11, 21, 23 }),
            new BaseDomain(23, "Родники", new [] { 22, 21, 24, 30 }),
            new BaseDomain(24, "Барроутон", new [] { 21, 19, 25, 28, 30, 23 }),
            new BaseDomain(25, "Белая гавань", new [] { 18, 17, 26, 27, 28, 24, 19 }),
            new BaseDomain(26, "Бараньи ворота", new [] { 17, 16, 27, 25 }),
            new BaseDomain(27, "Старый замок", new [] { 25, 26, 47, 46, 45, 29, 28 }),
            new BaseDomain(28, "Ров Кайлин", new [] { 25, 27, 29, 24 }),
            new BaseDomain(29, "Перешеек", new [] { 28, 27, 45, 66 }),
            new BaseDomain(30, "Кремневый палец", new [] { 23, 24, 67, 31 }),

            new BaseDomain(31, "Чёрная волна", new [] { 30, 67, 39, 38, 32 }),
            new BaseDomain(32, "Старый Вик", new [] { 31, 38, 37, 33 }),
            new BaseDomain(33, "Хаммерхорн", new [] { 32, 37, 36, 35, 34 }),
            new BaseDomain(34, "Одинокий светоч", new [] { 33 }),
            new BaseDomain(35, "Солёный утёс", new [] { 32, 36, 43 }),
            new BaseDomain(36, "Гольцы", new [] { 33, 37, 42, 43, 35 }),
            new BaseDomain(37, "Пебблтон", new [] { 32, 38, 39, 41, 36, 33 }),
            new BaseDomain(38, "Оркмонт", new [] { 31, 39, 37, 32 }),
            new BaseDomain(39, "Десять башен", new [] { 31, 67, 68, 70, 85, 86, 44, 40, 41, 37, 38 }),
            new BaseDomain(40, "Камнедрев", new [] { 39, 44, 42, 41 }),
            new BaseDomain(41, "Железная роща", new [] { 37, 39, 40, 42 }),
            new BaseDomain(42, "Лордпорт", new [] { 41, 40, 44, 43, 36 }),
            new BaseDomain(43, "Пайк", new [] { 36, 42, 86, 35 }),
            new BaseDomain(44, "Волмарк", new [] { 40, 39, 86, 42 }),

            new BaseDomain(45, "Длинная сестра", new [] { 29, 27, 46, 47, 66 }),
            new BaseDomain(46, "Милая сестра", new [] { 27, 47, 45 }),
            new BaseDomain(47, "Малая сестра", new [] { 46, 27, 48, 50, 66 }),
            new BaseDomain(48, "Галечный остров", new [] { 47, 50 }),
            new BaseDomain(49, "Сосцы", new [] { 50 }),
            new BaseDomain(50, "Персты", new [] { 66, 47, 48, 49, 54, 53, 51 }),
            new BaseDomain(51, "Ледяноый ручей", new [] { 50, 53 }),
            new BaseDomain(52, "Суровая песнь", new [] { 54, 59 }),
            new BaseDomain(53, "Змеиный лес", new [] { 51, 50, 54 }),
            new BaseDomain(54, "Дом сердец", new [] { 53, 50, 55, 59 }),
            new BaseDomain(55, "Длинный лук", new [] { 54, 56, 57, 58, 59 }),
            new BaseDomain(56, "Старый якорь", new [] { 55, 62, 57 }),
            new BaseDomain(57, "Девять звёзд", new [] { 55, 56, 62, 58 }),
            new BaseDomain(58, "Железная дубрава", new [] { 55, 57, 61, 60, 59 }),
            new BaseDomain(59, "Орлиное гнездо", new [] { 52, 54, 55, 58, 60, 75 }),
            new BaseDomain(60, "Редфорт", new [] { 59, 58, 61, 64 }),
            new BaseDomain(61, "Серая лощина", new [] { 58, 62, 64, 60 }),
            new BaseDomain(62, "Рунный камень", new [] { 57, 56, 63, 64, 61 }),
            new BaseDomain(63, "Ведьмин остров", new [] { 62, 64 }),
            new BaseDomain(64, "Чаячий город", new [] { 60, 61, 62, 63, 65 }), //КЗ!!!
            new BaseDomain(65, "Фитили", new [] { 64, 77, 76, 75 }), //КЗ!!!

            new BaseDomain(66, "Близнецы", new [] { 29, 45, 47, 50, 74, 69, 68, 67 }),
            new BaseDomain(67, "Сигард", new [] { 66, 68, 39, 31, 30 }),
            new BaseDomain(68, "Старые камни", new [] { 67, 66, 69, 70, 39 }),
            new BaseDomain(69, "Добрая ярмарка", new [] { 68, 66, 74, 70 }),
            new BaseDomain(70, "Вранодрев", new [] { 68, 69, 74, 73, 72, 71, 85, 39 }),
            new BaseDomain(71, "Риверран", new [] { 70, 72, 80, 81, 89, 88, 85 }),
            new BaseDomain(72, "Каменный оплот", new [] { 70, 73, 79, 80, 71 }),
            new BaseDomain(73, "Замок Личестеров", new [] { 70, 74, 78, 79, 72 }),
            new BaseDomain(74, "Город Харровея", new [] { 66, 75, 78, 73, 70, 69 }),
            new BaseDomain(75, "Дарри", new [] { 74, 59, 65, 76, 77, 78 }),
            new BaseDomain(76, "Солеварни", new [] { 75, 65, 77 }), //КЗ!!!
            new BaseDomain(77, "Девичий пруд", new [] { 74, 76, 78 }), //КЗ!!!
            new BaseDomain(78, "Харренхол", new [] { 73, 74, 75, 77, 84, 83, 79 }), //КЗ!!!
            new BaseDomain(79, "Жёлудь", new [] { 80, 72, 73, 78, 83, 82 }),
            new BaseDomain(80, "Атранта", new [] { 71, 72, 79, 82, 81 }),
            new BaseDomain(81, "Розовая дева", new [] { 71, 80, 82, 84, 96, 95, 89 }),
            new BaseDomain(82, "Приют странника", new [] { 80, 79, 83, 84, 81 }),
            new BaseDomain(83, "Каслвуд", new [] { 79, 78, 84, 82 }),
            new BaseDomain(84, "Каменная септа", new [] { 82, 83, 78, 108, 97, 96, 81 }), //КЗ!!!

            new BaseDomain(85, "Виндхолл", new [] { 39, 70, 71, 88, 86 }),
            new BaseDomain(86, "Гибельная крепость", new [] { 43, 42, 44, 39, 85, 88, 87 }),
            new BaseDomain(87, "Скала", new [] { 86, 88, 91, 92 }),
            new BaseDomain(88, "Кастамере", new [] { 86, 85, 71, 89, 90, 91 }),
            new BaseDomain(89, "Золотой зуб", new [] { 88, 71, 81, 95, 90 }),
            new BaseDomain(90, "Эшмарк", new [] { 91, 88, 89, 95, 94 }),
            new BaseDomain(91, "Тарбекхолл", new [] { 87, 88, 90, 94, 92 }),
            new BaseDomain(92, "Светлый остров", new [] { 87, 91, 93 }),
            new BaseDomain(93, "Пиршественные огни", new [] { 92, 91, 94, 100, 101, 105 }),
            new BaseDomain(94, "Сарсфилд", new [] { 93, 91, 90, 95, 100 }),
            new BaseDomain(95, "Хорнваль", new [] { 94, 90, 89, 81, 96, 100 }),
            new BaseDomain(96, "Глубокая нора", new [] { 95, 81, 84, 97, 98, 99, 100 }),
            new BaseDomain(97, "Ключи", new [] { 96, 84, 108, 107, 98 }), //Простор
            new BaseDomain(98, "Серебрянный холм", new [] { 96, 97, 107, 106, 101, 99 }),
            new BaseDomain(99, "Замок Клиганов", new [] { 100, 96, 98, 101 }),
            new BaseDomain(100, "Утёс Кастерли", new [] { 93, 94, 95, 96, 99, 101 }),
            new BaseDomain(101, "Ланниспорт", new [] { 100, 99, 98, 106, 105, 93 }),
            new BaseDomain(105, "Крейкхолл", new [] { 93, 101, 106 }), //Простор
            new BaseDomain(106, "Корнфилд", new [] { 105, 101, 98, 107 }), //Простор
            new BaseDomain(107, "Гринфилд", new [] { 98, 97, 106 }), //Простор
            new BaseDomain(108, "Золотая дорога", new [] { 97, 84 }) //Простор, КЗ
        };

        public static Domain[] Organizations =>
            BaseDomains
                .Select(p => new Domain
                {
                    Id = p.Id,
                    Name = p.Name,
                    TurnOfDefeat = int.MinValue,
                    Warriors = RandomHelper.AddRandom(WarriorParameters.StartCount, randomNumber: (p.Id * p.Id) % 10 / 10.0),
                    Coffers = RandomHelper.AddRandom(CoffersParameters.StartCount, randomNumber: ((p.Id + 1) * p.Id) % 10 / 10.0, roundRequest: -1),
                    Fortifications = RandomHelper.AddRandom(FortificationsParameters.StartCount, randomNumber: ((p.Id + 2) * p.Id) % 10 / 10.0, roundRequest: -1)
                })
                .ToArray();

        public static Unit[] Units =>
            BaseDomains
                .Select(p => new Unit
                {
                    DomainId = p.Id,
                    PositionDomainId = p.Id,
                    Warriors = RandomHelper.AddRandom(WarriorParameters.StartCount, randomNumber: (p.Id * p.Id) % 10 / 10.0),
                    Type = enArmyCommandType.WarSupportDefense,
                    TargetDomainId = p.Id,
                    InitiatorDomainId = p.Id,
                    Status = enCommandStatus.ReadyToRun
                })
                .ToArray();

        internal static Turn GetFirstTurn()
        {
            return firstTurn;
        }

        internal static Route[] Routes =>        
            BaseDomains
                .SelectMany(b => b.RoutesToDomains
                    .Select(r => new Route
                    {
                        FromDomainId = b.Id,
                        ToDomainId = r
                    }))
                .ToArray();        

        private class BaseDomain
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int[] RoutesToDomains { get; set; }


            public BaseDomain(int id, string name, int[] routesToDomains)
            {
                Id = id;
                Name = name;
                RoutesToDomains = routesToDomains;
            }
        }
    }
}
