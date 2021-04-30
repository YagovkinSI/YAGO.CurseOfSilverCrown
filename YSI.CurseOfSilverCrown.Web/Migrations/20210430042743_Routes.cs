using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class Routes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    FromProvinceId = table.Column<int>(type: "int", nullable: false),
                    ToProvinceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => new { x.FromProvinceId, x.ToProvinceId });
                    table.ForeignKey(
                        name: "FK_Routes_Provinces_FromProvinceId",
                        column: x => x.FromProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Routes_Provinces_ToProvinceId",
                        column: x => x.ToProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromProvinceId", "ToProvinceId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 9, 7 },
                    { 8, 9 },
                    { 8, 7 },
                    { 8, 5 },
                    { 7, 9 },
                    { 7, 8 },
                    { 7, 6 },
                    { 7, 5 },
                    { 6, 7 },
                    { 6, 5 },
                    { 6, 4 },
                    { 5, 8 },
                    { 9, 8 },
                    { 5, 7 },
                    { 5, 4 },
                    { 4, 6 },
                    { 4, 5 },
                    { 4, 3 },
                    { 4, 2 },
                    { 3, 4 },
                    { 3, 2 },
                    { 3, 1 },
                    { 2, 4 },
                    { 2, 3 },
                    { 2, 1 },
                    { 1, 3 },
                    { 5, 6 }
                });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 4, 30, 4, 27, 42, 268, DateTimeKind.Utc).AddTicks(1403));

            migrationBuilder.CreateIndex(
                name: "IX_Routes_FromProvinceId",
                table: "Routes",
                column: "FromProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ToProvinceId",
                table: "Routes",
                column: "ToProvinceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 4, 18, 3, 40, 46, 417, DateTimeKind.Utc).AddTicks(2170));
        }
    }
}
