using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace YAGO.World.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DropColonoesAndTurns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"Turns\" CASCADE;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"Colonies\" CASCADE;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            throw new Exception("Cannot rollback migration that drops tables");
        }
    }
}
