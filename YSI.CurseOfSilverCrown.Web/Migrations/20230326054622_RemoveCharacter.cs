using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class RemoveCharacter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Characters_CharacterId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Commands_Characters_PersonInitiatorId",
                table: "Commands");

            migrationBuilder.DropForeignKey(
                name: "FK_Domains_Characters_OwnerId",
                table: "Domains");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Characters_InitiatorCharacterId",
                table: "Units");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Units_InitiatorCharacterId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Domains_OwnerId",
                table: "Domains");

            migrationBuilder.DropIndex(
                name: "IX_Commands_InitiatorCharacterId",
                table: "Commands");

            migrationBuilder.DropIndex(
                name: "IX_Commands_PersonInitiatorId",
                table: "Commands");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CharacterId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InitiatorCharacterId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Domains");

            migrationBuilder.DropColumn(
                name: "InitiatorCharacterId",
                table: "Commands");

            migrationBuilder.DropColumn(
                name: "PersonInitiatorId",
                table: "Commands");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Domains",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Domains_UserId",
                table: "Domains",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Commands_ExecutorType_ExecutorId",
                table: "Commands",
                columns: new[] { "ExecutorType", "ExecutorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Domains_AspNetUsers_UserId",
                table: "Domains",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Domains_AspNetUsers_UserId",
                table: "Domains");

            migrationBuilder.DropIndex(
                name: "IX_Domains_UserId",
                table: "Domains");

            migrationBuilder.DropIndex(
                name: "IX_Commands_ExecutorType_ExecutorId",
                table: "Commands");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Domains");

            migrationBuilder.AddColumn<int>(
                name: "InitiatorCharacterId",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Domains",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InitiatorCharacterId",
                table: "Commands",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PersonInitiatorId",
                table: "Commands",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "CharacterJson", "Name" },
                values: new object[,]
                {
                    { 1, null, "Эйгон 1" },
                    { 76, null, "Эйгон 76" },
                    { 75, null, "Эйгон 75" },
                    { 74, null, "Эйгон 74" },
                    { 73, null, "Эйгон 73" },
                    { 72, null, "Эйгон 72" },
                    { 71, null, "Эйгон 71" },
                    { 70, null, "Эйгон 70" },
                    { 69, null, "Эйгон 69" },
                    { 68, null, "Эйгон 68" },
                    { 67, null, "Эйгон 67" },
                    { 77, null, "Эйгон 77" },
                    { 66, null, "Эйгон 66" },
                    { 64, null, "Эйгон 64" },
                    { 63, null, "Эйгон 63" },
                    { 62, null, "Эйгон 62" },
                    { 61, null, "Эйгон 61" },
                    { 60, null, "Эйгон 60" },
                    { 59, null, "Эйгон 59" },
                    { 58, null, "Эйгон 58" },
                    { 57, null, "Эйгон 57" },
                    { 56, null, "Эйгон 56" },
                    { 55, null, "Эйгон 55" },
                    { 65, null, "Эйгон 65" },
                    { 78, null, "Эйгон 78" },
                    { 79, null, "Эйгон 79" },
                    { 80, null, "Эйгон 80" },
                    { 106, null, "Эйгон 106" },
                    { 105, null, "Эйгон 105" },
                    { 101, null, "Эйгон 101" },
                    { 100, null, "Эйгон 100" },
                    { 99, null, "Эйгон 99" },
                    { 98, null, "Эйгон 98" },
                    { 97, null, "Эйгон 97" },
                    { 96, null, "Эйгон 96" },
                    { 95, null, "Эйгон 95" },
                    { 94, null, "Эйгон 94" },
                    { 93, null, "Эйгон 93" },
                    { 92, null, "Эйгон 92" },
                    { 91, null, "Эйгон 91" },
                    { 90, null, "Эйгон 90" },
                    { 89, null, "Эйгон 89" }
                });

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "CharacterJson", "Name" },
                values: new object[,]
                {
                    { 88, null, "Эйгон 88" },
                    { 87, null, "Эйгон 87" },
                    { 86, null, "Эйгон 86" },
                    { 85, null, "Эйгон 85" },
                    { 84, null, "Эйгон 84" },
                    { 83, null, "Эйгон 83" },
                    { 82, null, "Эйгон 82" },
                    { 81, null, "Эйгон 81" },
                    { 54, null, "Эйгон 54" },
                    { 107, null, "Эйгон 107" },
                    { 53, null, "Эйгон 53" },
                    { 51, null, "Эйгон 51" },
                    { 23, null, "Эйгон 23" },
                    { 22, null, "Эйгон 22" },
                    { 21, null, "Эйгон 21" },
                    { 20, null, "Эйгон 20" },
                    { 19, null, "Эйгон 19" },
                    { 18, null, "Эйгон 18" },
                    { 17, null, "Эйгон 17" },
                    { 16, null, "Эйгон 16" },
                    { 15, null, "Эйгон 15" },
                    { 14, null, "Эйгон 14" },
                    { 24, null, "Эйгон 24" },
                    { 13, null, "Эйгон 13" },
                    { 11, null, "Эйгон 11" },
                    { 10, null, "Эйгон 10" },
                    { 9, null, "Эйгон 9" },
                    { 8, null, "Эйгон 8" },
                    { 7, null, "Эйгон 7" },
                    { 6, null, "Эйгон 6" },
                    { 5, null, "Эйгон 5" },
                    { 4, null, "Эйгон 4" },
                    { 3, null, "Эйгон 3" },
                    { 2, null, "Эйгон 2" },
                    { 12, null, "Эйгон 12" },
                    { 25, null, "Эйгон 25" },
                    { 26, null, "Эйгон 26" },
                    { 27, null, "Эйгон 27" },
                    { 50, null, "Эйгон 50" },
                    { 49, null, "Эйгон 49" },
                    { 48, null, "Эйгон 48" },
                    { 47, null, "Эйгон 47" }
                });

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "CharacterJson", "Name" },
                values: new object[,]
                {
                    { 46, null, "Эйгон 46" },
                    { 45, null, "Эйгон 45" },
                    { 44, null, "Эйгон 44" },
                    { 43, null, "Эйгон 43" },
                    { 42, null, "Эйгон 42" },
                    { 41, null, "Эйгон 41" },
                    { 40, null, "Эйгон 40" },
                    { 39, null, "Эйгон 39" },
                    { 38, null, "Эйгон 38" },
                    { 37, null, "Эйгон 37" },
                    { 36, null, "Эйгон 36" },
                    { 35, null, "Эйгон 35" },
                    { 34, null, "Эйгон 34" },
                    { 33, null, "Эйгон 33" },
                    { 32, null, "Эйгон 32" },
                    { 31, null, "Эйгон 31" },
                    { 30, null, "Эйгон 30" },
                    { 29, null, "Эйгон 29" },
                    { 28, null, "Эйгон 28" },
                    { 52, null, "Эйгон 52" },
                    { 108, null, "Эйгон 108" }
                });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 1,
                column: "OwnerId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 2,
                column: "OwnerId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 3,
                column: "OwnerId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 4,
                column: "OwnerId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 5,
                column: "OwnerId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 6,
                column: "OwnerId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 7,
                column: "OwnerId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 8,
                column: "OwnerId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 9,
                column: "OwnerId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 10,
                column: "OwnerId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 11,
                column: "OwnerId",
                value: 11);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 12,
                column: "OwnerId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 13,
                column: "OwnerId",
                value: 13);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 14,
                column: "OwnerId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 15,
                column: "OwnerId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 16,
                column: "OwnerId",
                value: 16);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 17,
                column: "OwnerId",
                value: 17);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 18,
                column: "OwnerId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 19,
                column: "OwnerId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 20,
                column: "OwnerId",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 21,
                column: "OwnerId",
                value: 21);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 22,
                column: "OwnerId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 23,
                column: "OwnerId",
                value: 23);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 24,
                column: "OwnerId",
                value: 24);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 25,
                column: "OwnerId",
                value: 25);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 26,
                column: "OwnerId",
                value: 26);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 27,
                column: "OwnerId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 28,
                column: "OwnerId",
                value: 28);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 29,
                column: "OwnerId",
                value: 29);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 30,
                column: "OwnerId",
                value: 30);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 31,
                column: "OwnerId",
                value: 31);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 32,
                column: "OwnerId",
                value: 32);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 33,
                column: "OwnerId",
                value: 33);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 34,
                column: "OwnerId",
                value: 34);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 35,
                column: "OwnerId",
                value: 35);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 36,
                column: "OwnerId",
                value: 36);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 37,
                column: "OwnerId",
                value: 37);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 38,
                column: "OwnerId",
                value: 38);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 39,
                column: "OwnerId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 40,
                column: "OwnerId",
                value: 40);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 41,
                column: "OwnerId",
                value: 41);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 42,
                column: "OwnerId",
                value: 42);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 43,
                column: "OwnerId",
                value: 43);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 44,
                column: "OwnerId",
                value: 44);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 45,
                column: "OwnerId",
                value: 45);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 46,
                column: "OwnerId",
                value: 46);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 47,
                column: "OwnerId",
                value: 47);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 48,
                column: "OwnerId",
                value: 48);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 49,
                column: "OwnerId",
                value: 49);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 50,
                column: "OwnerId",
                value: 50);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 51,
                column: "OwnerId",
                value: 51);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 52,
                column: "OwnerId",
                value: 52);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 53,
                column: "OwnerId",
                value: 53);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 54,
                column: "OwnerId",
                value: 54);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 55,
                column: "OwnerId",
                value: 55);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 56,
                column: "OwnerId",
                value: 56);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 57,
                column: "OwnerId",
                value: 57);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 58,
                column: "OwnerId",
                value: 58);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 59,
                column: "OwnerId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 60,
                column: "OwnerId",
                value: 60);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 61,
                column: "OwnerId",
                value: 61);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 62,
                column: "OwnerId",
                value: 62);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 63,
                column: "OwnerId",
                value: 63);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 64,
                column: "OwnerId",
                value: 64);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 65,
                column: "OwnerId",
                value: 65);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 66,
                column: "OwnerId",
                value: 66);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 67,
                column: "OwnerId",
                value: 67);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 68,
                column: "OwnerId",
                value: 68);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 69,
                column: "OwnerId",
                value: 69);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 70,
                column: "OwnerId",
                value: 70);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 71,
                column: "OwnerId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 72,
                column: "OwnerId",
                value: 72);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 73,
                column: "OwnerId",
                value: 73);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 74,
                column: "OwnerId",
                value: 74);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 75,
                column: "OwnerId",
                value: 75);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 76,
                column: "OwnerId",
                value: 76);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 77,
                column: "OwnerId",
                value: 77);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 78,
                column: "OwnerId",
                value: 78);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 79,
                column: "OwnerId",
                value: 79);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 80,
                column: "OwnerId",
                value: 80);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 81,
                column: "OwnerId",
                value: 81);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 82,
                column: "OwnerId",
                value: 82);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 83,
                column: "OwnerId",
                value: 83);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 84,
                column: "OwnerId",
                value: 84);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 85,
                column: "OwnerId",
                value: 85);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 86,
                column: "OwnerId",
                value: 86);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 87,
                column: "OwnerId",
                value: 87);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 88,
                column: "OwnerId",
                value: 88);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 89,
                column: "OwnerId",
                value: 89);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 90,
                column: "OwnerId",
                value: 90);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 91,
                column: "OwnerId",
                value: 91);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 92,
                column: "OwnerId",
                value: 92);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 93,
                column: "OwnerId",
                value: 93);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 94,
                column: "OwnerId",
                value: 94);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 95,
                column: "OwnerId",
                value: 95);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 96,
                column: "OwnerId",
                value: 96);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 97,
                column: "OwnerId",
                value: 97);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 98,
                column: "OwnerId",
                value: 98);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 99,
                column: "OwnerId",
                value: 99);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 100,
                column: "OwnerId",
                value: 100);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 101,
                column: "OwnerId",
                value: 101);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 105,
                column: "OwnerId",
                value: 105);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 106,
                column: "OwnerId",
                value: 106);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 107,
                column: "OwnerId",
                value: 107);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 108,
                column: "OwnerId",
                value: 108);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1,
                column: "InitiatorCharacterId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2,
                column: "InitiatorCharacterId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3,
                column: "InitiatorCharacterId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4,
                column: "InitiatorCharacterId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5,
                column: "InitiatorCharacterId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 6,
                column: "InitiatorCharacterId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 7,
                column: "InitiatorCharacterId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 8,
                column: "InitiatorCharacterId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 9,
                column: "InitiatorCharacterId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 10,
                column: "InitiatorCharacterId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 11,
                column: "InitiatorCharacterId",
                value: 11);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 12,
                column: "InitiatorCharacterId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 13,
                column: "InitiatorCharacterId",
                value: 13);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 14,
                column: "InitiatorCharacterId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 15,
                column: "InitiatorCharacterId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 16,
                column: "InitiatorCharacterId",
                value: 16);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 17,
                column: "InitiatorCharacterId",
                value: 17);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 18,
                column: "InitiatorCharacterId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 19,
                column: "InitiatorCharacterId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 20,
                column: "InitiatorCharacterId",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 21,
                column: "InitiatorCharacterId",
                value: 21);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 22,
                column: "InitiatorCharacterId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 23,
                column: "InitiatorCharacterId",
                value: 23);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 24,
                column: "InitiatorCharacterId",
                value: 24);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 25,
                column: "InitiatorCharacterId",
                value: 25);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 26,
                column: "InitiatorCharacterId",
                value: 26);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 27,
                column: "InitiatorCharacterId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 28,
                column: "InitiatorCharacterId",
                value: 28);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 29,
                column: "InitiatorCharacterId",
                value: 29);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 30,
                column: "InitiatorCharacterId",
                value: 30);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 31,
                column: "InitiatorCharacterId",
                value: 31);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 32,
                column: "InitiatorCharacterId",
                value: 32);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 33,
                column: "InitiatorCharacterId",
                value: 33);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 34,
                column: "InitiatorCharacterId",
                value: 34);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 35,
                column: "InitiatorCharacterId",
                value: 35);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 36,
                column: "InitiatorCharacterId",
                value: 36);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 37,
                column: "InitiatorCharacterId",
                value: 37);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 38,
                column: "InitiatorCharacterId",
                value: 38);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 39,
                column: "InitiatorCharacterId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 40,
                column: "InitiatorCharacterId",
                value: 40);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 41,
                column: "InitiatorCharacterId",
                value: 41);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 42,
                column: "InitiatorCharacterId",
                value: 42);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 43,
                column: "InitiatorCharacterId",
                value: 43);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 44,
                column: "InitiatorCharacterId",
                value: 44);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 45,
                column: "InitiatorCharacterId",
                value: 45);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 46,
                column: "InitiatorCharacterId",
                value: 46);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 47,
                column: "InitiatorCharacterId",
                value: 47);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 48,
                column: "InitiatorCharacterId",
                value: 48);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 49,
                column: "InitiatorCharacterId",
                value: 49);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 50,
                column: "InitiatorCharacterId",
                value: 50);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 51,
                column: "InitiatorCharacterId",
                value: 51);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 52,
                column: "InitiatorCharacterId",
                value: 52);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 53,
                column: "InitiatorCharacterId",
                value: 53);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 54,
                column: "InitiatorCharacterId",
                value: 54);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 55,
                column: "InitiatorCharacterId",
                value: 55);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 56,
                column: "InitiatorCharacterId",
                value: 56);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 57,
                column: "InitiatorCharacterId",
                value: 57);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 58,
                column: "InitiatorCharacterId",
                value: 58);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 59,
                column: "InitiatorCharacterId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 60,
                column: "InitiatorCharacterId",
                value: 60);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 61,
                column: "InitiatorCharacterId",
                value: 61);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 62,
                column: "InitiatorCharacterId",
                value: 62);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 63,
                column: "InitiatorCharacterId",
                value: 63);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 64,
                column: "InitiatorCharacterId",
                value: 64);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 65,
                column: "InitiatorCharacterId",
                value: 65);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 66,
                column: "InitiatorCharacterId",
                value: 66);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 67,
                column: "InitiatorCharacterId",
                value: 67);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 68,
                column: "InitiatorCharacterId",
                value: 68);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 69,
                column: "InitiatorCharacterId",
                value: 69);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 70,
                column: "InitiatorCharacterId",
                value: 70);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 71,
                column: "InitiatorCharacterId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 72,
                column: "InitiatorCharacterId",
                value: 72);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 73,
                column: "InitiatorCharacterId",
                value: 73);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 74,
                column: "InitiatorCharacterId",
                value: 74);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 75,
                column: "InitiatorCharacterId",
                value: 75);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 76,
                column: "InitiatorCharacterId",
                value: 76);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 77,
                column: "InitiatorCharacterId",
                value: 77);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 78,
                column: "InitiatorCharacterId",
                value: 78);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 79,
                column: "InitiatorCharacterId",
                value: 79);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 80,
                column: "InitiatorCharacterId",
                value: 80);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 81,
                column: "InitiatorCharacterId",
                value: 81);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 82,
                column: "InitiatorCharacterId",
                value: 82);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 83,
                column: "InitiatorCharacterId",
                value: 83);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 84,
                column: "InitiatorCharacterId",
                value: 84);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 85,
                column: "InitiatorCharacterId",
                value: 85);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 86,
                column: "InitiatorCharacterId",
                value: 86);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 87,
                column: "InitiatorCharacterId",
                value: 87);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 88,
                column: "InitiatorCharacterId",
                value: 88);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 89,
                column: "InitiatorCharacterId",
                value: 89);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 90,
                column: "InitiatorCharacterId",
                value: 90);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 91,
                column: "InitiatorCharacterId",
                value: 91);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 92,
                column: "InitiatorCharacterId",
                value: 92);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 93,
                column: "InitiatorCharacterId",
                value: 93);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 94,
                column: "InitiatorCharacterId",
                value: 94);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 95,
                column: "InitiatorCharacterId",
                value: 95);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 96,
                column: "InitiatorCharacterId",
                value: 96);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 97,
                column: "InitiatorCharacterId",
                value: 97);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 98,
                column: "InitiatorCharacterId",
                value: 98);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 99,
                column: "InitiatorCharacterId",
                value: 99);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 100,
                column: "InitiatorCharacterId",
                value: 100);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 101,
                column: "InitiatorCharacterId",
                value: 101);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 105,
                column: "InitiatorCharacterId",
                value: 105);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 106,
                column: "InitiatorCharacterId",
                value: 106);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 107,
                column: "InitiatorCharacterId",
                value: 107);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 108,
                column: "InitiatorCharacterId",
                value: 108);

            migrationBuilder.CreateIndex(
                name: "IX_Units_InitiatorCharacterId",
                table: "Units",
                column: "InitiatorCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Domains_OwnerId",
                table: "Domains",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Commands_InitiatorCharacterId",
                table: "Commands",
                column: "InitiatorCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Commands_PersonInitiatorId",
                table: "Commands",
                column: "PersonInitiatorId");

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
                name: "FK_Commands_Characters_PersonInitiatorId",
                table: "Commands",
                column: "PersonInitiatorId",
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
    }
}
