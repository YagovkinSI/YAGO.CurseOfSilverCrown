using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class RoutesFromCarhold : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 6, 9 });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[] { 6, 15 });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 5, 15, 2, 36, 3, 751, DateTimeKind.Utc).AddTicks(6631));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromProvinceId", "ToProvinceId" },
                keyValues: new object[] { 6, 15 });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[] { 6, 9 });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 5, 14, 4, 19, 30, 406, DateTimeKind.Utc).AddTicks(636));
        }
    }
}
