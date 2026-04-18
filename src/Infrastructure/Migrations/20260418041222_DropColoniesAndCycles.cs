using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YAGO.World.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UseGuidIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"Cycles\" CASCADE;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"Colonies\" CASCADE;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            throw new Exception("Cannot rollback migration that drops tables");
        }
    }
}
