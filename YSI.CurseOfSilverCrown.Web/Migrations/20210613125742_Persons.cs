using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class Persons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "Domains",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Эйгон 1" },
                    { 76, "Эйгон 76" },
                    { 75, "Эйгон 75" },
                    { 74, "Эйгон 74" },
                    { 73, "Эйгон 73" },
                    { 72, "Эйгон 72" },
                    { 71, "Эйгон 71" },
                    { 70, "Эйгон 70" },
                    { 69, "Эйгон 69" },
                    { 68, "Эйгон 68" },
                    { 67, "Эйгон 67" },
                    { 77, "Эйгон 77" },
                    { 66, "Эйгон 66" },
                    { 64, "Эйгон 64" },
                    { 63, "Эйгон 63" },
                    { 62, "Эйгон 62" },
                    { 61, "Эйгон 61" },
                    { 60, "Эйгон 60" },
                    { 59, "Эйгон 59" },
                    { 58, "Эйгон 58" },
                    { 57, "Эйгон 57" },
                    { 56, "Эйгон 56" },
                    { 55, "Эйгон 55" },
                    { 65, "Эйгон 65" },
                    { 78, "Эйгон 78" },
                    { 79, "Эйгон 79" },
                    { 80, "Эйгон 80" },
                    { 106, "Эйгон 106" },
                    { 105, "Эйгон 105" },
                    { 101, "Эйгон 101" },
                    { 100, "Эйгон 100" },
                    { 99, "Эйгон 99" },
                    { 98, "Эйгон 98" },
                    { 97, "Эйгон 97" },
                    { 96, "Эйгон 96" },
                    { 95, "Эйгон 95" },
                    { 94, "Эйгон 94" },
                    { 93, "Эйгон 93" },
                    { 92, "Эйгон 92" },
                    { 91, "Эйгон 91" },
                    { 90, "Эйгон 90" },
                    { 89, "Эйгон 89" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 88, "Эйгон 88" },
                    { 87, "Эйгон 87" },
                    { 86, "Эйгон 86" },
                    { 85, "Эйгон 85" },
                    { 84, "Эйгон 84" },
                    { 83, "Эйгон 83" },
                    { 82, "Эйгон 82" },
                    { 81, "Эйгон 81" },
                    { 54, "Эйгон 54" },
                    { 107, "Эйгон 107" },
                    { 53, "Эйгон 53" },
                    { 51, "Эйгон 51" },
                    { 23, "Эйгон 23" },
                    { 22, "Эйгон 22" },
                    { 21, "Эйгон 21" },
                    { 20, "Эйгон 20" },
                    { 19, "Эйгон 19" },
                    { 18, "Эйгон 18" },
                    { 17, "Эйгон 17" },
                    { 16, "Эйгон 16" },
                    { 15, "Эйгон 15" },
                    { 14, "Эйгон 14" },
                    { 24, "Эйгон 24" },
                    { 13, "Эйгон 13" },
                    { 11, "Эйгон 11" },
                    { 10, "Эйгон 10" },
                    { 9, "Эйгон 9" },
                    { 8, "Эйгон 8" },
                    { 7, "Эйгон 7" },
                    { 6, "Эйгон 6" },
                    { 5, "Эйгон 5" },
                    { 4, "Эйгон 4" },
                    { 3, "Эйгон 3" },
                    { 2, "Эйгон 2" },
                    { 12, "Эйгон 12" },
                    { 25, "Эйгон 25" },
                    { 26, "Эйгон 26" },
                    { 27, "Эйгон 27" },
                    { 50, "Эйгон 50" },
                    { 49, "Эйгон 49" },
                    { 48, "Эйгон 48" },
                    { 47, "Эйгон 47" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 46, "Эйгон 46" },
                    { 45, "Эйгон 45" },
                    { 44, "Эйгон 44" },
                    { 43, "Эйгон 43" },
                    { 42, "Эйгон 42" },
                    { 41, "Эйгон 41" },
                    { 40, "Эйгон 40" },
                    { 39, "Эйгон 39" },
                    { 38, "Эйгон 38" },
                    { 37, "Эйгон 37" },
                    { 36, "Эйгон 36" },
                    { 35, "Эйгон 35" },
                    { 34, "Эйгон 34" },
                    { 33, "Эйгон 33" },
                    { 32, "Эйгон 32" },
                    { 31, "Эйгон 31" },
                    { 30, "Эйгон 30" },
                    { 29, "Эйгон 29" },
                    { 28, "Эйгон 28" },
                    { 52, "Эйгон 52" },
                    { 108, "Эйгон 108" }
                });

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
                name: "FK_AspNetUsers_Persons_PersonId",
                table: "AspNetUsers",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Domains_Persons_PersonId",
                table: "Domains",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Persons_PersonId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Domains_Persons_PersonId",
                table: "Domains");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Domains_PersonId",
                table: "Domains");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PersonId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Domains");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "AspNetUsers");
        }
    }
}
