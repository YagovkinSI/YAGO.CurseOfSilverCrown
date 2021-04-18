using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class Investments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Investments",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "CapeRaptor",
                column: "Coffers",
                value: 3770);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "ChalRocks",
                columns: new[] { "Coffers", "Warriors" },
                values: new object[] { 3710, 90 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaFarms",
                column: "Coffers",
                value: 3650);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaValley",
                columns: new[] { "Coffers", "Warriors" },
                values: new object[] { 4250, 100 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "HeatherOfDimmoria",
                columns: new[] { "Coffers", "Warriors" },
                values: new object[] { 4330, 90 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LimestoneRidges",
                columns: new[] { "Coffers", "Warriors" },
                values: new object[] { 4330, 110 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "MouthOfPolaima",
                columns: new[] { "Coffers", "Warriors" },
                values: new object[] { 3770, 90 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SummerCoast",
                columns: new[] { "Coffers", "Warriors" },
                values: new object[] { 3860, 110 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TinMines",
                columns: new[] { "Coffers", "Warriors" },
                values: new object[] { 3860, 110 });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 4, 18, 3, 40, 46, 417, DateTimeKind.Utc).AddTicks(2170));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Investments",
                table: "Organizations");

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "CapeRaptor",
                column: "Coffers",
                value: 3890);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "ChalRocks",
                columns: new[] { "Coffers", "Warriors" },
                values: new object[] { 4010, 110 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaFarms",
                column: "Coffers",
                value: 3660);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaValley",
                columns: new[] { "Coffers", "Warriors" },
                values: new object[] { 4180, 110 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "HeatherOfDimmoria",
                columns: new[] { "Coffers", "Warriors" },
                values: new object[] { 4000, 100 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LimestoneRidges",
                columns: new[] { "Coffers", "Warriors" },
                values: new object[] { 4250, 100 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "MouthOfPolaima",
                columns: new[] { "Coffers", "Warriors" },
                values: new object[] { 3910, 100 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SummerCoast",
                columns: new[] { "Coffers", "Warriors" },
                values: new object[] { 4280, 100 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TinMines",
                columns: new[] { "Coffers", "Warriors" },
                values: new object[] { 4330, 100 });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 4, 17, 4, 0, 44, 545, DateTimeKind.Utc).AddTicks(7693));
        }
    }
}
