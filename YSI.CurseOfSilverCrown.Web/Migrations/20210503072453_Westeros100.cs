using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class Westeros100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 4, 5 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 4, 6 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 5, 8 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 6, 4 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 7, 9 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 8, 5 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 9, 7 });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "Coffers", "Investments", "Name", "OrganizationType", "ProvinceId", "SuzerainId", "Warriors" },
                values: new object[,]
                {
                    { "TwilightTower", 4000, 0, "Сумеречная башня", 1, 1, null, 100 },
                    { "BlackCastle", 4000, 0, "Чёрный замок", 1, 2, null, 100 },
                    { "EasternWatch", 4000, 0, "Восточный дозор", 1, 3, null, 100 },
                    { "NewGift", 4000, 0, "Новый дар", 1, 4, null, 100 },
                    { "Skagos", 4000, 0, "Скагос", 1, 5, null, 100 },
                    { "Carhold", 4000, 0, "Кархолд", 1, 6, null, 100 },
                    { "TheLastHearth", 4000, 0, "Последний очаг", 1, 7, null, 100 },
                    { "TheTop", 4000, 0, "Вершина", 1, 8, null, 100 },
                    { "StoneHill", 4000, 0, "Каменный холм", 1, 9, null, 100 }
                });

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Сумеречная башня");

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Чёрный замок");

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Восточный дозор");

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Новый дар");

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Скагос");

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Кархолд");

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Последний очаг");

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Вершина");

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "Каменный холм");

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 81, "Розовая дева" },
                    { 80, "Атранта" },
                    { 79, "Жёлудь" },
                    { 78, "Харренхол" },
                    { 77, "Девичий пруд" },
                    { 76, "Солеварни" },
                    { 75, "Дарри" },
                    { 74, "Город Харровея" },
                    { 65, "Фитили" },
                    { 72, "Каменный оплот" },
                    { 71, "Риверран" },
                    { 70, "Вранодрев" },
                    { 69, "Добрая ярмарка" },
                    { 68, "Старые камни" },
                    { 67, "Сигард" },
                    { 66, "Близнецы" },
                    { 82, "Приют странника" },
                    { 64, "Чаячий город" },
                    { 73, "Замок Личестеров" },
                    { 83, "Каслвуд" },
                    { 92, "Светлый остров" },
                    { 85, "Виндхолл" },
                    { 108, "Золотая дорога" },
                    { 107, "Гринфилд" }
                });

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 106, "Корнфилд" },
                    { 105, "Крейкхолл" },
                    { 101, "Ланниспорт" },
                    { 100, "Утёс Кастерли" },
                    { 99, "Замок Клиганов" },
                    { 98, "Серебрянный холм" },
                    { 97, "Ключи" },
                    { 96, "Глубокая нора" },
                    { 95, "Хорнваль" },
                    { 94, "Сарсфилд" },
                    { 93, "Пиршественные огни" },
                    { 91, "Тарбекхолл" },
                    { 90, "Эшмарк" },
                    { 89, "Золотой зуб" },
                    { 88, "Кастамере" },
                    { 87, "Скала" },
                    { 86, "Гибельная крепость" },
                    { 84, "Каменная септа" },
                    { 62, "Рунный камень" },
                    { 63, "Ведьмин остров" },
                    { 60, "Редфорт" },
                    { 32, "Старый Вик" },
                    { 31, "Чёрная волна" },
                    { 30, "Кремневый палец" },
                    { 61, "Серая лощина" },
                    { 28, "Ров Кайлин" },
                    { 27, "Старый замок" },
                    { 26, "Бараньи ворота" },
                    { 25, "Белая гавань" },
                    { 24, "Барроутон" },
                    { 23, "Родники" },
                    { 22, "Каменный берег" },
                    { 21, "Торхенов удел" },
                    { 20, "Волчий лес" },
                    { 19, "Замок Сервинов" },
                    { 18, "Чёрная заводь" },
                    { 17, "Хорнвуд" },
                    { 16, "Вдовий дозор" },
                    { 15, "Дредфорт" },
                    { 14, "Винтерфелл" },
                    { 13, "Железный холм" },
                    { 12, "Темнолесье" }
                });

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 11, "Мыс морского дракона" },
                    { 10, "Медвежий остров" },
                    { 33, "Хаммерхорн" },
                    { 34, "Одинокий светоч" },
                    { 29, "Перешеек" },
                    { 36, "Гольцы" },
                    { 53, "Змеиный лес" },
                    { 54, "Дом сердец" },
                    { 55, "Длинный лук" },
                    { 56, "Старый якорь" },
                    { 57, "Девять звёзд" },
                    { 35, "Солёный утёс" },
                    { 58, "Железная дубрава" },
                    { 59, "Орлиное гнездо" },
                    { 51, "Ледяноый ручей" },
                    { 50, "Персты" },
                    { 49, "Сосцы" },
                    { 48, "Галечный остров" },
                    { 47, "Малая сестра" },
                    { 46, "Милая сестра" },
                    { 45, "Длинная сестра" },
                    { 44, "Волмарк" },
                    { 43, "Пайк" },
                    { 42, "Лордпорт" },
                    { 41, "Железная роща" },
                    { 40, "Камнедрев" },
                    { 39, "Десять башен" },
                    { 38, "Оркмонт" },
                    { 37, "Пебблтон" },
                    { 52, "Суровая песнь" }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[,]
                {
                    { 9, 1 },
                    { 3, 5 },
                    { 7, 4 },
                    { 6, 9 },
                    { 5, 3 },
                    { 4, 8 },
                    { 4, 7 },
                    { 4, 1 },
                    { 1, 4 },
                    { 1, 9 },
                    { 8, 4 }
                });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 5, 3, 7, 24, 53, 121, DateTimeKind.Utc).AddTicks(6495));

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "CapeRaptor",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Дредфорт", 15 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "ChalRocks",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Утёс Кастерли", 100 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaFarms",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Редфорт", 60 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaValley",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Риверран", 71 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "HeatherOfDimmoria",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Вранодрев", 70 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LimestoneRidges",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Орлиное гнездо", 59 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "MouthOfPolaima",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Винтерфелл", 14 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SummerCoast",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Пайк", 43 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TinMines",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Хорнвуд", 17 });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "Coffers", "Investments", "Name", "OrganizationType", "ProvinceId", "SuzerainId", "Warriors" },
                values: new object[,]
                {
                    { "BearIsland", 4000, 0, "Медвежий остров", 1, 10, null, 100 },
                    { "Harrenhal", 4000, 0, "Харренхол", 1, 78, null, 100 },
                    { "KailinMoat", 4000, 0, "Ров Кайлин", 1, 28, null, 100 },
                    { "HarshSong", 4000, 0, "Суровая песнь", 1, 52, null, 100 },
                    { "LightIsland", 4000, 0, "Светлый остров", 1, 92, null, 100 },
                    { "Tarbekhall", 4000, 0, "Тарбекхолл", 1, 91, null, 100 },
                    { "IceCreek", 4000, 0, "Ледяноый ручей", 1, 51, null, 100 },
                    { "SnakeForest", 4000, 0, "Змеиный лес", 1, 53, null, 100 },
                    { "OldCastle", 4000, 0, "Старый замок", 1, 27, null, 100 },
                    { "MemorableLights", 4000, 0, "Пиршественные огни", 1, 93, null, 100 },
                    { "Isthmus", 4000, 0, "Перешеек", 1, 29, null, 100 },
                    { "MaidenPond", 4000, 0, "Девичий пруд", 1, 77, null, 100 },
                    { "SheepsGate", 4000, 0, "Бараньи ворота", 1, 26, null, 100 },
                    { "Sarsfield", 4000, 0, "Сарсфилд", 1, 94, null, 100 },
                    { "Longbow", 4000, 0, "Длинный лук", 1, 55, null, 100 },
                    { "Saltworks", 4000, 0, "Солеварни", 1, 76, null, 100 },
                    { "WhiteHarbor", 4000, 0, "Белая гавань", 1, 25, null, 100 },
                    { "Hornval", 4000, 0, "Хорнваль", 1, 95, null, 100 },
                    { "NineStars", 4000, 0, "Девять звёзд", 1, 57, null, 100 },
                    { "Barrowton", 4000, 0, "Барроутон", 1, 24, null, 100 },
                    { "IronOakwood", 4000, 0, "Железная дубрава", 1, 58, null, 100 },
                    { "DeepBurrow", 4000, 0, "Глубокая нора", 1, 96, null, 100 },
                    { "Springs", 4000, 0, "Родники", 1, 23, null, 100 },
                    { "Darry", 4000, 0, "Дарри", 1, 75, null, 100 },
                    { "HeartsHouse", 4000, 0, "Дом сердец", 1, 54, null, 100 },
                    { "Ashmark", 4000, 0, "Эшмарк", 1, 90, null, 100 },
                    { "Fingers", 4000, 0, "Персты", 1, 50, null, 100 },
                    { "StoneCoast", 4000, 0, "Каменный берег", 1, 22, null, 100 },
                    { "Lordport", 4000, 0, "Лордпорт", 1, 42, null, 100 },
                    { "StoneSepta", 4000, 0, "Каменная септа", 1, 84, null, 100 },
                    { "Castlewood", 4000, 0, "Каслвуд", 1, 83, null, 100 },
                    { "IronGrove", 4000, 0, "Железная роща", 1, 41, null, 100 },
                    { "Stonewood", 4000, 0, "Камнедрев", 1, 40, null, 100 }
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "Coffers", "Investments", "Name", "OrganizationType", "ProvinceId", "SuzerainId", "Warriors" },
                values: new object[,]
                {
                    { "Walmark", 4000, 0, "Волмарк", 1, 44, null, 100 },
                    { "Windhall", 4000, 0, "Виндхолл", 1, 85, null, 100 },
                    { "WanderersRefuge", 4000, 0, "Приют странника", 1, 82, null, 100 },
                    { "TenTowers", 4000, 0, "Десять башен", 1, 39, null, 100 },
                    { "BaneFortress", 4000, 0, "Гибельная крепость", 1, 86, null, 100 },
                    { "LongSister", 4000, 0, "Длинная сестра", 1, 45, null, 100 },
                    { "PinkMaiden", 4000, 0, "Розовая дева", 1, 81, null, 100 },
                    { "Orkmont", 4000, 0, "Оркмонт", 1, 38, null, 100 },
                    { "SweetSister", 4000, 0, "Милая сестра", 1, 46, null, 100 },
                    { "Pebbleton", 4000, 0, "Пебблтон", 1, 37, null, 100 },
                    { "Rock", 4000, 0, "Скала", 1, 87, null, 100 },
                    { "Atranta", 4000, 0, "Атранта", 1, 80, null, 100 },
                    { "LittleSister", 4000, 0, "Малая сестра", 1, 47, null, 100 },
                    { "Castamere", 4000, 0, "Кастамере", 1, 88, null, 100 },
                    { "Loaches", 4000, 0, "Гольцы", 1, 36, null, 100 },
                    { "SaltCliff", 4000, 0, "Солёный утёс", 1, 35, null, 100 },
                    { "LonelyBeacon", 4000, 0, "Одинокий светоч", 1, 34, null, 100 },
                    { "PebbleIsland", 4000, 0, "Галечный остров", 1, 48, null, 100 },
                    { "Hammerhorn", 4000, 0, "Хаммерхорн", 1, 33, null, 100 },
                    { "GoldenTooth", 4000, 0, "Золотой зуб", 1, 89, null, 100 },
                    { "OldVic", 4000, 0, "Старый Вик", 1, 32, null, 100 },
                    { "Acorn", 4000, 0, "Жёлудь", 1, 79, null, 100 },
                    { "Socks", 4000, 0, "Сосцы", 1, 49, null, 100 },
                    { "BlackWave", 4000, 0, "Чёрная волна", 1, 31, null, 100 },
                    { "FlintFinger", 4000, 0, "Кремневый палец", 1, 30, null, 100 },
                    { "Keys", 4000, 0, "Ключи", 1, 97, null, 100 },
                    { "OldAnchor", 4000, 0, "Старый якорь", 1, 56, null, 100 },
                    { "IronHill", 4000, 0, "Железный холм", 1, 13, null, 100 },
                    { "WolfForest", 4000, 0, "Волчий лес", 1, 20, null, 100 },
                    { "GrayHollow", 4000, 0, "Серая лощина", 1, 61, null, 100 },
                    { "Greenfield", 4000, 0, "Гринфилд", 1, 107, null, 100 },
                    { "ServinCastle", 4000, 0, "Замок Сервинов", 1, 19, null, 100 },
                    { "Gemini", 4000, 0, "Близнецы", 1, 66, null, 100 },
                    { "Runestone", 4000, 0, "Рунный камень", 1, 62, null, 100 },
                    { "StoneBulwark", 4000, 0, "Каменный оплот", 1, 72, null, 100 },
                    { "BlackBackwater", 4000, 0, "Чёрная заводь", 1, 18, null, 100 },
                    { "Lannisport", 4000, 0, "Ланниспорт", 1, 101, null, 100 },
                    { "Wicks", 4000, 0, "Фитили", 1, 65, null, 100 },
                    { "GoodFair", 4000, 0, "Добрая ярмарка", 1, 69, null, 100 },
                    { "Cornfield", 4000, 0, "Корнфилд", 1, 106, null, 100 },
                    { "WitchIsland", 4000, 0, "Ведьмин остров", 1, 63, null, 100 },
                    { "WidowsWatch", 4000, 0, "Вдовий дозор", 1, 16, null, 100 }
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "Coffers", "Investments", "Name", "OrganizationType", "ProvinceId", "SuzerainId", "Warriors" },
                values: new object[,]
                {
                    { "TeaCity", 4000, 0, "Чаячий город", 1, 64, null, 100 },
                    { "LeicesterCastle", 4000, 0, "Замок Личестеров", 1, 73, null, 100 },
                    { "CleansCastle", 4000, 0, "Замок Клиганов", 1, 99, null, 100 },
                    { "Crackhall", 4000, 0, "Крейкхолл", 1, 105, null, 100 },
                    { "OlStones", 4000, 0, "Старые камни", 1, 68, null, 100 },
                    { "SilverHill", 4000, 0, "Серебрянный холм", 1, 98, null, 100 },
                    { "TorchensInheritance", 4000, 0, "Торхенов удел", 1, 21, null, 100 },
                    { "GoldenRoad", 4000, 0, "Золотая дорога", 1, 108, null, 100 },
                    { "SeaDragonCape", 4000, 0, "Мыс морского дракона", 1, 11, null, 100 },
                    { "Darkshire", 4000, 0, "Темнолесье", 1, 12, null, 100 },
                    { "HarrowayCity", 4000, 0, "Город Харровея", 1, 74, null, 100 },
                    { "Seagard", 4000, 0, "Сигард", 1, 67, null, 100 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[,]
                {
                    { 68, 69 },
                    { 79, 73 },
                    { 79, 78 },
                    { 71, 80 },
                    { 66, 69 },
                    { 78, 79 },
                    { 82, 81 },
                    { 82, 79 },
                    { 78, 83 },
                    { 69, 68 },
                    { 68, 39 },
                    { 68, 66 },
                    { 73, 79 },
                    { 79, 72 },
                    { 67, 39 },
                    { 82, 80 },
                    { 79, 82 },
                    { 39, 68 },
                    { 81, 80 },
                    { 81, 71 },
                    { 66, 68 },
                    { 67, 30 },
                    { 80, 81 },
                    { 71, 81 },
                    { 67, 31 },
                    { 67, 68 },
                    { 80, 82 },
                    { 69, 66 },
                    { 80, 79 },
                    { 68, 67 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[,]
                {
                    { 80, 72 },
                    { 80, 71 },
                    { 81, 82 },
                    { 79, 80 },
                    { 72, 80 },
                    { 73, 74 },
                    { 78, 77 },
                    { 39, 70 },
                    { 76, 75 },
                    { 75, 76 },
                    { 65, 76 },
                    { 79, 83 },
                    { 70, 73 },
                    { 72, 73 },
                    { 75, 65 },
                    { 73, 70 },
                    { 75, 59 },
                    { 76, 65 },
                    { 75, 74 },
                    { 73, 72 },
                    { 65, 75 },
                    { 59, 75 },
                    { 66, 74 },
                    { 74, 69 },
                    { 74, 70 },
                    { 74, 73 },
                    { 69, 74 },
                    { 74, 66 },
                    { 74, 75 },
                    { 72, 71 },
                    { 65, 77 },
                    { 72, 70 },
                    { 68, 70 },
                    { 69, 70 },
                    { 70, 68 },
                    { 70, 74 },
                    { 78, 75 },
                    { 78, 74 },
                    { 70, 69 },
                    { 78, 73 },
                    { 77, 78 },
                    { 75, 78 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[,]
                {
                    { 70, 39 },
                    { 70, 71 },
                    { 74, 78 },
                    { 73, 78 },
                    { 71, 70 },
                    { 77, 76 },
                    { 70, 72 },
                    { 77, 74 },
                    { 76, 77 },
                    { 71, 72 },
                    { 75, 77 },
                    { 72, 79 },
                    { 82, 83 },
                    { 44, 86 },
                    { 83, 78 },
                    { 96, 100 },
                    { 95, 100 },
                    { 94, 100 },
                    { 93, 100 },
                    { 99, 98 },
                    { 99, 96 },
                    { 98, 99 },
                    { 96, 99 },
                    { 98, 97 },
                    { 98, 96 },
                    { 97, 98 },
                    { 96, 98 },
                    { 97, 84 },
                    { 99, 100 },
                    { 97, 96 },
                    { 84, 97 },
                    { 96, 84 },
                    { 96, 81 },
                    { 96, 95 },
                    { 95, 96 },
                    { 84, 96 },
                    { 81, 96 },
                    { 95, 81 },
                    { 95, 89 },
                    { 95, 90 },
                    { 95, 94 },
                    { 94, 95 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[,]
                {
                    { 90, 95 },
                    { 96, 97 },
                    { 89, 95 },
                    { 100, 93 },
                    { 100, 95 },
                    { 97, 108 },
                    { 84, 108 },
                    { 107, 106 },
                    { 107, 97 },
                    { 107, 98 },
                    { 106, 107 },
                    { 98, 107 },
                    { 97, 107 },
                    { 106, 98 },
                    { 106, 101 },
                    { 106, 105 },
                    { 105, 106 },
                    { 101, 106 },
                    { 100, 94 },
                    { 98, 106 },
                    { 105, 93 },
                    { 101, 105 },
                    { 93, 105 },
                    { 101, 93 },
                    { 101, 98 },
                    { 101, 99 },
                    { 101, 100 },
                    { 100, 101 },
                    { 99, 101 },
                    { 98, 101 },
                    { 93, 101 },
                    { 100, 99 },
                    { 100, 96 },
                    { 105, 101 },
                    { 83, 79 },
                    { 81, 95 },
                    { 94, 91 },
                    { 85, 88 },
                    { 71, 88 },
                    { 87, 86 },
                    { 86, 87 },
                    { 86, 85 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[,]
                {
                    { 86, 39 },
                    { 86, 44 },
                    { 86, 42 },
                    { 86, 43 },
                    { 85, 86 },
                    { 67, 66 },
                    { 43, 86 },
                    { 39, 86 },
                    { 86, 88 },
                    { 85, 71 },
                    { 85, 39 },
                    { 71, 85 },
                    { 70, 85 },
                    { 39, 85 },
                    { 84, 81 },
                    { 84, 78 },
                    { 84, 83 },
                    { 84, 82 },
                    { 83, 84 },
                    { 82, 84 },
                    { 81, 84 },
                    { 78, 84 },
                    { 83, 82 },
                    { 85, 70 },
                    { 94, 90 },
                    { 87, 88 },
                    { 88, 85 },
                    { 94, 93 },
                    { 93, 94 },
                    { 91, 94 },
                    { 90, 94 },
                    { 93, 91 },
                    { 93, 92 },
                    { 92, 93 },
                    { 92, 91 },
                    { 92, 87 },
                    { 91, 92 },
                    { 87, 92 },
                    { 91, 90 },
                    { 91, 88 },
                    { 88, 86 },
                    { 91, 87 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[,]
                {
                    { 88, 91 },
                    { 87, 91 },
                    { 90, 89 },
                    { 90, 88 },
                    { 89, 90 },
                    { 88, 90 },
                    { 89, 81 },
                    { 89, 71 },
                    { 89, 88 },
                    { 88, 89 },
                    { 81, 89 },
                    { 71, 89 },
                    { 88, 71 },
                    { 90, 91 },
                    { 66, 67 },
                    { 62, 61 },
                    { 31, 67 },
                    { 27, 26 },
                    { 27, 25 },
                    { 26, 27 },
                    { 25, 27 },
                    { 26, 25 },
                    { 26, 16 },
                    { 26, 17 },
                    { 25, 26 },
                    { 17, 26 },
                    { 16, 26 },
                    { 25, 19 },
                    { 25, 24 },
                    { 25, 17 },
                    { 25, 18 },
                    { 24, 25 },
                    { 19, 25 },
                    { 18, 25 },
                    { 17, 25 },
                    { 24, 23 },
                    { 24, 19 },
                    { 24, 21 },
                    { 23, 24 },
                    { 21, 24 },
                    { 19, 24 },
                    { 23, 21 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[,]
                {
                    { 23, 22 },
                    { 22, 23 },
                    { 24, 28 },
                    { 25, 28 },
                    { 27, 28 },
                    { 28, 25 },
                    { 37, 32 },
                    { 36, 37 },
                    { 33, 37 },
                    { 32, 37 },
                    { 36, 35 },
                    { 36, 33 },
                    { 35, 36 },
                    { 33, 36 },
                    { 35, 32 },
                    { 33, 35 },
                    { 34, 33 },
                    { 33, 34 },
                    { 33, 32 },
                    { 21, 23 },
                    { 32, 33 },
                    { 31, 32 },
                    { 31, 30 },
                    { 30, 31 },
                    { 30, 24 },
                    { 30, 23 },
                    { 24, 30 },
                    { 23, 30 },
                    { 29, 27 },
                    { 29, 28 },
                    { 28, 29 },
                    { 27, 29 },
                    { 28, 24 },
                    { 28, 27 },
                    { 32, 31 },
                    { 22, 21 },
                    { 22, 11 },
                    { 21, 22 },
                    { 15, 16 },
                    { 15, 14 },
                    { 15, 6 },
                    { 15, 7 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[,]
                {
                    { 14, 15 },
                    { 7, 15 },
                    { 14, 13 },
                    { 14, 7 },
                    { 14, 8 },
                    { 13, 14 },
                    { 8, 14 },
                    { 7, 14 },
                    { 13, 12 },
                    { 16, 15 },
                    { 13, 8 },
                    { 12, 13 },
                    { 9, 13 },
                    { 8, 13 },
                    { 12, 9 },
                    { 12, 10 },
                    { 12, 11 },
                    { 11, 12 },
                    { 10, 12 },
                    { 9, 12 },
                    { 11, 10 },
                    { 10, 11 },
                    { 10, 9 },
                    { 9, 10 },
                    { 13, 9 },
                    { 37, 36 },
                    { 14, 17 },
                    { 16, 17 },
                    { 11, 22 },
                    { 21, 11 },
                    { 21, 19 },
                    { 21, 20 },
                    { 20, 21 },
                    { 19, 21 },
                    { 11, 21 },
                    { 20, 11 },
                    { 20, 19 },
                    { 20, 14 },
                    { 20, 13 },
                    { 20, 12 },
                    { 19, 20 },
                    { 15, 17 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[,]
                {
                    { 14, 20 },
                    { 12, 20 },
                    { 11, 20 },
                    { 19, 18 },
                    { 19, 14 },
                    { 18, 19 },
                    { 14, 19 },
                    { 18, 17 },
                    { 18, 14 },
                    { 17, 18 },
                    { 14, 18 },
                    { 17, 16 },
                    { 17, 15 },
                    { 17, 14 },
                    { 13, 20 },
                    { 39, 67 },
                    { 37, 33 },
                    { 32, 38 },
                    { 60, 59 },
                    { 59, 60 },
                    { 58, 60 },
                    { 59, 58 },
                    { 59, 55 },
                    { 59, 54 },
                    { 59, 52 },
                    { 58, 59 },
                    { 55, 59 },
                    { 54, 59 },
                    { 52, 59 },
                    { 58, 57 },
                    { 58, 55 },
                    { 57, 58 },
                    { 55, 58 },
                    { 57, 56 },
                    { 57, 55 },
                    { 56, 57 },
                    { 55, 57 },
                    { 56, 55 },
                    { 55, 56 },
                    { 55, 54 },
                    { 54, 55 },
                    { 54, 50 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[,]
                {
                    { 54, 53 },
                    { 53, 54 },
                    { 52, 54 },
                    { 60, 58 },
                    { 58, 61 },
                    { 60, 61 },
                    { 61, 58 },
                    { 30, 67 },
                    { 66, 50 },
                    { 66, 47 },
                    { 66, 46 },
                    { 66, 29 },
                    { 50, 66 },
                    { 47, 66 },
                    { 45, 66 },
                    { 29, 66 },
                    { 65, 64 },
                    { 64, 65 },
                    { 64, 63 },
                    { 64, 62 },
                    { 50, 54 },
                    { 64, 61 },
                    { 63, 64 },
                    { 62, 64 },
                    { 61, 64 },
                    { 60, 64 },
                    { 63, 62 },
                    { 62, 63 },
                    { 108, 97 },
                    { 62, 56 },
                    { 62, 57 },
                    { 61, 62 },
                    { 57, 62 },
                    { 56, 62 },
                    { 61, 60 },
                    { 64, 60 },
                    { 53, 50 },
                    { 53, 51 },
                    { 51, 53 },
                    { 43, 36 },
                    { 42, 43 },
                    { 36, 43 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[,]
                {
                    { 35, 43 },
                    { 42, 36 },
                    { 42, 40 },
                    { 42, 41 },
                    { 41, 42 },
                    { 40, 42 },
                    { 36, 42 },
                    { 41, 40 },
                    { 41, 39 },
                    { 41, 37 },
                    { 43, 42 },
                    { 40, 41 },
                    { 37, 41 },
                    { 40, 39 },
                    { 39, 40 },
                    { 39, 38 },
                    { 39, 37 },
                    { 39, 31 },
                    { 38, 39 },
                    { 37, 39 },
                    { 31, 39 },
                    { 38, 32 },
                    { 38, 37 },
                    { 38, 31 },
                    { 37, 38 },
                    { 39, 41 },
                    { 31, 38 },
                    { 43, 35 },
                    { 40, 44 },
                    { 50, 53 },
                    { 51, 50 },
                    { 50, 51 },
                    { 50, 49 },
                    { 50, 48 },
                    { 50, 47 },
                    { 49, 50 },
                    { 48, 50 },
                    { 47, 50 },
                    { 48, 47 },
                    { 47, 48 },
                    { 47, 27 },
                    { 47, 46 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[,]
                {
                    { 39, 44 },
                    { 46, 47 },
                    { 27, 47 },
                    { 46, 45 },
                    { 46, 27 },
                    { 45, 46 },
                    { 27, 46 },
                    { 45, 27 },
                    { 45, 29 },
                    { 29, 45 },
                    { 27, 45 },
                    { 44, 42 },
                    { 44, 39 },
                    { 44, 40 },
                    { 42, 44 },
                    { 45, 47 },
                    { 108, 84 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Acorn");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Ashmark");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Atranta");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "BaneFortress");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Barrowton");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "BearIsland");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "BlackBackwater");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "BlackCastle");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "BlackWave");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Carhold");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Castamere");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Castlewood");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "CleansCastle");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Cornfield");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Crackhall");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Darkshire");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Darry");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DeepBurrow");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "EasternWatch");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Fingers");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "FlintFinger");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Gemini");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "GoldenRoad");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "GoldenTooth");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "GoodFair");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "GrayHollow");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Greenfield");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Hammerhorn");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Harrenhal");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "HarrowayCity");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "HarshSong");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "HeartsHouse");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Hornval");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "IceCreek");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "IronGrove");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "IronHill");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "IronOakwood");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Isthmus");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "KailinMoat");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Keys");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Lannisport");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LeicesterCastle");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LightIsland");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LittleSister");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Loaches");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LonelyBeacon");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Longbow");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LongSister");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Lordport");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "MaidenPond");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "MemorableLights");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "NewGift");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "NineStars");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "OldAnchor");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "OldCastle");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "OldVic");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "OlStones");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Orkmont");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "PebbleIsland");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Pebbleton");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "PinkMaiden");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Rock");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Runestone");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SaltCliff");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Saltworks");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Sarsfield");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SeaDragonCape");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Seagard");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "ServinCastle");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SheepsGate");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SilverHill");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Skagos");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SnakeForest");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Socks");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Springs");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "StoneBulwark");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "StoneCoast");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "StoneHill");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "StoneSepta");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Stonewood");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SweetSister");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Tarbekhall");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TeaCity");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TenTowers");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TheLastHearth");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TheTop");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TorchensInheritance");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TwilightTower");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Walmark");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "WanderersRefuge");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "WhiteHarbor");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Wicks");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "WidowsWatch");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Windhall");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "WitchIsland");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "WolfForest");

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 1, 9 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 4, 7 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 4, 8 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 6, 9 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 7, 4 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 7, 14 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 7, 15 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 8, 4 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 8, 13 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 8, 14 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 9, 1 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 9, 10 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 9, 12 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 9, 13 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 10, 9 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 10, 11 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 10, 12 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 11, 10 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 11, 12 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 11, 20 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 11, 21 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 11, 22 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 12, 9 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 12, 10 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 12, 11 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 12, 13 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 12, 20 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 13, 8 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 13, 9 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 13, 12 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 13, 14 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 13, 20 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 14, 7 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 14, 8 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 14, 13 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 14, 15 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 14, 17 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 14, 18 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 14, 19 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 14, 20 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 15, 6 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 15, 7 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 15, 14 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 15, 16 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 15, 17 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 16, 15 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 16, 17 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 16, 26 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 17, 14 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 17, 15 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 17, 16 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 17, 18 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 17, 25 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 17, 26 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 18, 14 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 18, 17 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 18, 19 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 18, 25 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 19, 14 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 19, 18 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 19, 20 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 19, 21 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 19, 24 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 19, 25 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 20, 11 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 20, 12 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 20, 13 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 20, 14 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 20, 19 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 20, 21 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 21, 11 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 21, 19 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 21, 20 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 21, 22 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 21, 23 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 21, 24 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 22, 11 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 22, 21 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 22, 23 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 23, 21 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 23, 22 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 23, 24 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 23, 30 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 24, 19 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 24, 21 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 24, 23 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 24, 25 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 24, 28 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 24, 30 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 25, 17 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 25, 18 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 25, 19 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 25, 24 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 25, 26 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 25, 27 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 25, 28 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 26, 16 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 26, 17 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 26, 25 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 26, 27 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 27, 25 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 27, 26 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 27, 28 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 27, 29 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 27, 45 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 27, 46 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 27, 47 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 28, 24 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 28, 25 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 28, 27 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 28, 29 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 29, 27 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 29, 28 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 29, 45 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 29, 66 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 30, 23 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 30, 24 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 30, 31 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 30, 67 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 31, 30 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 31, 32 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 31, 38 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 31, 39 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 31, 67 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 32, 31 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 32, 33 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 32, 37 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 32, 38 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 33, 32 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 33, 34 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 33, 35 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 33, 36 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 33, 37 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 34, 33 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 35, 32 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 35, 36 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 35, 43 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 36, 33 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 36, 35 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 36, 37 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 36, 42 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 36, 43 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 37, 32 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 37, 33 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 37, 36 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 37, 38 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 37, 39 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 37, 41 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 38, 31 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 38, 32 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 38, 37 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 38, 39 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 39, 31 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 39, 37 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 39, 38 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 39, 40 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 39, 41 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 39, 44 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 39, 67 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 39, 68 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 39, 70 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 39, 85 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 39, 86 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 40, 39 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 40, 41 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 40, 42 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 40, 44 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 41, 37 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 41, 39 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 41, 40 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 41, 42 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 42, 36 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 42, 40 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 42, 41 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 42, 43 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 42, 44 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 43, 35 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 43, 36 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 43, 42 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 43, 86 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 44, 39 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 44, 40 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 44, 42 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 44, 86 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 45, 27 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 45, 29 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 45, 46 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 45, 47 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 45, 66 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 46, 27 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 46, 45 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 46, 47 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 47, 27 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 47, 46 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 47, 48 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 47, 50 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 47, 66 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 48, 47 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 48, 50 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 49, 50 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 50, 47 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 50, 48 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 50, 49 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 50, 51 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 50, 53 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 50, 54 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 50, 66 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 51, 50 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 51, 53 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 52, 54 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 52, 59 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 53, 50 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 53, 51 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 53, 54 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 54, 50 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 54, 53 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 54, 55 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 54, 59 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 55, 54 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 55, 56 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 55, 57 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 55, 58 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 55, 59 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 56, 55 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 56, 57 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 56, 62 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 57, 55 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 57, 56 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 57, 58 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 57, 62 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 58, 55 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 58, 57 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 58, 59 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 58, 60 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 58, 61 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 59, 52 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 59, 54 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 59, 55 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 59, 58 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 59, 60 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 59, 75 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 60, 58 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 60, 59 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 60, 61 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 60, 64 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 61, 58 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 61, 60 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 61, 62 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 61, 64 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 62, 56 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 62, 57 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 62, 61 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 62, 63 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 62, 64 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 63, 62 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 63, 64 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 64, 60 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 64, 61 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 64, 62 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 64, 63 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 64, 65 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 65, 64 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 65, 75 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 65, 76 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 65, 77 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 66, 29 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 66, 46 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 66, 47 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 66, 50 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 66, 67 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 66, 68 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 66, 69 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 66, 74 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 67, 30 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 67, 31 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 67, 39 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 67, 66 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 67, 68 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 68, 39 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 68, 66 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 68, 67 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 68, 69 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 68, 70 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 69, 66 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 69, 68 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 69, 70 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 69, 74 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 70, 39 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 70, 68 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 70, 69 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 70, 71 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 70, 72 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 70, 73 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 70, 74 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 70, 85 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 71, 70 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 71, 72 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 71, 80 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 71, 81 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 71, 85 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 71, 88 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 71, 89 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 72, 70 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 72, 71 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 72, 73 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 72, 79 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 72, 80 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 73, 70 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 73, 72 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 73, 74 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 73, 78 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 73, 79 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 74, 66 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 74, 69 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 74, 70 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 74, 73 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 74, 75 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 74, 78 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 75, 59 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 75, 65 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 75, 74 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 75, 76 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 75, 77 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 75, 78 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 76, 65 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 76, 75 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 76, 77 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 77, 74 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 77, 76 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 77, 78 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 78, 73 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 78, 74 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 78, 75 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 78, 77 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 78, 79 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 78, 83 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 78, 84 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 79, 72 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 79, 73 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 79, 78 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 79, 80 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 79, 82 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 79, 83 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 80, 71 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 80, 72 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 80, 79 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 80, 81 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 80, 82 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 81, 71 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 81, 80 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 81, 82 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 81, 84 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 81, 89 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 81, 95 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 81, 96 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 82, 79 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 82, 80 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 82, 81 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 82, 83 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 82, 84 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 83, 78 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 83, 79 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 83, 82 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 83, 84 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 84, 78 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 84, 81 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 84, 82 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 84, 83 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 84, 96 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 84, 97 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 84, 108 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 85, 39 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 85, 70 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 85, 71 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 85, 86 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 85, 88 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 86, 39 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 86, 42 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 86, 43 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 86, 44 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 86, 85 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 86, 87 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 86, 88 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 87, 86 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 87, 88 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 87, 91 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 87, 92 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 88, 71 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 88, 85 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 88, 86 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 88, 89 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 88, 90 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 88, 91 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 89, 71 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 89, 81 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 89, 88 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 89, 90 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 89, 95 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 90, 88 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 90, 89 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 90, 91 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 90, 94 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 90, 95 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 91, 87 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 91, 88 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 91, 90 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 91, 92 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 91, 94 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 92, 87 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 92, 91 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 92, 93 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 93, 91 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 93, 92 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 93, 94 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 93, 100 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 93, 101 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 93, 105 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 94, 90 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 94, 91 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 94, 93 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 94, 95 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 94, 100 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 95, 81 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 95, 89 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 95, 90 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 95, 94 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 95, 96 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 95, 100 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 96, 81 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 96, 84 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 96, 95 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 96, 97 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 96, 98 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 96, 99 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 96, 100 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 97, 84 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 97, 96 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 97, 98 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 97, 107 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 97, 108 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 98, 96 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 98, 97 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 98, 99 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 98, 101 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 98, 106 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 98, 107 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 99, 96 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 99, 98 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 99, 100 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 99, 101 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 100, 93 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 100, 94 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 100, 95 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 100, 96 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 100, 99 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 100, 101 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 101, 93 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 101, 98 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 101, 99 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 101, 100 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 101, 105 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 101, 106 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 105, 93 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 105, 101 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 105, 106 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 106, 98 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 106, 101 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 106, 105 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 106, 107 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 107, 97 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 107, 98 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 107, 106 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 108, 84 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 108, 97 });

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "CapeRaptor",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Мыс ящера", 2 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "ChalRocks",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Меловые скалы", 8 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaFarms",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Фермы Диммории", 7 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaValley",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Долина Диммории", 5 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "HeatherOfDimmoria",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Верещатник Диммории", 4 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LimestoneRidges",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Известняковые хребты", 9 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "MouthOfPolaima",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Устье Полаймы", 3 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SummerCoast",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Летний берег", 6 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TinMines",
                columns: new[] { "Name", "ProvinceId" },
                values: new object[] { "Оловянные шахты", 1 });

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Оловянные шахты");

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Мыс ящера");

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Устье Полаймы");

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Верещатник Диммории");

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Долина Диммории");

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Летний берег");

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Фермы Диммории");

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Меловые скалы");

            migrationBuilder.UpdateData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "Известняковые хребты");

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[,]
                {
                    { 1, 3 },
                    { 8, 5 },
                    { 7, 9 },
                    { 6, 4 },
                    { 5, 8 },
                    { 4, 6 },
                    { 4, 5 },
                    { 3, 1 },
                    { 9, 7 }
                });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 4, 30, 4, 27, 42, 268, DateTimeKind.Utc).AddTicks(1403));
        }
    }
}
