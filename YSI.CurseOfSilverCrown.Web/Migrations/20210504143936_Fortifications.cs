using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class Fortifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Fortifications",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Target2OrganizationId",
                table: "Commands",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 5, 4, 14, 39, 35, 729, DateTimeKind.Utc).AddTicks(150));

            migrationBuilder.CreateIndex(
                name: "IX_Commands_Target2OrganizationId",
                table: "Commands",
                column: "Target2OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commands_Organizations_Target2OrganizationId",
                table: "Commands",
                column: "Target2OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commands_Organizations_Target2OrganizationId",
                table: "Commands");

            migrationBuilder.DropIndex(
                name: "IX_Commands_Target2OrganizationId",
                table: "Commands");

            migrationBuilder.DropColumn(
                name: "Fortifications",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Target2OrganizationId",
                table: "Commands");

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 5, 3, 7, 24, 53, 121, DateTimeKind.Utc).AddTicks(6495));
        }
    }
}
