using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class RenameFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Characters_PersonId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Domains_Characters_PersonId",
                table: "Domains");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Characters_InitiatorPersonId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Domains_MoveOrder",
                table: "Domains");

            migrationBuilder.DropIndex(
                name: "IX_Domains_PersonId",
                table: "Domains");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PersonId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "InitiatorPersonId",
                table: "Units",
                newName: "InitiatorCharacterId");

            migrationBuilder.RenameColumn(
                name: "Coffers",
                table: "Units",
                newName: "Gold");

            migrationBuilder.RenameIndex(
                name: "IX_Units_InitiatorPersonId",
                table: "Units",
                newName: "IX_Units_InitiatorCharacterId");

            migrationBuilder.RenameColumn(
                name: "PermissionOfPassage",
                table: "Relations",
                newName: "Defense");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Domains",
                newName: "Size");

            migrationBuilder.RenameColumn(
                name: "MoveOrder",
                table: "Domains",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "Coffers",
                table: "Domains",
                newName: "Gold");

            migrationBuilder.RenameColumn(
                name: "InitiatorPersonId",
                table: "Commands",
                newName: "InitiatorCharacterId");

            migrationBuilder.RenameColumn(
                name: "Coffers",
                table: "Commands",
                newName: "Gold");

            migrationBuilder.RenameIndex(
                name: "IX_Commands_InitiatorPersonId",
                table: "Commands",
                newName: "IX_Commands_InitiatorCharacterId");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "AspNetUsers",
                newName: "CharacterId");

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 1, 1010 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 2, 1510 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 3, 1310 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 4, 3010 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 5, 4010 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 6, 5510 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 7, 5010 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 8, 3510 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 9, 3310 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 10, 1810 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 11, 3810 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 12, 3710 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 13, 2010 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 14, 5310 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 15, 5210 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 16, 4510 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 17, 4810 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 18, 2510 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 19, 2810 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 20, 5810 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 21, 5910 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 22, 4310 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 23, 5410 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 24, 5710 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 25, 5610 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 26, 3610 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 27, 3410 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 28, 4610 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 29, 6010 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 30, 5110 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 31, 1520 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 32, 1020 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 33, 3020 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 34, 1820 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 35, 1720 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 36, 1420 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 37, 1620 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 38, 2020 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 39, 3520 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 40, 520 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 41, 820 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 42, 320 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 43, 1320 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 44, 620 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 45, 1530 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 46, 830 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 47, 1030 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 48, 530 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 49, 630 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 50, 3030 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 51, 2030 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 52, 2530 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 53, 2330 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 54, 3530 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 55, 2830 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 56, 1830 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 57, 1730 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 58, 1930 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 59, 3830 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 60, 1630 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 61, 930 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 62, 1430 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 63, 330 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 64, 2130 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 65, 3330 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 66, 5040 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 67, 3040 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 68, 3440 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 69, 2040 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 70, 4040 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 71, 3540 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 72, 2540 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 73, 2840 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 74, 4540 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 75, 3840 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 76, 1040 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 77, 1540 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 78, 3340 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 79, 1840 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 80, 1340 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 81, 2340 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 82, 1740 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 83, 2640 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 84, 3740 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 85,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 85, 2050 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 86, 2550 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 87, 1050 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 88, 2850 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 89,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 89, 2350 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 90, 2750 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 91, 1550 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 92, 1850 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 93, 3050 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 94, 1350 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 95, 2250 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 96,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 96, 3550 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 97,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 97, 2650 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 98,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 98, 2150 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 99, 1650 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 100, 3850 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 101, 3350 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 105, 3950 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 106, 2950 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 107,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 107, 2450 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 108,
                columns: new[] { "OwnerId", "Size" },
                values: new object[] { 108, 2055 });

            migrationBuilder.CreateIndex(
                name: "IX_Domains_OwnerId",
                table: "Domains",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Domains_Size",
                table: "Domains",
                column: "Size",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CharacterId",
                table: "AspNetUsers",
                column: "CharacterId",
                unique: true,
                filter: "[CharacterId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Characters_CharacterId",
                table: "AspNetUsers",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Domains_Characters_OwnerId",
                table: "Domains",
                column: "OwnerId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Characters_InitiatorCharacterId",
                table: "Units",
                column: "InitiatorCharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Characters_CharacterId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Domains_Characters_OwnerId",
                table: "Domains");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Characters_InitiatorCharacterId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Domains_OwnerId",
                table: "Domains");

            migrationBuilder.DropIndex(
                name: "IX_Domains_Size",
                table: "Domains");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CharacterId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "InitiatorCharacterId",
                table: "Units",
                newName: "InitiatorPersonId");

            migrationBuilder.RenameColumn(
                name: "Gold",
                table: "Units",
                newName: "Coffers");

            migrationBuilder.RenameIndex(
                name: "IX_Units_InitiatorCharacterId",
                table: "Units",
                newName: "IX_Units_InitiatorPersonId");

            migrationBuilder.RenameColumn(
                name: "Defense",
                table: "Relations",
                newName: "PermissionOfPassage");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "Domains",
                newName: "PersonId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Domains",
                newName: "MoveOrder");

            migrationBuilder.RenameColumn(
                name: "Gold",
                table: "Domains",
                newName: "Coffers");

            migrationBuilder.RenameColumn(
                name: "InitiatorCharacterId",
                table: "Commands",
                newName: "InitiatorPersonId");

            migrationBuilder.RenameColumn(
                name: "Gold",
                table: "Commands",
                newName: "Coffers");

            migrationBuilder.RenameIndex(
                name: "IX_Commands_InitiatorCharacterId",
                table: "Commands",
                newName: "IX_Commands_InitiatorPersonId");

            migrationBuilder.RenameColumn(
                name: "CharacterId",
                table: "AspNetUsers",
                newName: "PersonId");

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1010, 1 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1510, 2 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1310, 3 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3010, 4 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 4010, 5 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 5510, 6 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 5010, 7 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3510, 8 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3310, 9 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1810, 10 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3810, 11 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3710, 12 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2010, 13 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 5310, 14 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 5210, 15 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 4510, 16 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 4810, 17 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2510, 18 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2810, 19 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 5810, 20 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 5910, 21 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 4310, 22 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 5410, 23 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 5710, 24 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 5610, 25 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3610, 26 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3410, 27 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 4610, 28 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 6010, 29 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 5110, 30 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1520, 31 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1020, 32 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3020, 33 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1820, 34 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1720, 35 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1420, 36 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1620, 37 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2020, 38 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3520, 39 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 520, 40 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 820, 41 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 320, 42 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1320, 43 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 620, 44 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1530, 45 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 830, 46 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1030, 47 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 530, 48 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 630, 49 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3030, 50 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2030, 51 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2530, 52 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2330, 53 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3530, 54 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2830, 55 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1830, 56 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1730, 57 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1930, 58 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3830, 59 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1630, 60 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 930, 61 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1430, 62 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 330, 63 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2130, 64 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3330, 65 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 5040, 66 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3040, 67 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3440, 68 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2040, 69 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 4040, 70 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3540, 71 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2540, 72 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2840, 73 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 4540, 74 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3840, 75 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1040, 76 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1540, 77 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3340, 78 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1840, 79 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1340, 80 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2340, 81 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1740, 82 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2640, 83 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3740, 84 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 85,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2050, 85 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2550, 86 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1050, 87 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2850, 88 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 89,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2350, 89 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2750, 90 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1550, 91 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1850, 92 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3050, 93 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1350, 94 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2250, 95 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 96,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3550, 96 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 97,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2650, 97 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 98,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2150, 98 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 1650, 99 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3850, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3350, 101 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 3950, 105 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2950, 106 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 107,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2450, 107 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 108,
                columns: new[] { "MoveOrder", "PersonId" },
                values: new object[] { 2055, 108 });

            migrationBuilder.CreateIndex(
                name: "IX_Domains_MoveOrder",
                table: "Domains",
                column: "MoveOrder",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Domains_PersonId",
                table: "Domains",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PersonId",
                table: "AspNetUsers",
                column: "PersonId",
                unique: true,
                filter: "[PersonId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Characters_PersonId",
                table: "AspNetUsers",
                column: "PersonId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Domains_Characters_PersonId",
                table: "Domains",
                column: "PersonId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Characters_InitiatorPersonId",
                table: "Units",
                column: "InitiatorPersonId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
