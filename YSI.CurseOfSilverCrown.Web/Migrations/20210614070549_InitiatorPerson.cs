using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class InitiatorPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InitiatorPersonId",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "InitiatorPersonId",
                table: "Commands",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "PersonInitiatorId",
                table: "Commands",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_InitiatorPersonId",
                table: "Units",
                column: "InitiatorPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Commands_InitiatorPersonId",
                table: "Commands",
                column: "InitiatorPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Commands_PersonInitiatorId",
                table: "Commands",
                column: "PersonInitiatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commands_Persons_PersonInitiatorId",
                table: "Commands",
                column: "PersonInitiatorId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Persons_InitiatorPersonId",
                table: "Units",
                column: "InitiatorPersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commands_Persons_PersonInitiatorId",
                table: "Commands");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Persons_InitiatorPersonId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_InitiatorPersonId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Commands_InitiatorPersonId",
                table: "Commands");

            migrationBuilder.DropIndex(
                name: "IX_Commands_PersonInitiatorId",
                table: "Commands");

            migrationBuilder.DropColumn(
                name: "InitiatorPersonId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "InitiatorPersonId",
                table: "Commands");

            migrationBuilder.DropColumn(
                name: "PersonInitiatorId",
                table: "Commands");
        }
    }
}
