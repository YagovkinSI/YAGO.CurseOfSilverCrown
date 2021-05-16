using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class UnitPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionDomainId",
                table: "Units",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_PositionDomainId",
                table: "Units",
                column: "PositionDomainId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Domains_PositionDomainId",
                table: "Units",
                column: "PositionDomainId",
                principalTable: "Domains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Domains_PositionDomainId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_PositionDomainId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "PositionDomainId",
                table: "Units");
        }
    }
}
