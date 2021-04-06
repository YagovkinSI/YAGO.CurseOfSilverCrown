using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Data.Migrations
{
    public partial class Add_Coffers_adn_Warriors_To_Commands : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "Coffers",
                table: "Commands",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Warriors",
                table: "Commands",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "Coffers", "OrganizationId", "TargetOrganizationId", "TurnId", "Type", "Warriors" },
                values: new object[,]
                {
                    { "3f968153-b894-4f70-b451-e66864c0aa4f", 0, "TinMines", null, 0, 0, 0 },
                    { "27636ccd-213f-4321-9a7a-c88fc1ac64fe", 0, "CapeRaptor", null, 0, 0, 0 },
                    { "8137943d-1017-4139-8841-d310f33f7c6e", 0, "MouthOfPolaima", null, 0, 0, 0 },
                    { "2fcf93fa-b8e9-4f43-9eca-707b230b2fde", 0, "HeatherOfDimmoria", null, 0, 0, 0 },
                    { "461ec216-256d-49b4-9fed-590bb65d2302", 0, "DimmoriaValley", null, 0, 0, 0 },
                    { "097ec149-5ebe-40c8-88cc-36e9924c4688", 0, "SummerCoast", null, 0, 0, 0 },
                    { "08f7c03f-6568-48fd-8191-7e35a2887bfb", 0, "DimmoriaFarms", null, 0, 0, 0 },
                    { "5fce013b-4924-45ff-b374-c0e8c40a845d", 0, "ChalRocks", null, 0, 0, 0 },
                    { "f4d7576b-17be-42c5-be70-e6a25e7e7b19", 0, "LimestoneRidges", null, 0, 0, 0 }
                });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 4, 6, 7, 32, 31, 417, DateTimeKind.Utc).AddTicks(6430));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "08f7c03f-6568-48fd-8191-7e35a2887bfb");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "097ec149-5ebe-40c8-88cc-36e9924c4688");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "27636ccd-213f-4321-9a7a-c88fc1ac64fe");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "2fcf93fa-b8e9-4f43-9eca-707b230b2fde");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "3f968153-b894-4f70-b451-e66864c0aa4f");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "461ec216-256d-49b4-9fed-590bb65d2302");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "5fce013b-4924-45ff-b374-c0e8c40a845d");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "8137943d-1017-4139-8841-d310f33f7c6e");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "f4d7576b-17be-42c5-be70-e6a25e7e7b19");

            migrationBuilder.DropColumn(
                name: "Coffers",
                table: "Commands");

            migrationBuilder.DropColumn(
                name: "Warriors",
                table: "Commands");

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "OrganizationId", "TargetOrganizationId", "TurnId", "Type" },
                values: new object[,]
                {
                    { "49274109-cdb7-49ce-8eed-22185d00815f", "TinMines", null, 0, 0 },
                    { "ff4ffd34-025c-4564-a076-6210a4e07f77", "CapeRaptor", null, 0, 0 },
                    { "9273c315-bfa6-4c77-87e7-f6181eb5e8eb", "MouthOfPolaima", null, 0, 0 },
                    { "c7eabd63-40e2-4ab7-be39-0d7fb1b9115a", "HeatherOfDimmoria", null, 0, 0 },
                    { "deec4204-32e1-4506-a51b-9ca7e4b77ec1", "DimmoriaValley", null, 0, 0 },
                    { "3ad9b544-3655-4ced-acb7-498495a9deed", "SummerCoast", null, 0, 0 },
                    { "8d56b9fa-19cd-4719-a801-f447f92b5653", "DimmoriaFarms", null, 0, 0 },
                    { "fd3b9fd8-0cd6-48d4-9758-d001286607bf", "ChalRocks", null, 0, 0 },
                    { "22f555ad-13dd-48dd-930a-37918875a2e7", "LimestoneRidges", null, 0, 0 }
                });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 4, 6, 7, 19, 16, 586, DateTimeKind.Utc).AddTicks(5546));
        }
    }
}
