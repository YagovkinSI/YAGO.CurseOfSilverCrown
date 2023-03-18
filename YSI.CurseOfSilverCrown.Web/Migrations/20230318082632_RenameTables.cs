using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class RenameTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Persons_PersonId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Commands_Persons_PersonInitiatorId",
                table: "Commands");

            migrationBuilder.DropForeignKey(
                name: "FK_DomainRelations_Domains_SourceDomainId",
                table: "DomainRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_DomainRelations_Domains_TargetDomainId",
                table: "DomainRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_Domains_Persons_PersonId",
                table: "Domains");

            migrationBuilder.DropForeignKey(
                name: "FK_EventStories_Turns_TurnId",
                table: "EventStories");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Persons_InitiatorPersonId",
                table: "Units");

            migrationBuilder.DropTable(
                name: "OrganizationEventStories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Persons",
                table: "Persons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventStories",
                table: "EventStories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DomainRelations",
                table: "DomainRelations");

            migrationBuilder.RenameTable(
                name: "Persons",
                newName: "Characters");

            migrationBuilder.RenameTable(
                name: "EventStories",
                newName: "Events");

            migrationBuilder.RenameTable(
                name: "DomainRelations",
                newName: "Relations");

            migrationBuilder.RenameIndex(
                name: "IX_DomainRelations_TargetDomainId",
                table: "Relations",
                newName: "IX_Relations_TargetDomainId");

            migrationBuilder.RenameIndex(
                name: "IX_DomainRelations_SourceDomainId_TargetDomainId",
                table: "Relations",
                newName: "IX_Relations_SourceDomainId_TargetDomainId");

            migrationBuilder.RenameIndex(
                name: "IX_DomainRelations_SourceDomainId",
                table: "Relations",
                newName: "IX_Relations_SourceDomainId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Characters",
                table: "Characters",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                columns: new[] { "TurnId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Relations",
                table: "Relations",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EventObjects",
                columns: table => new
                {
                    TurnId = table.Column<int>(type: "int", nullable: false),
                    DomainId = table.Column<int>(type: "int", nullable: false),
                    EventStoryId = table.Column<int>(type: "int", nullable: false),
                    Importance = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventObjects", x => new { x.TurnId, x.DomainId, x.EventStoryId });
                    table.ForeignKey(
                        name: "FK_EventObjects_Domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventObjects_Events_TurnId_EventStoryId",
                        columns: x => new { x.TurnId, x.EventStoryId },
                        principalTable: "Events",
                        principalColumns: new[] { "TurnId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventObjects_Turns_TurnId",
                        column: x => x.TurnId,
                        principalTable: "Turns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventObjects_DomainId",
                table: "EventObjects",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_EventObjects_TurnId_EventStoryId",
                table: "EventObjects",
                columns: new[] { "TurnId", "EventStoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Characters_PersonId",
                table: "AspNetUsers",
                column: "PersonId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Commands_Characters_PersonInitiatorId",
                table: "Commands",
                column: "PersonInitiatorId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Domains_Characters_PersonId",
                table: "Domains",
                column: "PersonId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Turns_TurnId",
                table: "Events",
                column: "TurnId",
                principalTable: "Turns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Relations_Domains_SourceDomainId",
                table: "Relations",
                column: "SourceDomainId",
                principalTable: "Domains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Relations_Domains_TargetDomainId",
                table: "Relations",
                column: "TargetDomainId",
                principalTable: "Domains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Characters_InitiatorPersonId",
                table: "Units",
                column: "InitiatorPersonId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Characters_PersonId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Commands_Characters_PersonInitiatorId",
                table: "Commands");

            migrationBuilder.DropForeignKey(
                name: "FK_Domains_Characters_PersonId",
                table: "Domains");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Turns_TurnId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Relations_Domains_SourceDomainId",
                table: "Relations");

            migrationBuilder.DropForeignKey(
                name: "FK_Relations_Domains_TargetDomainId",
                table: "Relations");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Characters_InitiatorPersonId",
                table: "Units");

            migrationBuilder.DropTable(
                name: "EventObjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Relations",
                table: "Relations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Characters",
                table: "Characters");

            migrationBuilder.RenameTable(
                name: "Relations",
                newName: "DomainRelations");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "EventStories");

            migrationBuilder.RenameTable(
                name: "Characters",
                newName: "Persons");

            migrationBuilder.RenameIndex(
                name: "IX_Relations_TargetDomainId",
                table: "DomainRelations",
                newName: "IX_DomainRelations_TargetDomainId");

            migrationBuilder.RenameIndex(
                name: "IX_Relations_SourceDomainId_TargetDomainId",
                table: "DomainRelations",
                newName: "IX_DomainRelations_SourceDomainId_TargetDomainId");

            migrationBuilder.RenameIndex(
                name: "IX_Relations_SourceDomainId",
                table: "DomainRelations",
                newName: "IX_DomainRelations_SourceDomainId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DomainRelations",
                table: "DomainRelations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventStories",
                table: "EventStories",
                columns: new[] { "TurnId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Persons",
                table: "Persons",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "OrganizationEventStories",
                columns: table => new
                {
                    TurnId = table.Column<int>(type: "int", nullable: false),
                    DomainId = table.Column<int>(type: "int", nullable: false),
                    EventStoryId = table.Column<int>(type: "int", nullable: false),
                    Importance = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationEventStories", x => new { x.TurnId, x.DomainId, x.EventStoryId });
                    table.ForeignKey(
                        name: "FK_OrganizationEventStories_Domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationEventStories_EventStories_TurnId_EventStoryId",
                        columns: x => new { x.TurnId, x.EventStoryId },
                        principalTable: "EventStories",
                        principalColumns: new[] { "TurnId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationEventStories_Turns_TurnId",
                        column: x => x.TurnId,
                        principalTable: "Turns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEventStories_DomainId",
                table: "OrganizationEventStories",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEventStories_TurnId_EventStoryId",
                table: "OrganizationEventStories",
                columns: new[] { "TurnId", "EventStoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Persons_PersonId",
                table: "AspNetUsers",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Commands_Persons_PersonInitiatorId",
                table: "Commands",
                column: "PersonInitiatorId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainRelations_Domains_SourceDomainId",
                table: "DomainRelations",
                column: "SourceDomainId",
                principalTable: "Domains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DomainRelations_Domains_TargetDomainId",
                table: "DomainRelations",
                column: "TargetDomainId",
                principalTable: "Domains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Domains_Persons_PersonId",
                table: "Domains",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventStories_Turns_TurnId",
                table: "EventStories",
                column: "TurnId",
                principalTable: "Turns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Persons_InitiatorPersonId",
                table: "Units",
                column: "InitiatorPersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
