using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class GoT_Suzerains : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromDomainId", "ToDomainId" },
                keyValues: new object[] { 77, 74 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 1,
                column: "SuzerainId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 3,
                column: "SuzerainId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 4,
                column: "SuzerainId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 5,
                column: "SuzerainId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 6,
                column: "SuzerainId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 7,
                column: "SuzerainId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 8,
                column: "SuzerainId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 9,
                column: "SuzerainId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 10,
                column: "SuzerainId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 11,
                column: "SuzerainId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 12,
                column: "SuzerainId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 13,
                column: "SuzerainId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 15,
                column: "SuzerainId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 16,
                column: "SuzerainId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 17,
                column: "SuzerainId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 18,
                column: "SuzerainId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 19,
                column: "SuzerainId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 20,
                column: "SuzerainId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Name", "SuzerainId" },
                values: new object[] { "Торрхенов удел", 14 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 22,
                column: "SuzerainId",
                value: 23);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 23,
                column: "SuzerainId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 24,
                column: "SuzerainId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 25,
                column: "SuzerainId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 26,
                column: "SuzerainId",
                value: 25);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 27,
                column: "SuzerainId",
                value: 25);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 28,
                column: "SuzerainId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 29,
                column: "SuzerainId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 30,
                column: "SuzerainId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 31,
                column: "SuzerainId",
                value: 43);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 32,
                column: "SuzerainId",
                value: 43);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 33,
                column: "SuzerainId",
                value: 43);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 34,
                column: "SuzerainId",
                value: 43);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 35,
                column: "SuzerainId",
                value: 43);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 36,
                column: "SuzerainId",
                value: 43);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 37,
                column: "SuzerainId",
                value: 43);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 38,
                column: "SuzerainId",
                value: 43);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 39,
                column: "SuzerainId",
                value: 43);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 40,
                column: "SuzerainId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 41,
                column: "SuzerainId",
                value: 43);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 42,
                column: "SuzerainId",
                value: 43);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 44,
                column: "SuzerainId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 45,
                column: "SuzerainId",
                value: 46);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 46,
                column: "SuzerainId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 47,
                column: "SuzerainId",
                value: 46);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 48,
                column: "SuzerainId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 49,
                column: "SuzerainId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 50,
                column: "SuzerainId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 51,
                column: "SuzerainId",
                value: 62);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 52,
                column: "SuzerainId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 53,
                column: "SuzerainId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 54,
                column: "SuzerainId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 55,
                column: "SuzerainId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 56,
                column: "SuzerainId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 57,
                column: "SuzerainId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 58,
                column: "SuzerainId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 60,
                column: "SuzerainId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 61,
                column: "SuzerainId",
                value: 62);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 62,
                column: "SuzerainId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 63,
                column: "SuzerainId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 64,
                column: "SuzerainId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 65,
                column: "SuzerainId",
                value: 59);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 66,
                column: "SuzerainId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 67,
                column: "SuzerainId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 68,
                column: "SuzerainId",
                value: 66);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 69,
                column: "SuzerainId",
                value: 66);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 70,
                column: "SuzerainId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 72,
                column: "SuzerainId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 73,
                column: "SuzerainId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 74,
                column: "SuzerainId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 75,
                column: "SuzerainId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 76,
                column: "SuzerainId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 77,
                column: "SuzerainId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 78,
                column: "SuzerainId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 79,
                column: "SuzerainId",
                value: 82);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 80,
                column: "SuzerainId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 81,
                column: "SuzerainId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 82,
                column: "SuzerainId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 83,
                column: "SuzerainId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 84,
                column: "SuzerainId",
                value: 71);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 85,
                column: "SuzerainId",
                value: 100);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 22550, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 21050, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 22850, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 89,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 22350, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 22750, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 21550, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 21850, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 23050, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 21350, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 22250, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 96,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 23550, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 97,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 22650, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 98,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 22150, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 21650, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 23350, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 23950, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 22950, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 107,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 22450, 100 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 108,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 22055, 100 });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId" },
                values: new object[,]
                {
                    { 77, 75 },
                    { 4, 5 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromDomainId", "ToDomainId" },
                keyValues: new object[] { 4, 5 });

            migrationBuilder.DeleteData(
                table: "Routes",
                keyColumns: new[] { "FromDomainId", "ToDomainId" },
                keyValues: new object[] { 77, 75 });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 1,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 3,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 4,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 5,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 6,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 7,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 8,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 9,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 10,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 11,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 12,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 13,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 15,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 16,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 17,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 18,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 19,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 20,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Name", "SuzerainId" },
                values: new object[] { "Торхенов удел", null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 22,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 23,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 24,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 25,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 26,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 27,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 28,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 29,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 30,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 31,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 32,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 33,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 34,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 35,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 36,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 37,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 38,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 39,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 40,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 41,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 42,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 44,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 45,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 46,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 47,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 48,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 49,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 50,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 51,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 52,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 53,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 54,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 55,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 56,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 57,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 58,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 60,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 61,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 62,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 63,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 64,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 65,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 66,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 67,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 68,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 69,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 70,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 72,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 73,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 74,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 75,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 76,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 77,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 78,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 79,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 80,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 81,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 82,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 83,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 84,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 85,
                column: "SuzerainId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 2550, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 1050, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 2850, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 89,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 2350, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 2750, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 1550, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 1850, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 3050, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 1350, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 2250, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 96,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 3550, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 97,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 2650, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 98,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 2150, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 1650, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 3350, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 3950, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 2950, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 107,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 2450, null });

            migrationBuilder.UpdateData(
                table: "Domains",
                keyColumn: "Id",
                keyValue: 108,
                columns: new[] { "MoveOrder", "SuzerainId" },
                values: new object[] { 2055, null });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId" },
                values: new object[] { 77, 74 });
        }
    }
}
