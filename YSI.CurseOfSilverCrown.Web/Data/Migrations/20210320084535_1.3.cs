using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Data.Migrations
{
    public partial class _13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "044aebbd-dd4f-4177-b2b1-b4e3d28b6a87");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "0d0c20b5-e62b-46d4-9833-084945666d74");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "2dbffbd6-771c-4c13-a727-4286d8e8e363");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "32b08ce3-0371-4419-90ad-b45dcef200bc");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "4f46faca-211e-4124-8934-591e758e53dd");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "8578ac0f-27fe-4717-9397-3a577e124764");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "921edeb1-150a-447a-aa0d-446246a47a23");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "e8d288fb-ee9b-4c04-8467-68d2f87f5dbd");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "f140d3b8-1deb-43bc-b856-63c6a68785e8");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Turns",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Started",
                table: "Turns",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
                columns: new[] { "IsActive", "Started" },
                values: new object[] { true, new DateTime(2021, 3, 20, 8, 45, 34, 829, DateTimeKind.Utc).AddTicks(12) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "IsActive",
                table: "Turns");

            migrationBuilder.DropColumn(
                name: "Started",
                table: "Turns");

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "OrganizationId", "Result", "TargetOrganizationId", "TurnId", "Type" },
                values: new object[,]
                {
                    { "32b08ce3-0371-4419-90ad-b45dcef200bc", "TinMines", null, null, 1, 0 },
                    { "8578ac0f-27fe-4717-9397-3a577e124764", "CapeRaptor", null, null, 1, 0 },
                    { "921edeb1-150a-447a-aa0d-446246a47a23", "MouthOfPolaima", null, null, 1, 0 },
                    { "2dbffbd6-771c-4c13-a727-4286d8e8e363", "HeatherOfDimmoria", null, null, 1, 0 },
                    { "f140d3b8-1deb-43bc-b856-63c6a68785e8", "DimmoriaValley", null, null, 1, 0 },
                    { "044aebbd-dd4f-4177-b2b1-b4e3d28b6a87", "SummerCoast", null, null, 1, 0 },
                    { "4f46faca-211e-4124-8934-591e758e53dd", "DimmoriaFarms", null, null, 1, 0 },
                    { "0d0c20b5-e62b-46d4-9833-084945666d74", "ChalRocks", null, null, 1, 0 },
                    { "e8d288fb-ee9b-4c04-8467-68d2f87f5dbd", "LimestoneRidges", null, null, 1, 0 }
                });
        }
    }
}
