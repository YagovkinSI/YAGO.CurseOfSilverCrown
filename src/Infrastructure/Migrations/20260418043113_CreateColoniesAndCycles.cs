using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YAGO.World.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateColoniesAndCycles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Colonies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Solars = table.Column<double>(type: "double precision", nullable: false),
                    StatesJson = table.Column<string>(type: "text", nullable: false),
                    Deactivated = table.Column<bool>(type: "boolean", nullable: false),
                    DeactivateAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colonies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Colonies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cycles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ColonyId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RunAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StepNumber = table.Column<int>(type: "integer", nullable: false),
                    IsComplited = table.Column<bool>(type: "boolean", nullable: false),
                    Parameters = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false)
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
                name: "IX_Colonies_Name",
                table: "Colonies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colonies_UserId",
                table: "Colonies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cycles_ColonyId",
                table: "Cycles",
                column: "ColonyId");

            migrationBuilder.CreateIndex(
                name: "IX_Cycles_RunAtUtc",
                table: "Cycles",
                column: "RunAtUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cycles");

            migrationBuilder.DropTable(
                name: "Colonies");
        }
    }
}
