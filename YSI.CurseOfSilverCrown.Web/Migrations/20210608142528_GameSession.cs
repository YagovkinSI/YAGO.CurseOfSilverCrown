using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class GameSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndOfLastGameTurnId",
                table: "Turns");

            migrationBuilder.DropColumn(
                name: "NumberOfGame",
                table: "Turns");

            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartSeesionTurnId = table.Column<int>(type: "int", nullable: false),
                    EndSeesionTurnId = table.Column<int>(type: "int", nullable: false),
                    NumberOfGame = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_EndSeesionTurnId",
                table: "GameSessions",
                column: "EndSeesionTurnId");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_StartSeesionTurnId",
                table: "GameSessions",
                column: "StartSeesionTurnId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameSessions");

            migrationBuilder.AddColumn<int>(
                name: "EndOfLastGameTurnId",
                table: "Turns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfGame",
                table: "Turns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
