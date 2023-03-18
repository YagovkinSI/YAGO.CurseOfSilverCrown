using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class AddJsonFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EventStoryJson",
                table: "Events",
                newName: "EventJson");

            migrationBuilder.AddColumn<string>(
                name: "UnitJson",
                table: "Units",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TurnJson",
                table: "Turns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RouteJson",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelationJson",
                table: "Relations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EventObjectJson",
                table: "EventObjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorJson",
                table: "Errors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DomainJson",
                table: "Domains",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommandJson",
                table: "Commands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CharacterJson",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserJson",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitJson",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "TurnJson",
                table: "Turns");

            migrationBuilder.DropColumn(
                name: "RouteJson",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "RelationJson",
                table: "Relations");

            migrationBuilder.DropColumn(
                name: "EventObjectJson",
                table: "EventObjects");

            migrationBuilder.DropColumn(
                name: "ErrorJson",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "DomainJson",
                table: "Domains");

            migrationBuilder.DropColumn(
                name: "CommandJson",
                table: "Commands");

            migrationBuilder.DropColumn(
                name: "CharacterJson",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "UserJson",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "EventJson",
                table: "Events",
                newName: "EventStoryJson");
        }
    }
}
