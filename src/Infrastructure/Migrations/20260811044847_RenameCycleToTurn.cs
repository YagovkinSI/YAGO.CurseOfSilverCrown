using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YAGO.World.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCycleToTurn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cycles");

            migrationBuilder.DropColumn(
                name: "DeactivateAtUtc",
                table: "Colonies");

            migrationBuilder.DropColumn(
                name: "Deactivated",
                table: "Colonies");

            migrationBuilder.RenameColumn(
                name: "StatesJson",
                table: "Colonies",
                newName: "JsonData");

            migrationBuilder.CreateTable(
                name: "Turns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ColonyId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RunAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsComplited = table.Column<bool>(type: "boolean", nullable: false),
                    JsonData = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Turns_Colonies_ColonyId",
                        column: x => x.ColonyId,
                        principalTable: "Colonies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Turns_ColonyId",
                table: "Turns",
                column: "ColonyId");

            migrationBuilder.CreateIndex(
                name: "IX_Turns_RunAtUtc",
                table: "Turns",
                column: "RunAtUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Turns");

            migrationBuilder.RenameColumn(
                name: "JsonData",
                table: "Colonies",
                newName: "StatesJson");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivateAtUtc",
                table: "Colonies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deactivated",
                table: "Colonies",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Cycles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ColonyId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsComplited = table.Column<bool>(type: "boolean", nullable: false),
                    Parameters = table.Column<string>(type: "text", nullable: false),
                    RunAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StartAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StepNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cycles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cycles_Colonies_ColonyId",
                        column: x => x.ColonyId,
                        principalTable: "Colonies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cycles_ColonyId",
                table: "Cycles",
                column: "ColonyId");

            migrationBuilder.CreateIndex(
                name: "IX_Cycles_RunAtUtc",
                table: "Cycles",
                column: "RunAtUtc");
        }
    }
}
