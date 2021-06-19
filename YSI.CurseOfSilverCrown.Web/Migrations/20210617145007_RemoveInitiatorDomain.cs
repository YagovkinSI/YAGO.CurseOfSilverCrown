using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class RemoveInitiatorDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commands_Domains_InitiatorId",
                table: "Commands");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Domains_InitiatorDomainId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_InitiatorDomainId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Commands_InitiatorDomainId",
                table: "Commands");

            migrationBuilder.DropIndex(
                name: "IX_Commands_InitiatorId",
                table: "Commands");

            migrationBuilder.DropColumn(
                name: "InitiatorDomainId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "InitiatorDomainId",
                table: "Commands");

            migrationBuilder.DropColumn(
                name: "InitiatorId",
                table: "Commands");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InitiatorDomainId",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InitiatorDomainId",
                table: "Commands",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InitiatorId",
                table: "Commands",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_InitiatorDomainId",
                table: "Units",
                column: "InitiatorDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Commands_InitiatorDomainId",
                table: "Commands",
                column: "InitiatorDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Commands_InitiatorId",
                table: "Commands",
                column: "InitiatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commands_Domains_InitiatorId",
                table: "Commands",
                column: "InitiatorId",
                principalTable: "Domains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Domains_InitiatorDomainId",
                table: "Units",
                column: "InitiatorDomainId",
                principalTable: "Domains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
