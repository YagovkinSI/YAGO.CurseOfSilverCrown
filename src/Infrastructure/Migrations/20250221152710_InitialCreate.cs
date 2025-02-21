using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace YAGO.World.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    LastActivityTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserJson = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Errors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RequestId = table.Column<string>(type: "text", nullable: true),
                    TypeFullName = table.Column<string>(type: "text", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true),
                    StackTrace = table.Column<string>(type: "text", nullable: true),
                    ErrorJson = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Turns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Started = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    TurnJson = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Domains",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Gold = table.Column<int>(type: "integer", nullable: false),
                    Investments = table.Column<int>(type: "integer", nullable: false),
                    Fortifications = table.Column<int>(type: "integer", nullable: false),
                    Size = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    SuzerainId = table.Column<int>(type: "integer", nullable: true),
                    TurnOfDefeat = table.Column<int>(type: "integer", nullable: false),
                    DomainJson = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Domains_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Domains_Domains_SuzerainId",
                        column: x => x.SuzerainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    TurnId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    EventJson = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => new { x.TurnId, x.Id });
                    table.ForeignKey(
                        name: "FK_Events_Turns_TurnId",
                        column: x => x.TurnId,
                        principalTable: "Turns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Commands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExecutorType = table.Column<int>(type: "integer", nullable: false),
                    ExecutorId = table.Column<int>(type: "integer", nullable: false),
                    DomainId = table.Column<int>(type: "integer", nullable: false),
                    Gold = table.Column<int>(type: "integer", nullable: false),
                    Warriors = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    TargetDomainId = table.Column<int>(type: "integer", nullable: true),
                    Target2DomainId = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CommandJson = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commands_Domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commands_Domains_Target2DomainId",
                        column: x => x.Target2DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commands_Domains_TargetDomainId",
                        column: x => x.TargetDomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Relations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SourceDomainId = table.Column<int>(type: "integer", nullable: false),
                    TargetDomainId = table.Column<int>(type: "integer", nullable: false),
                    IsIncludeVassals = table.Column<bool>(type: "boolean", nullable: false),
                    Defense = table.Column<bool>(type: "boolean", nullable: false),
                    RelationJson = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relations_Domains_SourceDomainId",
                        column: x => x.SourceDomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Relations_Domains_TargetDomainId",
                        column: x => x.TargetDomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    FromDomainId = table.Column<int>(type: "integer", nullable: false),
                    ToDomainId = table.Column<int>(type: "integer", nullable: false),
                    RouteJson = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => new { x.FromDomainId, x.ToDomainId });
                    table.ForeignKey(
                        name: "FK_Routes_Domains_FromDomainId",
                        column: x => x.FromDomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Routes_Domains_ToDomainId",
                        column: x => x.ToDomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DomainId = table.Column<int>(type: "integer", nullable: false),
                    Gold = table.Column<int>(type: "integer", nullable: false),
                    Warriors = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    TargetDomainId = table.Column<int>(type: "integer", nullable: true),
                    Target2DomainId = table.Column<int>(type: "integer", nullable: true),
                    PositionDomainId = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    UnitJson = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Units_Domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Units_Domains_PositionDomainId",
                        column: x => x.PositionDomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Units_Domains_Target2DomainId",
                        column: x => x.Target2DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Units_Domains_TargetDomainId",
                        column: x => x.TargetDomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventObjects",
                columns: table => new
                {
                    TurnId = table.Column<int>(type: "integer", nullable: false),
                    DomainId = table.Column<int>(type: "integer", nullable: false),
                    EventStoryId = table.Column<int>(type: "integer", nullable: false),
                    Importance = table.Column<int>(type: "integer", nullable: false),
                    EventObjectJson = table.Column<string>(type: "text", nullable: true)
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

            migrationBuilder.InsertData(
                table: "Domains",
                columns: new[] { "Id", "DomainJson", "Fortifications", "Gold", "Investments", "Name", "Size", "SuzerainId", "TurnOfDefeat", "UserId" },
                values: new object[,]
                {
                    { 2, null, 81620, 12240, 45000, "Чёрный замок", 1510, null, -2147483648, null },
                    { 14, null, 75460, 32400, 318000, "Винтерфелл", 5310, null, -2147483648, null },
                    { 43, null, 77000, 28200, 318000, "Пайк", 1320, null, -2147483648, null },
                    { 59, null, 131760, 37800, 318000, "Орлиное гнездо", 3830, null, -2147483648, null },
                    { 71, null, 117120, 33840, 294000, "Риверран", 3540, null, -2147483648, null },
                    { 100, null, 69300, 43200, 135000, "Утёс Кастерли", 3850, null, -2147483648, null }
                });

            migrationBuilder.InsertData(
                table: "Turns",
                columns: new[] { "Id", "IsActive", "Started", "TurnJson" },
                values: new object[] { 1, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.InsertData(
                table: "Domains",
                columns: new[] { "Id", "DomainJson", "Fortifications", "Gold", "Investments", "Name", "Size", "SuzerainId", "TurnOfDefeat", "UserId" },
                values: new object[,]
                {
                    { 1, null, 40320, 5640, 49000, "Сумеречная башня", 1010, 2, -2147483648, null },
                    { 82, null, 18020, 6120, 135000, "Приют странника", 1740, 71, -2147483648, null },
                    { 81, null, 16320, 5640, 147000, "Розовая дева", 2340, 71, -2147483648, null },
                    { 80, null, 15300, 5400, 135000, "Атранта", 1340, 71, -2147483648, null },
                    { 78, null, 37800, 5640, 159000, "Харренхол", 3340, 71, -2147483648, null },
                    { 77, null, 16320, 6120, 405000, "Девичий пруд", 1540, 71, -2147483648, null },
                    { 76, null, 18020, 5640, 294000, "Солеварни", 1040, 71, -2147483648, null },
                    { 75, null, 17000, 5400, 135000, "Дарри", 3840, 71, -2147483648, null },
                    { 74, null, 16660, 5400, 318000, "Город Харровея", 4540, 71, -2147483648, null },
                    { 73, null, 17000, 5640, 159000, "Замок Личестеров", 2840, 71, -2147483648, null },
                    { 72, null, 18020, 6120, 135000, "Каменный оплот", 2540, 71, -2147483648, null },
                    { 70, null, 15300, 5400, 135000, "Вранодрев", 4040, 71, -2147483648, null },
                    { 67, null, 16320, 6120, 135000, "Сигард", 3040, 71, -2147483648, null },
                    { 66, null, 81620, 11280, 294000, "Близнецы", 5040, 71, -2147483648, null },
                    { 65, null, 17000, 5400, 135000, "Фитили", 3330, 59, -2147483648, null },
                    { 64, null, 16660, 5400, 795000, "Чаячий город", 2130, 59, -2147483648, null },
                    { 63, null, 42000, 5640, 159000, "Ведьмин остров", 330, 59, -2147483648, null },
                    { 62, null, 18020, 12240, 270000, "Рунный камень", 1430, 59, -2147483648, null },
                    { 83, null, 17000, 5640, 159000, "Каслвуд", 2640, 71, -2147483648, null },
                    { 60, null, 15300, 5400, 135000, "Редфорт", 1630, 59, -2147483648, null },
                    { 84, null, 16660, 5400, 159000, "Каменная септа", 3740, 71, -2147483648, null },
                    { 86, null, 18020, 5640, 147000, "Гибельная крепость", 2550, 100, -2147483648, null },
                    { 107, null, 16320, 6120, 135000, "Гринфилд", 2450, 100, -2147483648, null },
                    { 106, null, 18020, 5640, 147000, "Корнфилд", 2950, 100, -2147483648, null },
                    { 105, null, 17000, 5400, 135000, "Крейкхолл", 3950, 100, -2147483648, null },
                    { 101, null, 16320, 5640, 882000, "Ланниспорт", 3350, 100, -2147483648, null },
                    { 99, null, 18360, 5400, 159000, "Замок Клиганов", 1650, 100, -2147483648, null },
                    { 98, null, 15300, 5640, 159000, "Серебрянный холм", 2150, 100, -2147483648, null },
                    { 97, null, 16320, 6120, 135000, "Ключи", 2650, 100, -2147483648, null },
                    { 96, null, 18020, 5640, 147000, "Глубокая нора", 3550, 100, -2147483648, null },
                    { 95, null, 17000, 5400, 135000, "Хорнваль", 2250, 100, -2147483648, null },
                    { 94, null, 16660, 5400, 159000, "Сарсфилд", 1350, 100, -2147483648, null },
                    { 93, null, 17000, 5640, 159000, "Пиршественные огни", 3050, 100, -2147483648, null },
                    { 92, null, 44520, 6120, 135000, "Светлый остров", 1850, 100, -2147483648, null },
                    { 91, null, 1920, 5640, 147000, "Тарбекхолл", 1550, 100, -2147483648, null },
                    { 90, null, 15300, 5400, 135000, "Эшмарк", 2750, 100, -2147483648, null },
                    { 89, null, 18360, 5400, 159000, "Золотой зуб", 2350, 100, -2147483648, null },
                    { 88, null, 1800, 5640, 159000, "Кастамере", 2850, 100, -2147483648, null },
                    { 87, null, 16320, 6120, 135000, "Скала", 1050, 100, -2147483648, null },
                    { 85, null, 17000, 5400, 135000, "Виндхолл", 2050, 100, -2147483648, null },
                    { 108, null, 15300, 5640, 159000, "Золотая дорога", 2055, 100, -2147483648, null },
                    { 58, null, 15300, 5640, 159000, "Железная дубрава", 1930, 59, -2147483648, null },
                    { 56, null, 18020, 5640, 147000, "Старый якорь", 1830, 59, -2147483648, null },
                    { 30, null, 15300, 5400, 135000, "Кремневый палец", 5110, 14, -2147483648, null },
                    { 29, null, 18360, 5400, 53000, "Перешеек", 6010, 14, -2147483648, null },
                    { 28, null, 69300, 5640, 53000, "Ров Кайлин", 4610, 14, -2147483648, null },
                    { 25, null, 17000, 10800, 540000, "Белая гавань", 5610, 14, -2147483648, null },
                    { 24, null, 16660, 5400, 477000, "Барроутон", 5710, 14, -2147483648, null },
                    { 23, null, 17000, 5640, 159000, "Родники", 5410, 14, -2147483648, null },
                    { 21, null, 16320, 5640, 147000, "Торрхенов удел", 5910, 14, -2147483648, null },
                    { 19, null, 18360, 5400, 159000, "Замок Сервинов", 2810, 14, -2147483648, null },
                    { 15, null, 77000, 16200, 270000, "Дредфорт", 5210, 14, -2147483648, null },
                    { 12, null, 18020, 12240, 135000, "Темнолесье", 3710, 14, -2147483648, null },
                    { 10, null, 37800, 5400, 135000, "Медвежий остров", 1810, 14, -2147483648, null },
                    { 9, null, 18360, 5400, 159000, "Каменный холм", 3310, 14, -2147483648, null },
                    { 7, null, 16320, 6120, 135000, "Последний очаг", 5010, 14, -2147483648, null },
                    { 6, null, 18020, 5640, 147000, "Кархолд", 5510, 14, -2147483648, null },
                    { 5, null, 17000, 5400, 135000, "Скагос", 4010, 14, -2147483648, null },
                    { 4, null, 16660, 5400, 159000, "Новый дар", 3010, 2, -2147483648, null },
                    { 3, null, 42000, 5640, 53000, "Восточный дозор", 1310, 2, -2147483648, null },
                    { 57, null, 16320, 6120, 135000, "Девять звёзд", 1730, 59, -2147483648, null },
                    { 32, null, 44520, 6120, 135000, "Старый Вик", 1020, 43, -2147483648, null },
                    { 31, null, 40320, 5640, 147000, "Чёрная волна", 1520, 43, -2147483648, null },
                    { 34, null, 41160, 5400, 53000, "Одинокий светоч", 1820, 43, -2147483648, null },
                    { 55, null, 17000, 5400, 135000, "Длинный лук", 2830, 59, -2147483648, null },
                    { 54, null, 16660, 5400, 159000, "Дом сердец", 3530, 59, -2147483648, null },
                    { 53, null, 17000, 5640, 159000, "Змеиный лес", 2330, 59, -2147483648, null },
                    { 52, null, 18020, 6120, 135000, "Суровая песнь", 2530, 59, -2147483648, null },
                    { 50, null, 15300, 5400, 135000, "Персты", 3030, 59, -2147483648, null },
                    { 49, null, 45360, 5400, 159000, "Сосцы", 630, 59, -2147483648, null },
                    { 48, null, 37800, 5640, 159000, "Галечный остров", 530, 59, -2147483648, null },
                    { 33, null, 42000, 5640, 159000, "Хаммерхорн", 3020, 43, -2147483648, null },
                    { 46, null, 44520, 11280, 147000, "Милая сестра", 830, 59, -2147483648, null },
                    { 41, null, 40320, 5640, 147000, "Железная роща", 820, 43, -2147483648, null },
                    { 39, null, 45360, 10800, 318000, "Десять башен", 3520, 43, -2147483648, null },
                    { 38, null, 37800, 5640, 159000, "Оркмонт", 2020, 43, -2147483648, null },
                    { 37, null, 40320, 6120, 135000, "Пебблтон", 1620, 43, -2147483648, null },
                    { 36, null, 44520, 5640, 147000, "Гольцы", 1420, 43, -2147483648, null },
                    { 35, null, 42000, 5400, 135000, "Солёный утёс", 1720, 43, -2147483648, null },
                    { 42, null, 44520, 6120, 270000, "Лордпорт", 320, 43, -2147483648, null }
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "DomainId", "Gold", "PositionDomainId", "Status", "Target2DomainId", "TargetDomainId", "Type", "UnitJson", "Warriors" },
                values: new object[,]
                {
                    { 43, 43, 0, 43, 100, null, 43, 5, null, 10778 },
                    { 59, 59, 0, 59, 100, null, 59, 5, null, 12079 },
                    { 2, 2, 0, 2, 100, null, 2, 5, null, 2038 },
                    { 14, 14, 0, 14, 100, null, 14, 5, null, 12015 },
                    { 71, 71, 0, 71, 100, null, 71, 5, null, 10423 },
                    { 100, 100, 0, 100, 100, null, 100, 5, null, 13167 }
                });

            migrationBuilder.InsertData(
                table: "Domains",
                columns: new[] { "Id", "DomainJson", "Fortifications", "Gold", "Investments", "Name", "Size", "SuzerainId", "TurnOfDefeat", "UserId" },
                values: new object[,]
                {
                    { 22, null, 18020, 6120, 45000, "Каменный берег", 4310, 23, -2147483648, null },
                    { 79, null, 18360, 5400, 159000, "Жёлудь", 1840, 82, -2147483648, null },
                    { 11, null, 16320, 5640, 49000, "Мыс морского дракона", 3810, 12, -2147483648, null },
                    { 13, null, 17000, 5640, 159000, "Железный холм", 2010, 12, -2147483648, null },
                    { 20, null, 1800, 5400, 45000, "Волчий лес", 5810, 12, -2147483648, null },
                    { 44, null, 41160, 5400, 159000, "Волмарк", 620, 39, -2147483648, null },
                    { 40, null, 37800, 5400, 135000, "Камнедрев", 520, 39, -2147483648, null },
                    { 8, null, 15300, 5640, 159000, "Вершина", 3510, 15, -2147483648, null },
                    { 16, null, 18020, 5640, 147000, "Вдовий дозор", 4510, 15, -2147483648, null },
                    { 17, null, 16320, 6120, 135000, "Хорнвуд", 4810, 15, -2147483648, null },
                    { 27, null, 16320, 6120, 135000, "Старый замок", 3410, 25, -2147483648, null },
                    { 26, null, 18020, 5640, 147000, "Бараньи ворота", 3610, 25, -2147483648, null },
                    { 51, null, 16320, 5640, 147000, "Ледяной ручей", 2030, 62, -2147483648, null },
                    { 18, null, 15300, 5640, 159000, "Чёрная заводь", 2510, 15, -2147483648, null },
                    { 45, null, 42000, 5400, 135000, "Длинная сестра", 1530, 46, -2147483648, null },
                    { 47, null, 40320, 6120, 135000, "Малая сестра", 1030, 46, -2147483648, null },
                    { 68, null, 1800, 5640, 159000, "Старые камни", 3440, 66, -2147483648, null },
                    { 69, null, 2160, 5400, 159000, "Добрая ярмарка", 2040, 66, -2147483648, null },
                    { 61, null, 16320, 5640, 147000, "Серая лощина", 930, 62, -2147483648, null }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId", "RouteJson" },
                values: new object[,]
                {
                    { 78, 73, null },
                    { 78, 74, null },
                    { 81, 80, null },
                    { 81, 71, null },
                    { 80, 81, null },
                    { 71, 81, null },
                    { 78, 75, null },
                    { 78, 77, null },
                    { 71, 80, null },
                    { 72, 80, null },
                    { 80, 71, null },
                    { 80, 82, null },
                    { 80, 72, null },
                    { 81, 82, null },
                    { 78, 83, null },
                    { 82, 81, null },
                    { 86, 42, null },
                    { 86, 43, null },
                    { 85, 86, null },
                    { 43, 86, null },
                    { 42, 86, null },
                    { 39, 86, null },
                    { 85, 71, null },
                    { 85, 70, null },
                    { 85, 39, null },
                    { 71, 85, null },
                    { 70, 85, null },
                    { 82, 80, null },
                    { 39, 85, null },
                    { 84, 78, null },
                    { 84, 83, null },
                    { 84, 82, null },
                    { 83, 84, null },
                    { 82, 84, null },
                    { 81, 84, null },
                    { 78, 84, null },
                    { 83, 82, null },
                    { 83, 78, null },
                    { 82, 83, null },
                    { 77, 78, null },
                    { 84, 81, null },
                    { 1, 2, null },
                    { 76, 77, null },
                    { 74, 78, null },
                    { 72, 71, null },
                    { 72, 70, null },
                    { 71, 72, null },
                    { 70, 72, null },
                    { 71, 70, null },
                    { 70, 39, null },
                    { 70, 71, null },
                    { 39, 70, null },
                    { 67, 30, null },
                    { 67, 31, null },
                    { 67, 39, null },
                    { 67, 66, null },
                    { 66, 67, null },
                    { 39, 67, null },
                    { 31, 67, null },
                    { 30, 67, null },
                    { 108, 84, null },
                    { 66, 29, null },
                    { 50, 66, null },
                    { 29, 66, null },
                    { 65, 64, null },
                    { 70, 73, null },
                    { 72, 73, null },
                    { 73, 70, null },
                    { 73, 72, null },
                    { 73, 78, null },
                    { 77, 76, null },
                    { 77, 75, null },
                    { 86, 39, null },
                    { 75, 77, null },
                    { 65, 77, null },
                    { 76, 65, null },
                    { 76, 75, null },
                    { 75, 76, null },
                    { 65, 76, null },
                    { 75, 78, null },
                    { 75, 65, null },
                    { 75, 74, null },
                    { 74, 75, null },
                    { 65, 75, null },
                    { 59, 75, null },
                    { 74, 70, null },
                    { 74, 73, null },
                    { 74, 66, null },
                    { 73, 74, null },
                    { 70, 74, null },
                    { 66, 74, null },
                    { 75, 59, null },
                    { 86, 85, null },
                    { 87, 88, null },
                    { 87, 86, null },
                    { 93, 101, null },
                    { 100, 99, null },
                    { 99, 98, null },
                    { 99, 96, null },
                    { 99, 100, null },
                    { 98, 99, null },
                    { 96, 99, null },
                    { 98, 97, null },
                    { 98, 96, null },
                    { 97, 98, null },
                    { 98, 101, null },
                    { 96, 98, null },
                    { 97, 96, null },
                    { 96, 97, null },
                    { 84, 97, null },
                    { 100, 96, null },
                    { 96, 100, null },
                    { 96, 84, null },
                    { 96, 81, null },
                    { 96, 95, null },
                    { 95, 96, null },
                    { 84, 96, null },
                    { 97, 84, null },
                    { 81, 96, null },
                    { 99, 101, null },
                    { 101, 100, null },
                    { 108, 97, null },
                    { 97, 108, null },
                    { 84, 108, null },
                    { 107, 106, null },
                    { 107, 97, null },
                    { 107, 98, null },
                    { 106, 107, null },
                    { 98, 107, null },
                    { 97, 107, null },
                    { 106, 98, null },
                    { 100, 101, null },
                    { 106, 101, null },
                    { 105, 106, null },
                    { 101, 106, null },
                    { 98, 106, null },
                    { 105, 101, null },
                    { 105, 93, null },
                    { 101, 105, null },
                    { 93, 105, null },
                    { 101, 93, null },
                    { 101, 98, null },
                    { 101, 99, null },
                    { 106, 105, null },
                    { 86, 87, null },
                    { 100, 95, null },
                    { 95, 81, null },
                    { 91, 87, null },
                    { 90, 91, null },
                    { 88, 91, null },
                    { 87, 91, null },
                    { 90, 89, null },
                    { 90, 88, null },
                    { 89, 90, null },
                    { 88, 90, null },
                    { 89, 81, null },
                    { 89, 71, null },
                    { 91, 88, null },
                    { 89, 88, null },
                    { 81, 89, null },
                    { 71, 89, null },
                    { 88, 87, null },
                    { 88, 71, null },
                    { 88, 85, null },
                    { 88, 86, null },
                    { 64, 65, null },
                    { 86, 88, null },
                    { 85, 88, null },
                    { 71, 88, null },
                    { 88, 89, null },
                    { 95, 100, null },
                    { 91, 90, null },
                    { 91, 92, null },
                    { 95, 89, null },
                    { 95, 90, null },
                    { 95, 94, null },
                    { 94, 95, null },
                    { 90, 95, null },
                    { 89, 95, null },
                    { 81, 95, null },
                    { 100, 94, null },
                    { 94, 100, null },
                    { 94, 90, null },
                    { 87, 92, null },
                    { 94, 91, null },
                    { 93, 94, null },
                    { 91, 94, null },
                    { 90, 94, null },
                    { 100, 93, null },
                    { 93, 100, null },
                    { 93, 91, null },
                    { 93, 92, null },
                    { 92, 93, null },
                    { 92, 91, null },
                    { 92, 87, null },
                    { 94, 93, null },
                    { 64, 63, null },
                    { 66, 50, null },
                    { 7, 14, null },
                    { 37, 38, null },
                    { 32, 38, null },
                    { 31, 38, null },
                    { 1, 9, null },
                    { 37, 33, null },
                    { 9, 1, null },
                    { 37, 36, null },
                    { 37, 32, null },
                    { 36, 37, null },
                    { 33, 37, null },
                    { 32, 37, null },
                    { 24, 23, null },
                    { 43, 36, null },
                    { 36, 35, null },
                    { 9, 10, null },
                    { 38, 31, null },
                    { 24, 19, null },
                    { 38, 37, null },
                    { 38, 32, null },
                    { 36, 42, null },
                    { 7, 5, null },
                    { 41, 39, null },
                    { 41, 37, null },
                    { 7, 6, null },
                    { 39, 41, null },
                    { 37, 41, null },
                    { 36, 43, null },
                    { 19, 21, null },
                    { 24, 21, null },
                    { 39, 37, null },
                    { 39, 31, null },
                    { 38, 39, null },
                    { 37, 39, null },
                    { 31, 39, null },
                    { 14, 7, null },
                    { 39, 38, null },
                    { 36, 33, null },
                    { 64, 62, null },
                    { 35, 36, null },
                    { 30, 31, null },
                    { 25, 24, null },
                    { 30, 24, null },
                    { 30, 23, null },
                    { 24, 30, null },
                    { 23, 30, null },
                    { 6, 15, null },
                    { 12, 9, null },
                    { 25, 19, null },
                    { 28, 29, null },
                    { 7, 15, null },
                    { 14, 15, null },
                    { 28, 24, null },
                    { 28, 25, null },
                    { 25, 28, null },
                    { 24, 28, null },
                    { 29, 28, null },
                    { 41, 42, null },
                    { 31, 30, null },
                    { 31, 32, null },
                    { 33, 36, null },
                    { 15, 14, null },
                    { 43, 35, null },
                    { 35, 43, null },
                    { 35, 33, null },
                    { 33, 35, null },
                    { 10, 9, null },
                    { 12, 10, null },
                    { 19, 25, null },
                    { 33, 34, null },
                    { 9, 12, null },
                    { 33, 32, null },
                    { 32, 33, null },
                    { 10, 12, null },
                    { 24, 25, null },
                    { 32, 31, null },
                    { 34, 33, null },
                    { 15, 7, null },
                    { 7, 4, null },
                    { 42, 43, null },
                    { 1, 4, null },
                    { 59, 58, null },
                    { 58, 59, null },
                    { 2, 4, null },
                    { 58, 57, null },
                    { 58, 55, null },
                    { 57, 58, null },
                    { 55, 58, null },
                    { 3, 4, null },
                    { 4, 1, null },
                    { 57, 56, null },
                    { 57, 55, null },
                    { 56, 57, null },
                    { 23, 21, null },
                    { 55, 57, null },
                    { 58, 60, null },
                    { 21, 23, null },
                    { 59, 60, null },
                    { 60, 59, null },
                    { 64, 60, null },
                    { 63, 64, null },
                    { 2, 1, null },
                    { 62, 64, null },
                    { 60, 64, null },
                    { 21, 19, null },
                    { 63, 62, null },
                    { 4, 2, null },
                    { 62, 63, null },
                    { 19, 14, null },
                    { 62, 56, null },
                    { 62, 57, null },
                    { 57, 62, null },
                    { 56, 62, null },
                    { 3, 2, null },
                    { 60, 58, null },
                    { 2, 3, null },
                    { 56, 55, null },
                    { 55, 56, null },
                    { 4, 3, null },
                    { 21, 24, null },
                    { 59, 52, null },
                    { 52, 59, null },
                    { 5, 6, null },
                    { 50, 49, null },
                    { 50, 48, null },
                    { 49, 50, null },
                    { 48, 50, null },
                    { 6, 5, null },
                    { 23, 24, null },
                    { 4, 7, null },
                    { 5, 7, null },
                    { 6, 7, null },
                    { 43, 42, null },
                    { 42, 36, null },
                    { 5, 4, null },
                    { 42, 41, null },
                    { 50, 53, null },
                    { 19, 24, null },
                    { 59, 55, null },
                    { 14, 19, null },
                    { 55, 59, null },
                    { 55, 54, null },
                    { 54, 55, null },
                    { 3, 5, null },
                    { 59, 54, null },
                    { 53, 50, null },
                    { 54, 59, null },
                    { 54, 50, null },
                    { 54, 53, null },
                    { 54, 52, null },
                    { 53, 54, null },
                    { 5, 3, null },
                    { 52, 54, null },
                    { 50, 54, null },
                    { 4, 5, null },
                    { 15, 6, null }
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "DomainId", "Gold", "PositionDomainId", "Status", "Target2DomainId", "TargetDomainId", "Type", "UnitJson", "Warriors" },
                values: new object[,]
                {
                    { 15, 15, 0, 15, 100, null, 15, 5, null, 5630 },
                    { 64, 64, 0, 64, 100, null, 64, 5, null, 3213 },
                    { 12, 12, 0, 12, 100, null, 12, 5, null, 3753 },
                    { 1, 1, 0, 1, 100, null, 1, 5, null, 349 },
                    { 107, 107, 0, 107, 100, null, 107, 5, null, 2192 },
                    { 3, 3, 0, 3, 100, null, 3, 5, null, 410 },
                    { 106, 106, 0, 106, 100, null, 106, 5, null, 2070 },
                    { 4, 4, 0, 4, 100, null, 4, 5, null, 2070 },
                    { 105, 105, 0, 105, 100, null, 105, 5, null, 2030 },
                    { 5, 5, 0, 5, 100, null, 5, 5, null, 2030 },
                    { 101, 101, 0, 101, 100, null, 101, 5, null, 2964 },
                    { 6, 6, 0, 6, 100, null, 6, 5, null, 2070 },
                    { 99, 99, 0, 99, 100, null, 99, 5, null, 1867 },
                    { 7, 7, 0, 7, 100, null, 7, 5, null, 2192 },
                    { 9, 9, 0, 9, 100, null, 9, 5, null, 1867 },
                    { 98, 98, 0, 98, 100, null, 98, 5, null, 1989 },
                    { 10, 10, 0, 10, 100, null, 10, 5, null, 1602 },
                    { 97, 97, 0, 97, 100, null, 97, 5, null, 2192 },
                    { 96, 96, 0, 96, 100, null, 96, 5, null, 2070 },
                    { 95, 95, 0, 95, 100, null, 95, 5, null, 2030 },
                    { 87, 87, 0, 87, 100, null, 87, 5, null, 2192 },
                    { 21, 21, 0, 21, 100, null, 21, 5, null, 1867 },
                    { 42, 42, 0, 42, 100, null, 42, 5, null, 2332 },
                    { 46, 46, 0, 46, 100, null, 46, 5, null, 3651 },
                    { 48, 48, 0, 48, 100, null, 48, 5, null, 1744 },
                    { 49, 49, 0, 49, 100, null, 49, 5, null, 1637 },
                    { 78, 78, 0, 78, 100, null, 78, 5, null, 1744 },
                    { 50, 50, 0, 50, 100, null, 50, 5, null, 1827 },
                    { 52, 52, 0, 52, 100, null, 52, 5, null, 1989 },
                    { 77, 77, 0, 77, 100, null, 77, 5, null, 3099 },
                    { 53, 53, 0, 53, 100, null, 53, 5, null, 2192 },
                    { 76, 76, 0, 76, 100, null, 76, 5, null, 2682 },
                    { 75, 75, 0, 75, 100, null, 75, 5, null, 2030 },
                    { 54, 54, 0, 54, 100, null, 54, 5, null, 2070 },
                    { 80, 80, 0, 80, 100, null, 80, 5, null, 1827 },
                    { 74, 74, 0, 74, 100, null, 74, 5, null, 2682 },
                    { 56, 56, 0, 56, 100, null, 56, 5, null, 2070 },
                    { 73, 73, 0, 73, 100, null, 73, 5, null, 2192 },
                    { 57, 57, 0, 57, 100, null, 57, 5, null, 2192 },
                    { 72, 72, 0, 72, 100, null, 72, 5, null, 1989 },
                    { 70, 70, 0, 70, 100, null, 70, 5, null, 1827 },
                    { 58, 58, 0, 58, 100, null, 58, 5, null, 1989 },
                    { 67, 67, 0, 67, 100, null, 67, 5, null, 2192 },
                    { 60, 60, 0, 60, 100, null, 60, 5, null, 1827 },
                    { 66, 66, 0, 66, 100, null, 66, 5, null, 3906 },
                    { 62, 62, 0, 62, 100, null, 62, 5, null, 4341 },
                    { 63, 63, 0, 63, 100, null, 63, 5, null, 1922 },
                    { 65, 65, 0, 65, 100, null, 65, 5, null, 2030 },
                    { 55, 55, 0, 55, 100, null, 55, 5, null, 2030 },
                    { 19, 19, 0, 19, 100, null, 19, 5, null, 1867 },
                    { 81, 81, 0, 81, 100, null, 81, 5, null, 1867 },
                    { 82, 82, 0, 82, 100, null, 82, 5, null, 1989 },
                    { 23, 23, 0, 23, 100, null, 23, 5, null, 2192 },
                    { 94, 94, 0, 94, 100, null, 94, 5, null, 2070 },
                    { 24, 24, 0, 24, 100, null, 24, 5, null, 2927 },
                    { 93, 93, 0, 93, 100, null, 93, 5, null, 2192 },
                    { 25, 25, 0, 25, 100, null, 25, 5, null, 4830 },
                    { 92, 92, 0, 92, 100, null, 92, 5, null, 1744 },
                    { 28, 28, 0, 28, 100, null, 28, 5, null, 29 },
                    { 29, 29, 0, 29, 100, null, 29, 5, null, 579 },
                    { 91, 91, 0, 91, 100, null, 91, 5, null, 2005 },
                    { 30, 30, 0, 30, 100, null, 30, 5, null, 1827 },
                    { 90, 90, 0, 90, 100, null, 90, 5, null, 1827 },
                    { 31, 31, 0, 31, 100, null, 31, 5, null, 1637 },
                    { 41, 41, 0, 41, 100, null, 41, 5, null, 1637 },
                    { 89, 89, 0, 89, 100, null, 89, 5, null, 1867 },
                    { 33, 33, 0, 33, 100, null, 33, 5, null, 1922 },
                    { 88, 88, 0, 88, 100, null, 88, 5, null, 2136 },
                    { 34, 34, 0, 34, 100, null, 34, 5, null, 387 },
                    { 35, 35, 0, 35, 100, null, 35, 5, null, 1780 },
                    { 86, 86, 0, 86, 100, null, 86, 5, null, 2070 },
                    { 36, 36, 0, 36, 100, null, 36, 5, null, 1815 },
                    { 85, 85, 0, 85, 100, null, 85, 5, null, 2030 },
                    { 37, 37, 0, 37, 100, null, 37, 5, null, 1922 },
                    { 84, 84, 0, 84, 100, null, 84, 5, null, 2070 },
                    { 38, 38, 0, 38, 100, null, 38, 5, null, 1744 },
                    { 83, 83, 0, 83, 100, null, 83, 5, null, 2192 },
                    { 39, 39, 0, 39, 100, null, 39, 5, null, 3845 },
                    { 32, 32, 0, 32, 100, null, 32, 5, null, 1744 },
                    { 108, 108, 0, 108, 100, null, 108, 5, null, 1989 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId", "RouteJson" },
                values: new object[,]
                {
                    { 10, 11, null },
                    { 44, 39, null },
                    { 44, 86, null },
                    { 44, 42, null },
                    { 86, 44, null },
                    { 72, 79, null },
                    { 27, 45, null },
                    { 44, 40, null },
                    { 29, 45, null },
                    { 45, 27, null },
                    { 45, 46, null },
                    { 45, 66, null },
                    { 46, 45, null },
                    { 66, 45, null },
                    { 27, 47, null },
                    { 45, 29, null },
                    { 42, 44, null },
                    { 40, 44, null },
                    { 39, 44, null },
                    { 27, 26, null },
                    { 27, 46, null },
                    { 27, 29, null },
                    { 27, 28, null },
                    { 28, 27, null },
                    { 29, 27, null },
                    { 46, 27, null },
                    { 83, 79, null },
                    { 39, 40, null },
                    { 40, 39, null },
                    { 40, 42, null },
                    { 40, 41, null },
                    { 41, 40, null },
                    { 42, 40, null },
                    { 73, 79, null },
                    { 45, 47, null },
                    { 27, 25, null },
                    { 46, 47, null },
                    { 47, 46, null },
                    { 62, 61, null },
                    { 64, 61, null },
                    { 69, 70, null },
                    { 39, 68, null },
                    { 66, 68, null },
                    { 67, 68, null },
                    { 61, 60, null },
                    { 68, 67, null },
                    { 68, 70, null },
                    { 68, 39, null },
                    { 70, 68, null },
                    { 69, 74, null },
                    { 66, 69, null },
                    { 68, 69, null },
                    { 68, 66, null },
                    { 61, 64, null },
                    { 61, 62, null },
                    { 61, 58, null },
                    { 47, 27, null },
                    { 47, 48, null },
                    { 47, 50, null },
                    { 47, 66, null },
                    { 48, 47, null },
                    { 50, 47, null },
                    { 66, 47, null },
                    { 74, 69, null },
                    { 50, 51, null },
                    { 51, 50, null },
                    { 51, 53, null },
                    { 53, 51, null },
                    { 70, 69, null },
                    { 58, 61, null },
                    { 60, 61, null },
                    { 47, 45, null },
                    { 69, 68, null },
                    { 26, 27, null },
                    { 78, 79, null },
                    { 20, 13, null },
                    { 20, 14, null },
                    { 20, 19, null },
                    { 20, 21, null },
                    { 20, 11, null },
                    { 21, 20, null },
                    { 79, 82, null },
                    { 4, 8, null },
                    { 7, 8, null },
                    { 8, 9, null },
                    { 8, 4, null },
                    { 8, 7, null },
                    { 8, 14, null },
                    { 8, 13, null },
                    { 9, 8, null },
                    { 20, 12, null },
                    { 13, 8, null },
                    { 19, 20, null },
                    { 13, 20, null },
                    { 11, 10, null },
                    { 11, 12, null },
                    { 11, 21, null },
                    { 12, 11, null },
                    { 21, 11, null },
                    { 82, 79, null },
                    { 9, 13, null },
                    { 12, 13, null },
                    { 13, 9, null },
                    { 13, 14, null },
                    { 13, 12, null },
                    { 14, 13, null },
                    { 80, 79, null },
                    { 11, 20, null },
                    { 12, 20, null },
                    { 14, 20, null },
                    { 25, 27, null },
                    { 14, 8, null },
                    { 15, 16, null },
                    { 25, 18, null },
                    { 79, 72, null },
                    { 11, 22, null },
                    { 21, 22, null },
                    { 22, 11, null },
                    { 22, 21, null },
                    { 22, 23, null },
                    { 23, 22, null },
                    { 79, 80, null },
                    { 16, 26, null },
                    { 17, 26, null },
                    { 25, 26, null },
                    { 26, 17, null },
                    { 26, 16, null },
                    { 26, 25, null },
                    { 19, 18, null },
                    { 79, 83, null },
                    { 18, 19, null },
                    { 18, 17, null },
                    { 16, 15, null },
                    { 79, 78, null },
                    { 14, 17, null },
                    { 15, 17, null },
                    { 16, 17, null },
                    { 17, 14, null },
                    { 18, 25, null },
                    { 17, 15, null },
                    { 17, 25, null },
                    { 25, 17, null },
                    { 79, 73, null },
                    { 14, 18, null },
                    { 17, 18, null },
                    { 18, 14, null },
                    { 17, 16, null },
                    { 69, 66, null }
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "DomainId", "Gold", "PositionDomainId", "Status", "Target2DomainId", "TargetDomainId", "Type", "UnitJson", "Warriors" },
                values: new object[,]
                {
                    { 69, 69, 0, 69, 100, null, 69, 5, null, 2005 },
                    { 27, 27, 0, 27, 100, null, 27, 5, null, 2192 },
                    { 61, 61, 0, 61, 100, null, 61, 5, null, 1867 },
                    { 11, 11, 0, 11, 100, null, 11, 5, null, 579 },
                    { 13, 13, 0, 13, 100, null, 13, 5, null, 2192 },
                    { 20, 20, 0, 20, 100, null, 20, 5, null, 702 },
                    { 8, 8, 0, 8, 100, null, 8, 5, null, 1989 },
                    { 16, 16, 0, 16, 100, null, 16, 5, null, 2070 },
                    { 17, 17, 0, 17, 100, null, 17, 5, null, 2192 },
                    { 68, 68, 0, 68, 100, null, 68, 5, null, 2136 },
                    { 18, 18, 0, 18, 100, null, 18, 5, null, 1989 },
                    { 26, 26, 0, 26, 100, null, 26, 5, null, 2070 },
                    { 40, 40, 0, 40, 100, null, 40, 5, null, 1602 },
                    { 44, 44, 0, 44, 100, null, 44, 5, null, 1815 },
                    { 45, 45, 0, 45, 100, null, 45, 5, null, 1780 },
                    { 47, 47, 0, 47, 100, null, 47, 5, null, 1922 },
                    { 51, 51, 0, 51, 100, null, 51, 5, null, 1867 },
                    { 22, 22, 0, 22, 100, null, 22, 5, null, 617 },
                    { 79, 79, 0, 79, 100, null, 79, 5, null, 1867 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Commands_DomainId",
                table: "Commands",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Commands_ExecutorType_ExecutorId",
                table: "Commands",
                columns: new[] { "ExecutorType", "ExecutorId" });

            migrationBuilder.CreateIndex(
                name: "IX_Commands_Target2DomainId",
                table: "Commands",
                column: "Target2DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Commands_TargetDomainId",
                table: "Commands",
                column: "TargetDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Commands_Type",
                table: "Commands",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Domains_Size",
                table: "Domains",
                column: "Size",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Domains_SuzerainId",
                table: "Domains",
                column: "SuzerainId");

            migrationBuilder.CreateIndex(
                name: "IX_Domains_UserId",
                table: "Domains",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventObjects_DomainId",
                table: "EventObjects",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_EventObjects_TurnId_EventStoryId",
                table: "EventObjects",
                columns: new[] { "TurnId", "EventStoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_Relations_SourceDomainId",
                table: "Relations",
                column: "SourceDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Relations_SourceDomainId_TargetDomainId",
                table: "Relations",
                columns: new[] { "SourceDomainId", "TargetDomainId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Relations_TargetDomainId",
                table: "Relations",
                column: "TargetDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_FromDomainId",
                table: "Routes",
                column: "FromDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ToDomainId",
                table: "Routes",
                column: "ToDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_DomainId",
                table: "Units",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_PositionDomainId",
                table: "Units",
                column: "PositionDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_Target2DomainId",
                table: "Units",
                column: "Target2DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_TargetDomainId",
                table: "Units",
                column: "TargetDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_Type",
                table: "Units",
                column: "Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Commands");

            migrationBuilder.DropTable(
                name: "Errors");

            migrationBuilder.DropTable(
                name: "EventObjects");

            migrationBuilder.DropTable(
                name: "Relations");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Domains");

            migrationBuilder.DropTable(
                name: "Turns");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
