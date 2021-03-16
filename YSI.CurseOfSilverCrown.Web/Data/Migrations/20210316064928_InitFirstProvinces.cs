using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Data.Migrations
{
    public partial class InitFirstProvinces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Оловянные шахты" },
                    { 2, "Мыс ящера" },
                    { 3, "Устье Полаймы" },
                    { 4, "Верещатник Диммории" },
                    { 5, "Долина Диммории" },
                    { 6, "Летний берег" },
                    { 7, "Фермы Диммории" },
                    { 8, "Меловые скалы" },
                    { 9, "Известняковые хребты" }
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "OrganizationType", "ProvinceId", "SuzerainId" },
                values: new object[,]
                {
                    { "TinMines", 1, 1, null },
                    { "CapeRaptor", 1, 2, null },
                    { "MouthOfPolaima", 1, 3, null },
                    { "HeatherOfDimmoria", 1, 4, null },
                    { "DimmoriaValley", 1, 5, null },
                    { "SummerCoast", 1, 6, null },
                    { "DimmoriaFarms", 1, 7, null },
                    { "ChalRocks", 1, 8, null },
                    { "LimestoneRidges", 1, 9, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "CapeRaptor");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "ChalRocks");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaFarms");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaValley");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "HeatherOfDimmoria");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LimestoneRidges");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "MouthOfPolaima");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SummerCoast");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TinMines");

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
