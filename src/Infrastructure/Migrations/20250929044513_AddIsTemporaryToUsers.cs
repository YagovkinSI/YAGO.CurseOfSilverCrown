using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YAGO.World.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsTemporaryToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTemporary",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTemporary",
                table: "AspNetUsers");
        }
    }
}
