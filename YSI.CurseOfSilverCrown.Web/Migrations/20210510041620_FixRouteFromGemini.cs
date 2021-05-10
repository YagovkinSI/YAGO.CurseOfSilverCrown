using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class FixRouteFromGemini : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 66, 46 });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[] { 66, 45 });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 5, 10, 4, 16, 19, 892, DateTimeKind.Utc).AddTicks(4774));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 66, 45 });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[] { 66, 46 });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 5, 9, 12, 30, 43, 124, DateTimeKind.Utc).AddTicks(6639));
        }
    }
}
