using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class InitiatorLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Domains_InitiatorId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_InitiatorId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "InitiatorId",
                table: "Units");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Domains_InitiatorDomainId",
                table: "Units",
                column: "InitiatorDomainId",
                principalTable: "Domains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Domains_InitiatorDomainId",
                table: "Units");

            migrationBuilder.AddColumn<int>(
                name: "InitiatorId",
                table: "Units",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_InitiatorId",
                table: "Units",
                column: "InitiatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Domains_InitiatorId",
                table: "Units",
                column: "InitiatorId",
                principalTable: "Domains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
