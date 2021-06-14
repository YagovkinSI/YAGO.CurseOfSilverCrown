using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class IceCreekNameFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 51,
                column: "Name",
                value: "Ледяной ручей");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 51,
                column: "Name",
                value: "Ледяноый ручей");
        }
    }
}
