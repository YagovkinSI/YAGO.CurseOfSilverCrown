using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YAGO.World.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStatesJsonToColony : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Population",
                table: "Colonies");

            migrationBuilder.DropColumn(
                name: "Reputation",
                table: "Colonies");

            migrationBuilder.DropColumn(
                name: "ZonesOccupied",
                table: "Colonies");

            migrationBuilder.RenameColumn(
                name: "SolarsIncome",
                table: "Colonies",
                newName: "ReputationByEvents");

            migrationBuilder.AddColumn<string>(
                name: "StatesJson",
                table: "Colonies",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatesJson",
                table: "Colonies");

            migrationBuilder.RenameColumn(
                name: "ReputationByEvents",
                table: "Colonies",
                newName: "SolarsIncome");

            migrationBuilder.AddColumn<int>(
                name: "Population",
                table: "Colonies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Reputation",
                table: "Colonies",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ZonesOccupied",
                table: "Colonies",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
