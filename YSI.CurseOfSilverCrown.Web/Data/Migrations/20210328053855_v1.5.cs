using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Data.Migrations
{
    public partial class v15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "306e70ee-3887-47af-a30e-c0972b4975d7");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "64fd1831-5f9a-4792-afb9-d60f6c6890c4");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "87ca7484-57a6-40b7-834a-443968660af9");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "a254fd4e-6eb0-4567-b587-29effbea2b98");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "adffbb1f-8098-4d1e-bfe6-b624c8cfe047");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "bfaa36df-fa2e-461c-8126-e62227030302");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "c4a17ef8-8ebc-4df1-a213-8f8c613c9eae");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "ea1dc420-e341-409d-be17-57ad73d1cb59");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "ea2781fa-b0d6-40a2-8f69-8cdb86863f7f");

            migrationBuilder.CreateTable(
                name: "EventStories",
                columns: table => new
                {
                    TurnId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    EventStoryJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStories", x => new { x.TurnId, x.Id });
                    table.ForeignKey(
                        name: "FK_EventStories_Turns_TurnId",
                        column: x => x.TurnId,
                        principalTable: "Turns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationEventStories",
                columns: table => new
                {
                    TurnId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EventStoryId = table.Column<int>(type: "int", nullable: false),
                    Importance = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationEventStories", x => new { x.TurnId, x.OrganizationId, x.EventStoryId });
                    table.ForeignKey(
                        name: "FK_OrganizationEventStories_EventStories_TurnId_EventStoryId",
                        columns: x => new { x.TurnId, x.EventStoryId },
                        principalTable: "EventStories",
                        principalColumns: new[] { "TurnId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationEventStories_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationEventStories_Turns_TurnId",
                        column: x => x.TurnId,
                        principalTable: "Turns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "OrganizationId", "Result", "TargetOrganizationId", "TurnId", "Type" },
                values: new object[,]
                {
                    { "b0fd6bee-2eb3-47b6-b7da-6ebc8f3a55a5", "TinMines", null, null, 1, 0 },
                    { "e3aaad84-11fd-44ee-9b3d-d753dec4f623", "CapeRaptor", null, null, 1, 0 },
                    { "235bb5a6-94b2-42d1-a0ef-3a4c04857c6c", "MouthOfPolaima", null, null, 1, 0 },
                    { "6baa792e-2378-4535-9d82-0c2171ca3ca1", "HeatherOfDimmoria", null, null, 1, 0 },
                    { "6bf9afea-77c3-4696-809c-78d0230ee360", "DimmoriaValley", null, null, 1, 0 },
                    { "bb6debd9-3311-4d59-9f8b-1d686332099a", "SummerCoast", null, null, 1, 0 },
                    { "981d9b86-baff-444b-a7bb-f4f7ae7a6e16", "DimmoriaFarms", null, null, 1, 0 },
                    { "552b1946-2767-4c4c-a2a1-1326e247b944", "ChalRocks", null, null, 1, 0 },
                    { "557bd7ca-5b66-4631-83d5-34d4ff7ce53d", "LimestoneRidges", null, null, 1, 0 }
                });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 3, 28, 5, 38, 54, 132, DateTimeKind.Utc).AddTicks(9736));

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEventStories_OrganizationId",
                table: "OrganizationEventStories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEventStories_TurnId_EventStoryId",
                table: "OrganizationEventStories",
                columns: new[] { "TurnId", "EventStoryId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationEventStories");

            migrationBuilder.DropTable(
                name: "EventStories");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "235bb5a6-94b2-42d1-a0ef-3a4c04857c6c");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "552b1946-2767-4c4c-a2a1-1326e247b944");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "557bd7ca-5b66-4631-83d5-34d4ff7ce53d");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "6baa792e-2378-4535-9d82-0c2171ca3ca1");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "6bf9afea-77c3-4696-809c-78d0230ee360");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "981d9b86-baff-444b-a7bb-f4f7ae7a6e16");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "b0fd6bee-2eb3-47b6-b7da-6ebc8f3a55a5");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "bb6debd9-3311-4d59-9f8b-1d686332099a");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "e3aaad84-11fd-44ee-9b3d-d753dec4f623");

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "OrganizationId", "Result", "TargetOrganizationId", "TurnId", "Type" },
                values: new object[,]
                {
                    { "ea2781fa-b0d6-40a2-8f69-8cdb86863f7f", "TinMines", null, null, 1, 0 },
                    { "adffbb1f-8098-4d1e-bfe6-b624c8cfe047", "CapeRaptor", null, null, 1, 0 },
                    { "a254fd4e-6eb0-4567-b587-29effbea2b98", "MouthOfPolaima", null, null, 1, 0 },
                    { "64fd1831-5f9a-4792-afb9-d60f6c6890c4", "HeatherOfDimmoria", null, null, 1, 0 },
                    { "ea1dc420-e341-409d-be17-57ad73d1cb59", "DimmoriaValley", null, null, 1, 0 },
                    { "87ca7484-57a6-40b7-834a-443968660af9", "SummerCoast", null, null, 1, 0 },
                    { "306e70ee-3887-47af-a30e-c0972b4975d7", "DimmoriaFarms", null, null, 1, 0 },
                    { "c4a17ef8-8ebc-4df1-a213-8f8c613c9eae", "ChalRocks", null, null, 1, 0 },
                    { "bfaa36df-fa2e-461c-8126-e62227030302", "LimestoneRidges", null, null, 1, 0 }
                });

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 3, 28, 4, 50, 24, 914, DateTimeKind.Utc).AddTicks(5116));
        }
    }
}
