using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Data.Migrations
{
    public partial class UserLastActivityDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "Power",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "TurnId",
                table: "Commands");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivityTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "Coffers", "OrganizationId", "TargetOrganizationId", "Type", "Warriors" },
                values: new object[,]
                {
                    { "126ef807-7c48-4c9b-bafb-a67ac5e30663", 0, "TinMines", null, 0, 0 },
                    { "cdb29eaa-ae45-45aa-a9ac-9f35f634da19", 0, "CapeRaptor", null, 0, 0 },
                    { "e4ba5409-28bd-4dbf-ba66-b3d1720f950a", 0, "MouthOfPolaima", null, 0, 0 },
                    { "3ceaf5d1-15ff-4b6d-8291-9a0646c59b53", 0, "HeatherOfDimmoria", null, 0, 0 },
                    { "0441c49c-bbc7-4f64-b213-4e567ed62eb6", 0, "DimmoriaValley", null, 0, 0 },
                    { "0d7ec146-48a0-423b-8173-70398b102040", 0, "SummerCoast", null, 0, 0 },
                    { "a2d27f52-71af-43c0-9559-5ccb5ec9ba59", 0, "DimmoriaFarms", null, 0, 0 },
                    { "6550b859-f22a-4ce9-8a99-96463ad24a41", 0, "ChalRocks", null, 0, 0 },
                    { "d5cd667c-fc0e-4f9a-b03d-598af85f1bad", 0, "LimestoneRidges", null, 0, 0 }
                });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 4, 16, 15, 7, 21, 823, DateTimeKind.Utc).AddTicks(3098));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "0441c49c-bbc7-4f64-b213-4e567ed62eb6");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "0d7ec146-48a0-423b-8173-70398b102040");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "126ef807-7c48-4c9b-bafb-a67ac5e30663");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "3ceaf5d1-15ff-4b6d-8291-9a0646c59b53");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "6550b859-f22a-4ce9-8a99-96463ad24a41");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "a2d27f52-71af-43c0-9559-5ccb5ec9ba59");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "cdb29eaa-ae45-45aa-a9ac-9f35f634da19");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "d5cd667c-fc0e-4f9a-b03d-598af85f1bad");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "e4ba5409-28bd-4dbf-ba66-b3d1720f950a");

            migrationBuilder.DropColumn(
                name: "LastActivityTime",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Power",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TurnId",
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
    }
}
