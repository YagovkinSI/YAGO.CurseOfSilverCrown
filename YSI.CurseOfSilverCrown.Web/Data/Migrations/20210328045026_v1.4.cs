using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Data.Migrations
{
    public partial class v14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "46fca3c9-42e1-40aa-9388-fe8065b13c9f");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "5d08be78-09ab-44bc-a10b-e9e5d0ca5ced");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "8fe02e3d-805f-4429-a4c5-0a50dfac4031");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "cf3d1dc8-fbbe-436f-8f7f-b880efe67a9f");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "dd2d6d29-1519-4361-9e9b-0062b6ada79f");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "e1be35a7-3146-46df-8855-5c3752ccf2d4");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "e60731f6-c73d-4e31-93cd-d6dd4ebda8fd");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "e83be3ce-c2a5-4934-b4f4-337bd6c244e8");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "ebe83d84-365c-4e3c-9d7e-a432bc0668f8");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Turns");

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "OrganizationId", "Result", "TargetOrganizationId", "TurnId", "Type" },
                values: new object[,]
                {
                    { "ea2781fa-b0d6-40a2-8f69-8cdb86863f7f", "TinMines", null, null, 1, 0 },
                    { "adffbb1f-8098-4d1e-bfe6-b624c8cfe047", "CapeRaptor", null, null, 1, 0 },
                    { "a254fd4e-6eb0-4567-b587-29effbea2b98", "MouthOfPolaima", null, null, 1, 0 },
                    { "64fd1831-5f9a-4792-afb9-d60f6c6890c4", "HeatherOfDimmoria", null, null, 1, 0 },
                    { "ea1dc420-e341-409d-be17-57ad73d1cb59", "DimmoriaValley", null, null, 1, 0 },
                    { "87ca7484-57a6-40b7-834a-443968660af9", "SummerCoast", null, null, 1, 0 },
                    { "306e70ee-3887-47af-a30e-c0972b4975d7", "DimmoriaFarms", null, null, 1, 0 },
                    { "c4a17ef8-8ebc-4df1-a213-8f8c613c9eae", "ChalRocks", null, null, 1, 0 },
                    { "bfaa36df-fa2e-461c-8126-e62227030302", "LimestoneRidges", null, null, 1, 0 }
                });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 3, 28, 4, 50, 24, 914, DateTimeKind.Utc).AddTicks(5116));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "306e70ee-3887-47af-a30e-c0972b4975d7");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "64fd1831-5f9a-4792-afb9-d60f6c6890c4");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "87ca7484-57a6-40b7-834a-443968660af9");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "a254fd4e-6eb0-4567-b587-29effbea2b98");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "adffbb1f-8098-4d1e-bfe6-b624c8cfe047");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "bfaa36df-fa2e-461c-8126-e62227030302");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "c4a17ef8-8ebc-4df1-a213-8f8c613c9eae");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "ea1dc420-e341-409d-be17-57ad73d1cb59");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "ea2781fa-b0d6-40a2-8f69-8cdb86863f7f");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Turns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "OrganizationId", "Result", "TargetOrganizationId", "TurnId", "Type" },
                values: new object[,]
                {
                    { "e83be3ce-c2a5-4934-b4f4-337bd6c244e8", "TinMines", null, null, 1, 0 },
                    { "dd2d6d29-1519-4361-9e9b-0062b6ada79f", "CapeRaptor", null, null, 1, 0 },
                    { "cf3d1dc8-fbbe-436f-8f7f-b880efe67a9f", "MouthOfPolaima", null, null, 1, 0 },
                    { "5d08be78-09ab-44bc-a10b-e9e5d0ca5ced", "HeatherOfDimmoria", null, null, 1, 0 },
                    { "e1be35a7-3146-46df-8855-5c3752ccf2d4", "DimmoriaValley", null, null, 1, 0 },
                    { "e60731f6-c73d-4e31-93cd-d6dd4ebda8fd", "SummerCoast", null, null, 1, 0 },
                    { "ebe83d84-365c-4e3c-9d7e-a432bc0668f8", "DimmoriaFarms", null, null, 1, 0 },
                    { "46fca3c9-42e1-40aa-9388-fe8065b13c9f", "ChalRocks", null, null, 1, 0 },
                    { "8fe02e3d-805f-4429-a4c5-0a50dfac4031", "LimestoneRidges", null, null, 1, 0 }
                });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Name", "Started" },
                values: new object[] { "587 год - Зима", new DateTime(2021, 3, 20, 8, 45, 34, 829, DateTimeKind.Utc).AddTicks(12) });
        }
    }
}
