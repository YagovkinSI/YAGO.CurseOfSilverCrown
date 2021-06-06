using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class DomainRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromDomainId", "ToDomainId" },
                keyValues: new object[] { 35, 32 });

            migrationBuilder.AddColumn<int>(
                name: "EndOfLastGameTurnId",
                table: "Turns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfGame",
                table: "Turns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DomainRelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceDomainId = table.Column<int>(type: "int", nullable: false),
                    TargetDomainId = table.Column<int>(type: "int", nullable: false),
                    IsIncludeVassals = table.Column<bool>(type: "bit", nullable: false),
                    PermissionOfPassage = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DomainRelations_Domains_SourceDomainId",
                        column: x => x.SourceDomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DomainRelations_Domains_TargetDomainId",
                        column: x => x.TargetDomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId" },
                values: new object[] { 35, 33 });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId" },
                values: new object[] { 42, 86 });

            migrationBuilder.CreateIndex(
                name: "IX_DomainRelations_SourceDomainId",
                table: "DomainRelations",
                column: "SourceDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_DomainRelations_SourceDomainId_TargetDomainId",
                table: "DomainRelations",
                columns: new[] { "SourceDomainId", "TargetDomainId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DomainRelations_TargetDomainId",
                table: "DomainRelations",
                column: "TargetDomainId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DomainRelations");

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromDomainId", "ToDomainId" },
                keyValues: new object[] { 35, 33 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromDomainId", "ToDomainId" },
                keyValues: new object[] { 42, 86 });

            migrationBuilder.DropColumn(
                name: "EndOfLastGameTurnId",
                table: "Turns");

            migrationBuilder.DropColumn(
                name: "NumberOfGame",
                table: "Turns");

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId" },
                values: new object[] { 35, 32 });
        }
    }
}
