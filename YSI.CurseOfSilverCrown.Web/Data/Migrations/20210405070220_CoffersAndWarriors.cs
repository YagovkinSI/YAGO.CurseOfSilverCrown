using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Data.Migrations
{
    public partial class CoffersAndWarriors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "235bb5a6-94b2-42d1-a0ef-3a4c04857c6c");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "552b1946-2767-4c4c-a2a1-1326e247b944");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "557bd7ca-5b66-4631-83d5-34d4ff7ce53d");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "6baa792e-2378-4535-9d82-0c2171ca3ca1");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "6bf9afea-77c3-4696-809c-78d0230ee360");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "981d9b86-baff-444b-a7bb-f4f7ae7a6e16");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "b0fd6bee-2eb3-47b6-b7da-6ebc8f3a55a5");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "bb6debd9-3311-4d59-9f8b-1d686332099a");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "e3aaad84-11fd-44ee-9b3d-d753dec4f623");

            migrationBuilder.AddColumn<int>(
                name: "Coffers",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Warriors",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "OrganizationId", "Result", "TargetOrganizationId", "TurnId", "Type" },
                values: new object[,]
                {
                    { "639250f2-5285-484e-99c9-9b5fa13e192a", "TinMines", null, null, 1, 0 },
                    { "05d9f127-d381-4277-bbd1-b554b1cce50b", "CapeRaptor", null, null, 1, 0 },
                    { "d8cb0361-5205-4107-b3a4-84bec250a8f8", "MouthOfPolaima", null, null, 1, 0 },
                    { "d44a8e52-0dd7-4581-a7d9-c3971dcc8fc8", "HeatherOfDimmoria", null, null, 1, 0 },
                    { "d79ae429-2714-49ef-82a7-7283407447b3", "DimmoriaValley", null, null, 1, 0 },
                    { "eb5a8e4a-e85c-414e-8597-5876e92ee58b", "SummerCoast", null, null, 1, 0 },
                    { "a0707b8b-863f-4247-b308-bfb16d2c9ca5", "DimmoriaFarms", null, null, 1, 0 },
                    { "3aa984a5-3570-429a-981d-488b718f55d3", "ChalRocks", null, null, 1, 0 },
                    { "6bcf2b16-14a4-491c-94e5-4306ca97aad7", "LimestoneRidges", null, null, 1, 0 }
                });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 4, 5, 7, 2, 19, 255, DateTimeKind.Utc).AddTicks(341));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "05d9f127-d381-4277-bbd1-b554b1cce50b");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "3aa984a5-3570-429a-981d-488b718f55d3");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "639250f2-5285-484e-99c9-9b5fa13e192a");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "6bcf2b16-14a4-491c-94e5-4306ca97aad7");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "a0707b8b-863f-4247-b308-bfb16d2c9ca5");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "d44a8e52-0dd7-4581-a7d9-c3971dcc8fc8");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "d79ae429-2714-49ef-82a7-7283407447b3");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "d8cb0361-5205-4107-b3a4-84bec250a8f8");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "eb5a8e4a-e85c-414e-8597-5876e92ee58b");

            migrationBuilder.DropColumn(
                name: "Coffers",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Warriors",
                table: "Organizations");

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "OrganizationId", "Result", "TargetOrganizationId", "TurnId", "Type" },
                values: new object[,]
                {
                    { "b0fd6bee-2eb3-47b6-b7da-6ebc8f3a55a5", "TinMines", null, null, 1, 0 },
                    { "e3aaad84-11fd-44ee-9b3d-d753dec4f623", "CapeRaptor", null, null, 1, 0 },
                    { "235bb5a6-94b2-42d1-a0ef-3a4c04857c6c", "MouthOfPolaima", null, null, 1, 0 },
                    { "6baa792e-2378-4535-9d82-0c2171ca3ca1", "HeatherOfDimmoria", null, null, 1, 0 },
                    { "6bf9afea-77c3-4696-809c-78d0230ee360", "DimmoriaValley", null, null, 1, 0 },
                    { "bb6debd9-3311-4d59-9f8b-1d686332099a", "SummerCoast", null, null, 1, 0 },
                    { "981d9b86-baff-444b-a7bb-f4f7ae7a6e16", "DimmoriaFarms", null, null, 1, 0 },
                    { "552b1946-2767-4c4c-a2a1-1326e247b944", "ChalRocks", null, null, 1, 0 },
                    { "557bd7ca-5b66-4631-83d5-34d4ff7ce53d", "LimestoneRidges", null, null, 1, 0 }
                });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 3, 28, 5, 38, 54, 132, DateTimeKind.Utc).AddTicks(9736));
        }
    }
}
