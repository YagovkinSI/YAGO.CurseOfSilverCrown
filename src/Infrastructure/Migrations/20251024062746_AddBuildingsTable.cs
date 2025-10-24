using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YAGO.World.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBuildingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZonesTotal",
                table: "Colonies");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Colonies",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuildingIdsJson",
                table: "Colonies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    ZonesOccupied = table.Column<int>(type: "integer", nullable: false),
                    SolarsIncome = table.Column<decimal>(type: "numeric", nullable: false),
                    Reputation = table.Column<decimal>(type: "numeric", nullable: false),
                    Population = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropColumn(
                name: "BuildingIdsJson",
                table: "Colonies");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Colonies",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "ZonesTotal",
                table: "Colonies",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
