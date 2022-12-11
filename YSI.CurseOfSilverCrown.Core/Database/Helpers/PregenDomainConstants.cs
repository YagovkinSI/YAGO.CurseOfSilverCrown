namespace YSI.CurseOfSilverCrown.Core.Database.Helpers
{
    internal static class PregenDomainConstants
    {
        public static PregenDomainModel[] Array = new[]
        {
            new PregenDomainModel(1, 1010, "Сумеречная башня", new [] { 9, 4, 2 }),
            new PregenDomainModel(2, 1510, "Чёрный замок", new [] { 1, 4, 3 }),
            new PregenDomainModel(3, 1310, "Восточный дозор", new [] { 2, 4, 5 }),
            new PregenDomainModel(4, 3010, "Новый дар", new [] { 1, 2, 3, 7, 8 }),

            new PregenDomainModel(5, 4010, "Скагос", new [] { 3, 4, 7, 6 }),
            new PregenDomainModel(6, 5510, "Кархолд", new [] { 5, 7, 15 }),
            new PregenDomainModel(7, 5010, "Последний очаг", new [] { 8, 4, 5, 6, 15, 14 }),
            new PregenDomainModel(8, 3510, "Вершина", new [] { 9, 4, 7, 14, 13 }),
            new PregenDomainModel(9, 3310, "Каменный холм", new [] { 10, 1, 8, 13, 12 }),
            new PregenDomainModel(10, 1810, "Медвежий остров", new [] { 9, 12, 11 }),
            new PregenDomainModel(11, 3810, "Мыс морского дракона", new [] { 10, 12, 20, 21, 22 }),
            new PregenDomainModel(12, 3710, "Темнолесье", new [] { 11, 10, 9, 13, 20 }),
            new PregenDomainModel(13, 2010, "Железный холм", new [] { 9, 8, 14, 20, 12 }),
            new PregenDomainModel(14, 5310, "Винтерфелл", new [] { 8, 7, 15, 17, 18, 19, 20, 13 }),
            new PregenDomainModel(15, 5210, "Дредфорт", new [] { 7, 6, 16, 17, 14 }),
            new PregenDomainModel(16, 4510, "Вдовий дозор", new [] { 15, 17, 26 }),
            new PregenDomainModel(17, 4810, "Хорнвуд", new [] { 14,15, 16, 26, 25, 18 }),
            new PregenDomainModel(18, 2510, "Чёрная заводь", new [] { 14, 17, 25, 19 }),
            new PregenDomainModel(19, 2810, "Замок Сервинов", new [] { 14, 18, 25, 24, 21, 20 }),
            new PregenDomainModel(20, 5810, "Волчий лес", new [] { 12, 13, 14, 19, 21, 11 }),
            new PregenDomainModel(21, 5910, "Торхенов удел", new [] { 20, 19, 24, 23, 22, 11 }),
            new PregenDomainModel(22, 4310, "Каменный берег", new [] { 11, 21, 23 }),
            new PregenDomainModel(23, 5410, "Родники", new [] { 22, 21, 24, 30 }),
            new PregenDomainModel(24, 5710, "Барроутон", new [] { 21, 19, 25, 28, 30, 23 }),
            new PregenDomainModel(25, 5610, "Белая гавань", new [] { 18, 17, 26, 27, 28, 24, 19 }),
            new PregenDomainModel(26, 3610, "Бараньи ворота", new [] { 17, 16, 27, 25 }),
            new PregenDomainModel(27, 3410, "Старый замок", new [] { 25, 26, 47, 46, 45, 29, 28 }),
            new PregenDomainModel(28, 4610, "Ров Кайлин", new [] { 25, 27, 29, 24 }),
            new PregenDomainModel(29, 6010, "Перешеек", new [] { 28, 27, 45, 66 }),
            new PregenDomainModel(30, 5110, "Кремневый палец", new [] { 23, 24, 67, 31 }),

            new PregenDomainModel(31, 1520, "Чёрная волна", new [] { 30, 67, 39, 38, 32 }),
            new PregenDomainModel(32, 1020, "Старый Вик", new [] { 31, 38, 37, 33 }),
            new PregenDomainModel(33, 3020, "Хаммерхорн", new [] { 32, 37, 36, 35, 34 }),
            new PregenDomainModel(34, 1820, "Одинокий светоч", new [] { 33 }),
            new PregenDomainModel(35, 1720, "Солёный утёс", new [] { 33, 36, 43 }),
            new PregenDomainModel(36, 1420, "Гольцы", new [] { 33, 37, 42, 43, 35 }),
            new PregenDomainModel(37, 1620, "Пебблтон", new [] { 32, 38, 39, 41, 36, 33 }),
            new PregenDomainModel(38, 2020, "Оркмонт", new [] { 31, 39, 37, 32 }),
            new PregenDomainModel(39, 3520, "Десять башен", new [] { 31, 67, 68, 70, 85, 86, 44, 40, 41, 37, 38 }),
            new PregenDomainModel(40, 0520, "Камнедрев", new [] { 39, 44, 42, 41 }),
            new PregenDomainModel(41, 0820, "Железная роща", new [] { 37, 39, 40, 42 }),
            new PregenDomainModel(42, 0320, "Лордпорт", new [] { 41, 40, 44, 86, 43, 36 }),
            new PregenDomainModel(43, 1320, "Пайк", new [] { 36, 42, 86, 35 }),
            new PregenDomainModel(44, 0620, "Волмарк", new [] { 40, 39, 86, 42 }),

            new PregenDomainModel(45, 1530, "Длинная сестра", new [] { 29, 27, 46, 47, 66 }),
            new PregenDomainModel(46, 0830, "Милая сестра", new [] { 27, 47, 45 }),
            new PregenDomainModel(47, 1030, "Малая сестра", new [] { 45, 46, 27, 48, 50, 66 }),
            new PregenDomainModel(48, 0530, "Галечный остров", new [] { 47, 50 }),
            new PregenDomainModel(49, 0630, "Сосцы", new [] { 50 }),
            new PregenDomainModel(50, 3030, "Персты", new [] { 66, 47, 48, 49, 54, 53, 51 }),
            new PregenDomainModel(51, 2030, "Ледяной ручей", new [] { 50, 53 }),
            new PregenDomainModel(52, 2530, "Суровая песнь", new [] { 54, 59 }),
            new PregenDomainModel(53, 2330, "Змеиный лес", new [] { 51, 50, 54 }),
            new PregenDomainModel(54, 3530, "Дом сердец", new [] { 52, 53, 50, 55, 59 }),
            new PregenDomainModel(55, 2830, "Длинный лук", new [] { 54, 56, 57, 58, 59 }),
            new PregenDomainModel(56, 1830, "Старый якорь", new [] { 55, 62, 57 }),
            new PregenDomainModel(57, 1730, "Девять звёзд", new [] { 55, 56, 62, 58 }),
            new PregenDomainModel(58, 1930, "Железная дубрава", new [] { 55, 57, 61, 60, 59 }),
            new PregenDomainModel(59, 3830, "Орлиное гнездо", new [] { 52, 54, 55, 58, 60, 75 }),
            new PregenDomainModel(60, 1630, "Редфорт", new [] { 59, 58, 61, 64 }),
            new PregenDomainModel(61, 0930, "Серая лощина", new [] { 58, 62, 64, 60 }),
            new PregenDomainModel(62, 1430, "Рунный камень", new [] { 57, 56, 63, 64, 61 }),
            new PregenDomainModel(63, 0330, "Ведьмин остров", new [] { 62, 64 }),
            new PregenDomainModel(64, 2130, "Чаячий город", new [] { 60, 61, 62, 63, 65 }), //КЗ!!!
            new PregenDomainModel(65, 3330, "Фитили", new [] { 64, 77, 76, 75 }), //КЗ!!!
            
            new PregenDomainModel(66, 5040, "Близнецы", new [] { 29, 45, 47, 50, 74, 69, 68, 67 }),
            new PregenDomainModel(67, 3040, "Сигард", new [] { 66, 68, 39, 31, 30 }),
            new PregenDomainModel(68, 3440, "Старые камни", new [] { 67, 66, 69, 70, 39 }),
            new PregenDomainModel(69, 2040, "Добрая ярмарка", new [] { 68, 66, 74, 70 }),
            new PregenDomainModel(70, 4040, "Вранодрев", new [] { 68, 69, 74, 73, 72, 71, 85, 39 }),
            new PregenDomainModel(71, 3540, "Риверран", new [] { 70, 72, 80, 81, 89, 88, 85 }),
            new PregenDomainModel(72, 2540, "Каменный оплот", new [] { 70, 73, 79, 80, 71 }),
            new PregenDomainModel(73, 2840, "Замок Личестеров", new [] { 70, 74, 78, 79, 72 }),
            new PregenDomainModel(74, 4540, "Город Харровея", new [] { 66, 75, 78, 73, 70, 69 }),
            new PregenDomainModel(75, 3840, "Дарри", new [] { 74, 59, 65, 76, 77, 78 }),
            new PregenDomainModel(76, 1040, "Солеварни", new [] { 75, 65, 77 }), //КЗ!!!
            new PregenDomainModel(77, 1540, "Девичий пруд", new [] { 74, 76, 78 }), //КЗ!!!
            new PregenDomainModel(78, 3340, "Харренхол", new [] { 73, 74, 75, 77, 84, 83, 79 }), //КЗ!!!
            new PregenDomainModel(79, 1840, "Жёлудь", new [] { 80, 72, 73, 78, 83, 82 }),
            new PregenDomainModel(80, 1340, "Атранта", new [] { 71, 72, 79, 82, 81 }),
            new PregenDomainModel(81, 2340, "Розовая дева", new [] { 71, 80, 82, 84, 96, 95, 89 }),
            new PregenDomainModel(82, 1740, "Приют странника", new [] { 80, 79, 83, 84, 81 }),
            new PregenDomainModel(83, 2640, "Каслвуд", new [] { 79, 78, 84, 82 }),
            new PregenDomainModel(84, 3740, "Каменная септа", new [] { 82, 83, 78, 108, 97, 96, 81 }), //КЗ!!!
            
            new PregenDomainModel(85, 2050, "Виндхолл", new [] { 39, 70, 71, 88, 86 }),
            new PregenDomainModel(86, 2550, "Гибельная крепость", new [] { 43, 42, 44, 39, 85, 88, 87 }),
            new PregenDomainModel(87, 1050, "Скала", new [] { 86, 88, 91, 92 }),
            new PregenDomainModel(88, 2850, "Кастамере", new [] { 86, 85, 71, 89, 90, 91, 87 }),
            new PregenDomainModel(89, 2350, "Золотой зуб", new [] { 88, 71, 81, 95, 90 }),
            new PregenDomainModel(90, 2750, "Эшмарк", new [] { 91, 88, 89, 95, 94 }),
            new PregenDomainModel(91, 1550, "Тарбекхолл", new [] { 87, 88, 90, 94, 92 }),
            new PregenDomainModel(92, 1850, "Светлый остров", new [] { 87, 91, 93 }),
            new PregenDomainModel(93, 3050, "Пиршественные огни", new [] { 92, 91, 94, 100, 101, 105 }),
            new PregenDomainModel(94, 1350, "Сарсфилд", new [] { 93, 91, 90, 95, 100 }),
            new PregenDomainModel(95, 2250, "Хорнваль", new [] { 94, 90, 89, 81, 96, 100 }),
            new PregenDomainModel(96, 3550, "Глубокая нора", new [] { 95, 81, 84, 97, 98, 99, 100 }),
            new PregenDomainModel(97, 2650, "Ключи", new [] { 96, 84, 108, 107, 98 }), //Простор
            new PregenDomainModel(98, 2150, "Серебрянный холм", new [] { 96, 97, 107, 106, 101, 99 }),
            new PregenDomainModel(99, 1650, "Замок Клиганов", new [] { 100, 96, 98, 101 }),
            new PregenDomainModel(100, 3850, "Утёс Кастерли", new [] { 93, 94, 95, 96, 99, 101 }),
            new PregenDomainModel(101, 3350, "Ланниспорт", new [] { 100, 99, 98, 106, 105, 93 }),
            new PregenDomainModel(105, 3950, "Крейкхолл", new [] { 93, 101, 106 }), //Простор
            new PregenDomainModel(106, 2950, "Корнфилд", new [] { 105, 101, 98, 107 }), //Простор
            new PregenDomainModel(107, 2450, "Гринфилд", new [] { 98, 97, 106 }), //Простор
            new PregenDomainModel(108, 2055, "Золотая дорога", new [] { 97, 84 }) //Простор, КЗ
        };
    }
}
