using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YAGO.World.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStartAtUtcToCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsComplited",
                table: "Cycles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Parameters",
                table: "Cycles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartAtUtc",
                table: "Cycles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComplited",
                table: "Cycles");

            migrationBuilder.DropColumn(
                name: "Parameters",
                table: "Cycles");

            migrationBuilder.DropColumn(
                name: "StartAtUtc",
                table: "Cycles");
        }
    }
}
