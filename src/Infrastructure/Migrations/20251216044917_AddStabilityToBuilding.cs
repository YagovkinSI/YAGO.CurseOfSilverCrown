using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable


namespace YAGO.World.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStabilityToBuilding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Reputation",
                table: "Buildings",
                newName: "Stability");

            migrationBuilder.UpdateData(
                table: "Buildings",
                keyColumn: "Id",
                keyValues: new object[]
                {
                    1L,
                    2L,
                    3L
                },
                columns: new[] { "Stability" },
                values: new object[,]
                {
                    { 1m },
                    { 0m },
                    { -1m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stability",
                table: "Buildings",
                newName: "Reputation");
        }
    }
}
