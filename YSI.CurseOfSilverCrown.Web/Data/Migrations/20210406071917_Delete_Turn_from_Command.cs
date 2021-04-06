using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Data.Migrations
{
    public partial class Delete_Turn_from_Command : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commands_Turns_TurnId",
                table: "Commands");

            migrationBuilder.DropIndex(
                name: "IX_Commands_TurnId",
                table: "Commands");

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
                name: "Result",
                table: "Commands");

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "OrganizationId", "TargetOrganizationId", "TurnId", "Type" },
                values: new object[,]
                {
                    { "49274109-cdb7-49ce-8eed-22185d00815f", "TinMines", null, 0, 0 },
                    { "fd3b9fd8-0cd6-48d4-9758-d001286607bf", "ChalRocks", null, 0, 0 },
                    { "8d56b9fa-19cd-4719-a801-f447f92b5653", "DimmoriaFarms", null, 0, 0 },
                    { "3ad9b544-3655-4ced-acb7-498495a9deed", "SummerCoast", null, 0, 0 },
                    { "22f555ad-13dd-48dd-930a-37918875a2e7", "LimestoneRidges", null, 0, 0 },
                    { "c7eabd63-40e2-4ab7-be39-0d7fb1b9115a", "HeatherOfDimmoria", null, 0, 0 },
                    { "9273c315-bfa6-4c77-87e7-f6181eb5e8eb", "MouthOfPolaima", null, 0, 0 },
                    { "ff4ffd34-025c-4564-a076-6210a4e07f77", "CapeRaptor", null, 0, 0 },
                    { "deec4204-32e1-4506-a51b-9ca7e4b77ec1", "DimmoriaValley", null, 0, 0 }
                });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "CapeRaptor",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 7000, 0, 100 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "ChalRocks",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 7000, 0, 100 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaFarms",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 7000, 0, 100 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaValley",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 7000, 0, 100 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "HeatherOfDimmoria",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 7000, 0, 100 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LimestoneRidges",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 7000, 0, 100 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "MouthOfPolaima",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 7000, 0, 100 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SummerCoast",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 7000, 0, 100 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TinMines",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 7000, 0, 100 });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 4, 6, 7, 19, 16, 586, DateTimeKind.Utc).AddTicks(5546));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "22f555ad-13dd-48dd-930a-37918875a2e7");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "3ad9b544-3655-4ced-acb7-498495a9deed");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "49274109-cdb7-49ce-8eed-22185d00815f");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "8d56b9fa-19cd-4719-a801-f447f92b5653");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "9273c315-bfa6-4c77-87e7-f6181eb5e8eb");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "c7eabd63-40e2-4ab7-be39-0d7fb1b9115a");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "deec4204-32e1-4506-a51b-9ca7e4b77ec1");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "fd3b9fd8-0cd6-48d4-9758-d001286607bf");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "ff4ffd34-025c-4564-a076-6210a4e07f77");

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "Commands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "OrganizationId", "Result", "TargetOrganizationId", "TurnId", "Type" },
                values: new object[,]
                {
                    { "639250f2-5285-484e-99c9-9b5fa13e192a", "TinMines", null, null, 1, 0 },
                    { "3aa984a5-3570-429a-981d-488b718f55d3", "ChalRocks", null, null, 1, 0 },
                    { "a0707b8b-863f-4247-b308-bfb16d2c9ca5", "DimmoriaFarms", null, null, 1, 0 },
                    { "eb5a8e4a-e85c-414e-8597-5876e92ee58b", "SummerCoast", null, null, 1, 0 },
                    { "6bcf2b16-14a4-491c-94e5-4306ca97aad7", "LimestoneRidges", null, null, 1, 0 },
                    { "d44a8e52-0dd7-4581-a7d9-c3971dcc8fc8", "HeatherOfDimmoria", null, null, 1, 0 },
                    { "d8cb0361-5205-4107-b3a4-84bec250a8f8", "MouthOfPolaima", null, null, 1, 0 },
                    { "05d9f127-d381-4277-bbd1-b554b1cce50b", "CapeRaptor", null, null, 1, 0 },
                    { "d79ae429-2714-49ef-82a7-7283407447b3", "DimmoriaValley", null, null, 1, 0 }
                });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "CapeRaptor",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 0, 200000, 0 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "ChalRocks",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 0, 200000, 0 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaFarms",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 0, 200000, 0 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaValley",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 0, 200000, 0 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "HeatherOfDimmoria",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 0, 200000, 0 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LimestoneRidges",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 0, 200000, 0 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "MouthOfPolaima",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 0, 200000, 0 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SummerCoast",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 0, 200000, 0 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TinMines",
                columns: new[] { "Coffers", "Power", "Warriors" },
                values: new object[] { 0, 200000, 0 });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 4, 5, 7, 2, 19, 255, DateTimeKind.Utc).AddTicks(341));

            migrationBuilder.CreateIndex(
                name: "IX_Commands_TurnId",
                table: "Commands",
                column: "TurnId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commands_Turns_TurnId",
                table: "Commands",
                column: "TurnId",
                principalTable: "Turns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
