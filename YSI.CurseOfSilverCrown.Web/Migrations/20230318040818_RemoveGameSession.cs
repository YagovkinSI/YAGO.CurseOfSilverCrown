using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class RemoveGameSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameSessions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndSeesionTurnId = table.Column<int>(type: "int", nullable: false),
                    NumberOfGame = table.Column<int>(type: "int", nullable: false),
                    StartSeesionTurnId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "GameSessions",
                columns: new[] { "Id", "EndSeesionTurnId", "NumberOfGame", "StartSeesionTurnId" },
                values: new object[] { 1, 2147483647, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_EndSeesionTurnId",
                table: "GameSessions",
                column: "EndSeesionTurnId");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_StartSeesionTurnId",
                table: "GameSessions",
                column: "StartSeesionTurnId");
        }
    }
}
