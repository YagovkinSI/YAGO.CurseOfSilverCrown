using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Data.Migrations
{
    public partial class _12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Power",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "CapeRaptor",
                columns: new[] { "Name", "Power" },
                values: new object[] { "Мыс ящера", 200000 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "ChalRocks",
                columns: new[] { "Name", "Power" },
                values: new object[] { "Меловые скалы", 200000 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaFarms",
                columns: new[] { "Name", "Power" },
                values: new object[] { "Фермы Диммории", 200000 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaValley",
                columns: new[] { "Name", "Power" },
                values: new object[] { "Долина Диммории", 200000 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "HeatherOfDimmoria",
                columns: new[] { "Name", "Power" },
                values: new object[] { "Верещатник Диммории", 200000 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LimestoneRidges",
                columns: new[] { "Name", "Power" },
                values: new object[] { "Известняковые хребты", 200000 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "MouthOfPolaima",
                columns: new[] { "Name", "Power" },
                values: new object[] { "Устье Полаймы", 200000 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SummerCoast",
                columns: new[] { "Name", "Power" },
                values: new object[] { "Летний берег", 200000 });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TinMines",
                columns: new[] { "Name", "Power" },
                values: new object[] { "Оловянные шахты", 200000 });

            migrationBuilder.InsertData(
                table: "Turns",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "587 год - Зима" });

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "OrganizationId", "Result", "TargetOrganizationId", "TurnId", "Type" },
                values: new object[,]
                {
                    { "32b08ce3-0371-4419-90ad-b45dcef200bc", "TinMines", null, null, 1, 0 },
                    { "8578ac0f-27fe-4717-9397-3a577e124764", "CapeRaptor", null, null, 1, 0 },
                    { "921edeb1-150a-447a-aa0d-446246a47a23", "MouthOfPolaima", null, null, 1, 0 },
                    { "2dbffbd6-771c-4c13-a727-4286d8e8e363", "HeatherOfDimmoria", null, null, 1, 0 },
                    { "f140d3b8-1deb-43bc-b856-63c6a68785e8", "DimmoriaValley", null, null, 1, 0 },
                    { "044aebbd-dd4f-4177-b2b1-b4e3d28b6a87", "SummerCoast", null, null, 1, 0 },
                    { "4f46faca-211e-4124-8934-591e758e53dd", "DimmoriaFarms", null, null, 1, 0 },
                    { "0d0c20b5-e62b-46d4-9833-084945666d74", "ChalRocks", null, null, 1, 0 },
                    { "e8d288fb-ee9b-4c04-8467-68d2f87f5dbd", "LimestoneRidges", null, null, 1, 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "044aebbd-dd4f-4177-b2b1-b4e3d28b6a87");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "0d0c20b5-e62b-46d4-9833-084945666d74");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "2dbffbd6-771c-4c13-a727-4286d8e8e363");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "32b08ce3-0371-4419-90ad-b45dcef200bc");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "4f46faca-211e-4124-8934-591e758e53dd");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "8578ac0f-27fe-4717-9397-3a577e124764");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "921edeb1-150a-447a-aa0d-446246a47a23");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "e8d288fb-ee9b-4c04-8467-68d2f87f5dbd");

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: "f140d3b8-1deb-43bc-b856-63c6a68785e8");

            migrationBuilder.DeleteData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Power",
                table: "Organizations");
        }
    }
}
