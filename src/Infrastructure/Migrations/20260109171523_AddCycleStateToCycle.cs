using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YAGO.World.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCycleStateToCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RunAtUtc",
                table: "Cycles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Cycles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StepNumber",
                table: "Cycles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RunAtUtc",
                table: "Cycles");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Cycles");

            migrationBuilder.DropColumn(
                name: "StepNumber",
                table: "Cycles");
        }
    }
}
