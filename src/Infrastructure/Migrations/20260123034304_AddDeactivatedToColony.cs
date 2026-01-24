using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YAGO.World.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeactivatedToColony : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Buildings",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Buildings",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Buildings",
                keyColumn: "Id",
                keyValue: 3L);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeactivateAtUtc",
                table: "Colonies");

            migrationBuilder.DropColumn(
                name: "Deactivated",
                table: "Colonies");

            migrationBuilder.InsertData(
                table: "Buildings",
                columns: new[] { "Id", "Challenges", "Cost", "Description", "Name", "Population", "SolarsIncome", "ZonesOccupied" },
                values: new object[,]
                {
                    { 1L, 4, 2000, new[] { "Небольшие, но обустроенные квартиры-студии для рабочих семей. Есть место для личных вещей и отдыха после смены. Такие условия помогают сохранить здоровье и лояльность колонистов." }, "Семейный модуль", 80, 40, 10 },
                    { 2L, 5, 2000, new[] { "Функциональные жилые капсулы с койко-местом, умывальником и небольшим складом для личных вещей. Всё необходимое для восстановления сил перед следующей рабочей сменой." }, "Стандартный модуль", 100, 45, 10 },
                    { 3L, 6, 2000, new[] { "Спальные ниши, общие душевые и столовая. Личное пространство сведено к минимуму. Подходит для временных рабочих или тех, кому нечего терять." }, "Казарменный модуль", 120, 50, 10 }
                });
        }
    }
}
