using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YAGO.World.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChoiceValueToColonyEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChoiceValue",
                table: "ColonyEvents",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ColonyEvents_EventCode",
                table: "ColonyEvents",
                column: "EventCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ColonyEvents_EventCode",
                table: "ColonyEvents");

            migrationBuilder.DropColumn(
                name: "ChoiceValue",
                table: "ColonyEvents");
        }
    }
}
