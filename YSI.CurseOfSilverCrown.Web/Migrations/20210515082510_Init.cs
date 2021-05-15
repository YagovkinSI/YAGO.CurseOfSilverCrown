using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Domains",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Coffers = table.Column<int>(type: "int", nullable: false),
                    Warriors = table.Column<int>(type: "int", nullable: false),
                    Investments = table.Column<int>(type: "int", nullable: false),
                    Fortifications = table.Column<int>(type: "int", nullable: false),
                    SuzerainId = table.Column<int>(type: "int", nullable: true),
                    TurnOfDefeat = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Domains_Domains_SuzerainId",
                        column: x => x.SuzerainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Errors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Turns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Started = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DomainId = table.Column<int>(type: "int", nullable: true),
                    LastActivityTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Commands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainId = table.Column<int>(type: "int", nullable: false),
                    Coffers = table.Column<int>(type: "int", nullable: false),
                    Warriors = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    TargetDomainId = table.Column<int>(type: "int", nullable: true),
                    Target2DomainId = table.Column<int>(type: "int", nullable: true),
                    InitiatorDomainId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    InitiatorId = table.Column<int>(type: "int", nullable: true),
                    TypeInt = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_Commands_Domains_InitiatorId",
                        column: x => x.InitiatorId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Routes",
                columns: table => new
                {
                    FromDomainId = table.Column<int>(type: "int", nullable: false),
                    ToDomainId = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainId = table.Column<int>(type: "int", nullable: false),
                    Coffers = table.Column<int>(type: "int", nullable: false),
                    Warriors = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    TargetDomainId = table.Column<int>(type: "int", nullable: true),
                    Target2DomainId = table.Column<int>(type: "int", nullable: true),
                    InitiatorDomainId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    InitiatorId = table.Column<int>(type: "int", nullable: true),
                    TypeInt = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_Units_Domains_InitiatorId",
                        column: x => x.InitiatorId,
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
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.InsertData(
                table: "Domains",
                columns: new[] { "Id", "Coffers", "Fortifications", "Investments", "Name", "SuzerainId", "TurnOfDefeat", "Warriors" },
                values: new object[,]
                {
                    { 1, 3760, 4800, 0, "Сумеречная башня", null, -2147483648, 92 },
                    { 77, 4080, 4800, 0, "Девичий пруд", null, -2147483648, 108 },
                    { 76, 3760, 5300, 0, "Солеварни", null, -2147483648, 102 },
                    { 75, 3600, 5000, 0, "Дарри", null, -2147483648, 100 },
                    { 74, 3600, 4900, 0, "Город Харровея", null, -2147483648, 102 },
                    { 73, 3760, 5000, 0, "Замок Личестеров", null, -2147483648, 108 },
                    { 72, 4080, 5300, 0, "Каменный оплот", null, -2147483648, 98 },
                    { 71, 3760, 4800, 0, "Риверран", null, -2147483648, 92 },
                    { 70, 3600, 4500, 0, "Вранодрев", null, -2147483648, 90 },
                    { 69, 3600, 5400, 0, "Добрая ярмарка", null, -2147483648, 92 },
                    { 68, 3760, 4500, 0, "Старые камни", null, -2147483648, 98 },
                    { 78, 3760, 4500, 0, "Харренхол", null, -2147483648, 98 },
                    { 67, 4080, 4800, 0, "Сигард", null, -2147483648, 108 },
                    { 65, 3600, 5000, 0, "Фитили", null, -2147483648, 100 },
                    { 64, 3600, 4900, 0, "Чаячий город", null, -2147483648, 102 },
                    { 63, 3760, 5000, 0, "Ведьмин остров", null, -2147483648, 108 },
                    { 62, 4080, 5300, 0, "Рунный камень", null, -2147483648, 98 },
                    { 61, 3760, 4800, 0, "Серая лощина", null, -2147483648, 92 },
                    { 60, 3600, 4500, 0, "Редфорт", null, -2147483648, 90 },
                    { 59, 3600, 5400, 0, "Орлиное гнездо", null, -2147483648, 92 },
                    { 58, 3760, 4500, 0, "Железная дубрава", null, -2147483648, 98 },
                    { 57, 4080, 4800, 0, "Девять звёзд", null, -2147483648, 108 },
                    { 56, 3760, 5300, 0, "Старый якорь", null, -2147483648, 102 },
                    { 66, 3760, 5300, 0, "Близнецы", null, -2147483648, 102 },
                    { 79, 3600, 5400, 0, "Жёлудь", null, -2147483648, 92 },
                    { 80, 3600, 4500, 0, "Атранта", null, -2147483648, 90 },
                    { 81, 3760, 4800, 0, "Розовая дева", null, -2147483648, 92 },
                    { 107, 4080, 4800, 0, "Гринфилд", null, -2147483648, 108 },
                    { 106, 3760, 5300, 0, "Корнфилд", null, -2147483648, 102 },
                    { 105, 3600, 5000, 0, "Крейкхолл", null, -2147483648, 100 },
                    { 101, 3760, 4800, 0, "Ланниспорт", null, -2147483648, 92 },
                    { 100, 3600, 4500, 0, "Утёс Кастерли", null, -2147483648, 90 },
                    { 99, 3600, 5400, 0, "Замок Клиганов", null, -2147483648, 92 },
                    { 98, 3760, 4500, 0, "Серебрянный холм", null, -2147483648, 98 },
                    { 97, 4080, 4800, 0, "Ключи", null, -2147483648, 108 },
                    { 96, 3760, 5300, 0, "Глубокая нора", null, -2147483648, 102 },
                    { 95, 3600, 5000, 0, "Хорнваль", null, -2147483648, 100 },
                    { 94, 3600, 4900, 0, "Сарсфилд", null, -2147483648, 102 },
                    { 93, 3760, 5000, 0, "Пиршественные огни", null, -2147483648, 108 },
                    { 92, 4080, 5300, 0, "Светлый остров", null, -2147483648, 98 },
                    { 91, 3760, 4800, 0, "Тарбекхолл", null, -2147483648, 92 },
                    { 90, 3600, 4500, 0, "Эшмарк", null, -2147483648, 90 }
                });

            migrationBuilder.InsertData(
                table: "Domains",
                columns: new[] { "Id", "Coffers", "Fortifications", "Investments", "Name", "SuzerainId", "TurnOfDefeat", "Warriors" },
                values: new object[,]
                {
                    { 89, 3600, 5400, 0, "Золотой зуб", null, -2147483648, 92 },
                    { 88, 3760, 4500, 0, "Кастамере", null, -2147483648, 98 },
                    { 87, 4080, 4800, 0, "Скала", null, -2147483648, 108 },
                    { 86, 3760, 5300, 0, "Гибельная крепость", null, -2147483648, 102 },
                    { 85, 3600, 5000, 0, "Виндхолл", null, -2147483648, 100 },
                    { 84, 3600, 4900, 0, "Каменная септа", null, -2147483648, 102 },
                    { 83, 3760, 5000, 0, "Каслвуд", null, -2147483648, 108 },
                    { 82, 4080, 5300, 0, "Приют странника", null, -2147483648, 98 },
                    { 55, 3600, 5000, 0, "Длинный лук", null, -2147483648, 100 },
                    { 54, 3600, 4900, 0, "Дом сердец", null, -2147483648, 102 },
                    { 53, 3760, 5000, 0, "Змеиный лес", null, -2147483648, 108 },
                    { 52, 4080, 5300, 0, "Суровая песнь", null, -2147483648, 98 },
                    { 24, 3600, 4900, 0, "Барроутон", null, -2147483648, 102 },
                    { 23, 3760, 5000, 0, "Родники", null, -2147483648, 108 },
                    { 22, 4080, 5300, 0, "Каменный берег", null, -2147483648, 98 },
                    { 21, 3760, 4800, 0, "Торхенов удел", null, -2147483648, 92 },
                    { 20, 3600, 4500, 0, "Волчий лес", null, -2147483648, 90 },
                    { 19, 3600, 5400, 0, "Замок Сервинов", null, -2147483648, 92 },
                    { 18, 3760, 4500, 0, "Чёрная заводь", null, -2147483648, 98 },
                    { 17, 4080, 4800, 0, "Хорнвуд", null, -2147483648, 108 },
                    { 16, 3760, 5300, 0, "Вдовий дозор", null, -2147483648, 102 },
                    { 15, 3600, 5000, 0, "Дредфорт", null, -2147483648, 100 },
                    { 14, 3600, 4900, 0, "Винтерфелл", null, -2147483648, 102 },
                    { 13, 3760, 5000, 0, "Железный холм", null, -2147483648, 108 },
                    { 12, 4080, 5300, 0, "Темнолесье", null, -2147483648, 98 },
                    { 11, 3760, 4800, 0, "Мыс морского дракона", null, -2147483648, 92 },
                    { 10, 3600, 4500, 0, "Медвежий остров", null, -2147483648, 90 },
                    { 9, 3600, 5400, 0, "Каменный холм", null, -2147483648, 92 },
                    { 8, 3760, 4500, 0, "Вершина", null, -2147483648, 98 },
                    { 7, 4080, 4800, 0, "Последний очаг", null, -2147483648, 108 },
                    { 6, 3760, 5300, 0, "Кархолд", null, -2147483648, 102 },
                    { 5, 3600, 5000, 0, "Скагос", null, -2147483648, 100 },
                    { 4, 3600, 4900, 0, "Новый дар", null, -2147483648, 102 },
                    { 3, 3760, 5000, 0, "Восточный дозор", null, -2147483648, 108 },
                    { 2, 4080, 5300, 0, "Чёрный замок", null, -2147483648, 98 },
                    { 25, 3600, 5000, 0, "Белая гавань", null, -2147483648, 100 },
                    { 108, 3760, 4500, 0, "Золотая дорога", null, -2147483648, 98 },
                    { 26, 3760, 5300, 0, "Бараньи ворота", null, -2147483648, 102 },
                    { 28, 3760, 4500, 0, "Ров Кайлин", null, -2147483648, 98 },
                    { 51, 3760, 4800, 0, "Ледяноый ручей", null, -2147483648, 92 },
                    { 50, 3600, 4500, 0, "Персты", null, -2147483648, 90 },
                    { 49, 3600, 5400, 0, "Сосцы", null, -2147483648, 92 }
                });

            migrationBuilder.InsertData(
                table: "Domains",
                columns: new[] { "Id", "Coffers", "Fortifications", "Investments", "Name", "SuzerainId", "TurnOfDefeat", "Warriors" },
                values: new object[,]
                {
                    { 48, 3760, 4500, 0, "Галечный остров", null, -2147483648, 98 },
                    { 47, 4080, 4800, 0, "Малая сестра", null, -2147483648, 108 },
                    { 46, 3760, 5300, 0, "Милая сестра", null, -2147483648, 102 },
                    { 45, 3600, 5000, 0, "Длинная сестра", null, -2147483648, 100 },
                    { 44, 3600, 4900, 0, "Волмарк", null, -2147483648, 102 },
                    { 43, 3760, 5000, 0, "Пайк", null, -2147483648, 108 },
                    { 42, 4080, 5300, 0, "Лордпорт", null, -2147483648, 98 },
                    { 41, 3760, 4800, 0, "Железная роща", null, -2147483648, 92 },
                    { 40, 3600, 4500, 0, "Камнедрев", null, -2147483648, 90 },
                    { 39, 3600, 5400, 0, "Десять башен", null, -2147483648, 92 },
                    { 38, 3760, 4500, 0, "Оркмонт", null, -2147483648, 98 },
                    { 37, 4080, 4800, 0, "Пебблтон", null, -2147483648, 108 },
                    { 36, 3760, 5300, 0, "Гольцы", null, -2147483648, 102 },
                    { 35, 3600, 5000, 0, "Солёный утёс", null, -2147483648, 100 },
                    { 34, 3600, 4900, 0, "Одинокий светоч", null, -2147483648, 102 },
                    { 33, 3760, 5000, 0, "Хаммерхорн", null, -2147483648, 108 },
                    { 32, 4080, 5300, 0, "Старый Вик", null, -2147483648, 98 },
                    { 31, 3760, 4800, 0, "Чёрная волна", null, -2147483648, 92 },
                    { 30, 3600, 4500, 0, "Кремневый палец", null, -2147483648, 90 },
                    { 29, 3600, 5400, 0, "Перешеек", null, -2147483648, 92 },
                    { 27, 4080, 4800, 0, "Старый замок", null, -2147483648, 108 }
                });

            migrationBuilder.InsertData(
                table: "Turns",
                columns: new[] { "Id", "IsActive", "Started" },
                values: new object[] { 1, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 75, 78 },
                    { 74, 78 },
                    { 73, 78 },
                    { 77, 76 },
                    { 77, 74 },
                    { 76, 77 },
                    { 75, 77 },
                    { 65, 77 },
                    { 76, 65 },
                    { 76, 75 },
                    { 75, 76 },
                    { 65, 76 },
                    { 75, 65 },
                    { 75, 59 },
                    { 75, 74 },
                    { 74, 75 },
                    { 65, 75 },
                    { 59, 75 },
                    { 74, 69 },
                    { 74, 70 },
                    { 74, 73 },
                    { 74, 66 },
                    { 73, 74 },
                    { 70, 74 },
                    { 69, 74 },
                    { 66, 74 },
                    { 73, 72 },
                    { 77, 78 },
                    { 73, 70 },
                    { 78, 73 },
                    { 78, 75 },
                    { 83, 79 },
                    { 82, 83 },
                    { 79, 83 },
                    { 78, 83 },
                    { 82, 81 },
                    { 82, 79 },
                    { 82, 80 },
                    { 81, 82 },
                    { 80, 82 },
                    { 79, 82 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId" },
                values: new object[,]
                {
                    { 81, 80 },
                    { 81, 71 },
                    { 80, 81 },
                    { 71, 81 },
                    { 80, 79 },
                    { 80, 72 },
                    { 80, 71 },
                    { 79, 80 },
                    { 72, 80 },
                    { 71, 80 },
                    { 79, 78 },
                    { 79, 73 },
                    { 79, 72 },
                    { 78, 79 },
                    { 73, 79 },
                    { 72, 79 },
                    { 78, 77 },
                    { 78, 74 },
                    { 83, 78 },
                    { 72, 73 },
                    { 72, 71 },
                    { 66, 45 },
                    { 66, 29 },
                    { 50, 66 },
                    { 47, 66 },
                    { 45, 66 },
                    { 29, 66 },
                    { 65, 64 },
                    { 64, 65 },
                    { 64, 63 },
                    { 64, 62 },
                    { 64, 61 },
                    { 64, 60 },
                    { 63, 64 },
                    { 62, 64 },
                    { 61, 64 },
                    { 60, 64 },
                    { 63, 62 },
                    { 62, 63 },
                    { 62, 61 },
                    { 62, 56 },
                    { 62, 57 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId" },
                values: new object[,]
                {
                    { 61, 62 },
                    { 57, 62 },
                    { 56, 62 },
                    { 61, 60 },
                    { 61, 58 },
                    { 60, 61 },
                    { 66, 47 },
                    { 70, 73 },
                    { 66, 50 },
                    { 31, 67 },
                    { 72, 70 },
                    { 71, 72 },
                    { 70, 72 },
                    { 71, 70 },
                    { 70, 71 },
                    { 70, 39 },
                    { 70, 69 },
                    { 70, 68 },
                    { 69, 70 },
                    { 68, 70 },
                    { 39, 70 },
                    { 69, 66 },
                    { 69, 68 },
                    { 68, 69 },
                    { 66, 69 },
                    { 68, 39 },
                    { 68, 66 },
                    { 68, 67 },
                    { 67, 68 },
                    { 66, 68 },
                    { 39, 68 },
                    { 67, 30 },
                    { 67, 31 },
                    { 67, 39 },
                    { 67, 66 },
                    { 66, 67 },
                    { 39, 67 },
                    { 30, 67 },
                    { 83, 82 },
                    { 78, 84 },
                    { 81, 84 },
                    { 99, 100 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId" },
                values: new object[,]
                {
                    { 96, 100 },
                    { 95, 100 },
                    { 94, 100 },
                    { 93, 100 },
                    { 99, 98 },
                    { 99, 96 },
                    { 98, 99 },
                    { 96, 99 },
                    { 98, 97 },
                    { 98, 96 },
                    { 97, 98 },
                    { 96, 98 },
                    { 97, 84 },
                    { 97, 96 },
                    { 96, 97 },
                    { 84, 97 },
                    { 96, 84 },
                    { 96, 81 },
                    { 96, 95 },
                    { 95, 96 },
                    { 84, 96 },
                    { 81, 96 },
                    { 95, 81 },
                    { 95, 89 },
                    { 95, 90 },
                    { 95, 94 },
                    { 100, 93 },
                    { 94, 95 },
                    { 100, 94 },
                    { 100, 96 },
                    { 97, 108 },
                    { 84, 108 },
                    { 107, 106 },
                    { 107, 97 },
                    { 107, 98 },
                    { 106, 107 },
                    { 98, 107 },
                    { 97, 107 },
                    { 106, 98 },
                    { 106, 101 },
                    { 106, 105 },
                    { 105, 106 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId" },
                values: new object[,]
                {
                    { 101, 106 },
                    { 98, 106 },
                    { 105, 101 },
                    { 105, 93 },
                    { 101, 105 },
                    { 93, 105 },
                    { 101, 93 },
                    { 101, 98 },
                    { 101, 99 },
                    { 101, 100 },
                    { 100, 101 },
                    { 99, 101 },
                    { 98, 101 },
                    { 93, 101 },
                    { 100, 99 },
                    { 100, 95 },
                    { 90, 95 },
                    { 89, 95 },
                    { 81, 95 },
                    { 87, 88 },
                    { 86, 88 },
                    { 85, 88 },
                    { 71, 88 },
                    { 87, 86 },
                    { 86, 87 },
                    { 86, 85 },
                    { 86, 39 },
                    { 86, 44 },
                    { 86, 42 },
                    { 86, 43 },
                    { 85, 86 },
                    { 44, 86 },
                    { 43, 86 },
                    { 39, 86 },
                    { 85, 71 },
                    { 85, 70 },
                    { 85, 39 },
                    { 71, 85 },
                    { 70, 85 },
                    { 39, 85 },
                    { 84, 81 },
                    { 84, 78 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId" },
                values: new object[,]
                {
                    { 84, 83 },
                    { 84, 82 },
                    { 83, 84 },
                    { 82, 84 },
                    { 88, 86 },
                    { 88, 85 },
                    { 88, 71 },
                    { 71, 89 },
                    { 94, 90 },
                    { 94, 91 },
                    { 94, 93 },
                    { 93, 94 },
                    { 91, 94 },
                    { 90, 94 },
                    { 93, 91 },
                    { 93, 92 },
                    { 92, 93 },
                    { 92, 91 },
                    { 92, 87 },
                    { 91, 92 },
                    { 87, 92 },
                    { 58, 61 },
                    { 91, 90 },
                    { 91, 87 },
                    { 90, 91 },
                    { 88, 91 },
                    { 87, 91 },
                    { 90, 89 },
                    { 90, 88 },
                    { 89, 90 },
                    { 88, 90 },
                    { 89, 81 },
                    { 89, 71 },
                    { 89, 88 },
                    { 88, 89 },
                    { 81, 89 },
                    { 91, 88 },
                    { 108, 97 },
                    { 60, 58 },
                    { 59, 60 },
                    { 11, 22 },
                    { 21, 11 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId" },
                values: new object[,]
                {
                    { 21, 19 },
                    { 21, 20 },
                    { 20, 21 },
                    { 19, 21 },
                    { 11, 21 },
                    { 20, 11 },
                    { 20, 19 },
                    { 20, 14 },
                    { 20, 13 },
                    { 20, 12 },
                    { 19, 20 },
                    { 14, 20 },
                    { 13, 20 },
                    { 12, 20 },
                    { 11, 20 },
                    { 19, 18 },
                    { 19, 14 },
                    { 18, 19 },
                    { 14, 19 },
                    { 18, 17 },
                    { 18, 14 },
                    { 17, 18 },
                    { 14, 18 },
                    { 17, 16 },
                    { 17, 15 },
                    { 21, 22 },
                    { 17, 14 },
                    { 22, 11 },
                    { 21, 23 },
                    { 27, 26 },
                    { 27, 25 },
                    { 26, 27 },
                    { 25, 27 },
                    { 26, 25 },
                    { 26, 16 },
                    { 26, 17 },
                    { 25, 26 },
                    { 17, 26 },
                    { 16, 26 },
                    { 25, 19 },
                    { 25, 24 },
                    { 25, 17 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId" },
                values: new object[,]
                {
                    { 25, 18 },
                    { 24, 25 },
                    { 19, 25 },
                    { 18, 25 },
                    { 17, 25 },
                    { 24, 23 },
                    { 24, 19 },
                    { 24, 21 },
                    { 23, 24 },
                    { 21, 24 },
                    { 19, 24 },
                    { 23, 21 },
                    { 23, 22 },
                    { 22, 23 },
                    { 22, 21 },
                    { 24, 28 },
                    { 16, 17 },
                    { 14, 17 },
                    { 9, 1 },
                    { 8, 9 },
                    { 1, 9 },
                    { 8, 7 },
                    { 8, 4 },
                    { 7, 8 },
                    { 4, 8 },
                    { 7, 6 },
                    { 7, 5 },
                    { 7, 4 },
                    { 6, 7 },
                    { 5, 7 },
                    { 4, 7 },
                    { 6, 5 },
                    { 5, 6 },
                    { 5, 4 },
                    { 5, 3 },
                    { 3, 5 },
                    { 4, 3 },
                    { 4, 2 },
                    { 4, 1 },
                    { 3, 4 },
                    { 2, 4 },
                    { 1, 4 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId" },
                values: new object[,]
                {
                    { 3, 2 },
                    { 2, 3 },
                    { 2, 1 },
                    { 9, 8 },
                    { 15, 17 },
                    { 9, 10 },
                    { 10, 11 },
                    { 16, 15 },
                    { 15, 16 },
                    { 15, 14 },
                    { 15, 6 },
                    { 15, 7 },
                    { 14, 15 },
                    { 7, 15 },
                    { 6, 15 },
                    { 14, 13 },
                    { 14, 7 },
                    { 14, 8 },
                    { 13, 14 },
                    { 8, 14 },
                    { 7, 14 },
                    { 13, 12 },
                    { 13, 8 },
                    { 13, 9 },
                    { 12, 13 },
                    { 9, 13 },
                    { 8, 13 },
                    { 12, 9 },
                    { 12, 10 },
                    { 12, 11 },
                    { 11, 12 },
                    { 10, 12 },
                    { 9, 12 },
                    { 11, 10 },
                    { 10, 9 },
                    { 25, 28 },
                    { 27, 28 },
                    { 28, 25 },
                    { 50, 51 },
                    { 50, 49 },
                    { 50, 48 },
                    { 50, 47 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId" },
                values: new object[,]
                {
                    { 49, 50 },
                    { 48, 50 },
                    { 47, 50 },
                    { 48, 47 },
                    { 47, 48 },
                    { 47, 27 },
                    { 47, 46 },
                    { 46, 47 },
                    { 45, 47 },
                    { 27, 47 },
                    { 46, 45 },
                    { 46, 27 },
                    { 45, 46 },
                    { 27, 46 },
                    { 45, 27 },
                    { 45, 29 },
                    { 29, 45 },
                    { 27, 45 },
                    { 44, 42 },
                    { 44, 39 },
                    { 44, 40 },
                    { 42, 44 },
                    { 40, 44 },
                    { 51, 50 },
                    { 39, 44 },
                    { 50, 53 },
                    { 53, 51 },
                    { 58, 60 },
                    { 59, 58 },
                    { 59, 55 },
                    { 59, 54 },
                    { 59, 52 },
                    { 58, 59 },
                    { 55, 59 },
                    { 54, 59 },
                    { 52, 59 },
                    { 58, 57 },
                    { 58, 55 },
                    { 57, 58 },
                    { 55, 58 },
                    { 57, 56 },
                    { 57, 55 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId" },
                values: new object[,]
                {
                    { 56, 57 },
                    { 55, 57 },
                    { 56, 55 },
                    { 55, 56 },
                    { 55, 54 },
                    { 54, 55 },
                    { 54, 50 },
                    { 54, 53 },
                    { 53, 54 },
                    { 52, 54 },
                    { 50, 54 },
                    { 53, 50 },
                    { 51, 53 },
                    { 43, 35 },
                    { 43, 42 },
                    { 43, 36 },
                    { 36, 37 },
                    { 33, 37 },
                    { 32, 37 },
                    { 36, 35 },
                    { 36, 33 },
                    { 35, 36 },
                    { 33, 36 },
                    { 35, 32 },
                    { 33, 35 },
                    { 34, 33 },
                    { 33, 34 },
                    { 33, 32 },
                    { 32, 33 },
                    { 32, 31 },
                    { 31, 32 },
                    { 31, 30 },
                    { 30, 31 },
                    { 30, 24 },
                    { 30, 23 },
                    { 24, 30 },
                    { 23, 30 },
                    { 29, 27 },
                    { 29, 28 },
                    { 28, 29 },
                    { 27, 29 },
                    { 28, 24 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "FromDomainId", "ToDomainId" },
                values: new object[,]
                {
                    { 28, 27 },
                    { 37, 32 },
                    { 37, 36 },
                    { 37, 33 },
                    { 31, 38 },
                    { 42, 43 },
                    { 36, 43 },
                    { 35, 43 },
                    { 42, 36 },
                    { 42, 40 },
                    { 42, 41 },
                    { 41, 42 },
                    { 40, 42 },
                    { 36, 42 },
                    { 41, 40 },
                    { 41, 39 },
                    { 41, 37 },
                    { 40, 41 },
                    { 60, 59 },
                    { 39, 41 },
                    { 40, 39 },
                    { 39, 40 },
                    { 39, 38 },
                    { 39, 37 },
                    { 39, 31 },
                    { 38, 39 },
                    { 37, 39 },
                    { 31, 39 },
                    { 38, 32 },
                    { 38, 37 },
                    { 38, 31 },
                    { 37, 38 },
                    { 32, 38 },
                    { 37, 41 },
                    { 108, 84 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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
                name: "IX_AspNetUsers_DomainId",
                table: "AspNetUsers",
                column: "DomainId",
                unique: true,
                filter: "[DomainId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Commands_DomainId",
                table: "Commands",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Commands_InitiatorDomainId",
                table: "Commands",
                column: "InitiatorDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Commands_InitiatorId",
                table: "Commands",
                column: "InitiatorId");

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
                name: "IX_Domains_SuzerainId",
                table: "Domains",
                column: "SuzerainId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEventStories_DomainId",
                table: "OrganizationEventStories",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEventStories_TurnId_EventStoryId",
                table: "OrganizationEventStories",
                columns: new[] { "TurnId", "EventStoryId" });

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
                name: "IX_Units_InitiatorDomainId",
                table: "Units",
                column: "InitiatorDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_InitiatorId",
                table: "Units",
                column: "InitiatorId");

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
                name: "OrganizationEventStories");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "EventStories");

            migrationBuilder.DropTable(
                name: "Domains");

            migrationBuilder.DropTable(
                name: "Turns");
        }
    }
}
