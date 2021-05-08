using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YSI.CurseOfSilverCrown.Web.Migrations
{
    public partial class RebelionTurn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TurnOfDefeat",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Acorn",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Ashmark",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Atranta",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "BaneFortress",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Barrowton",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "BearIsland",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "BlackBackwater",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "BlackCastle",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "BlackWave",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "CapeRaptor",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Carhold",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Castamere",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Castlewood",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "ChalRocks",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "CleansCastle",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Cornfield",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Crackhall",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Darkshire",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Darry",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DeepBurrow",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaFarms",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "DimmoriaValley",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "EasternWatch",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Fingers",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "FlintFinger",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Gemini",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "GoldenRoad",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "GoldenTooth",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "GoodFair",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "GrayHollow",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Greenfield",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Hammerhorn",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Harrenhal",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "HarrowayCity",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "HarshSong",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "HeartsHouse",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "HeatherOfDimmoria",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Hornval",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "IceCreek",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "IronGrove",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "IronHill",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "IronOakwood",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Isthmus",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "KailinMoat",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Keys",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Lannisport",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LeicesterCastle",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LightIsland",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LimestoneRidges",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LittleSister",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Loaches",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LonelyBeacon",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Longbow",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "LongSister",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Lordport",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "MaidenPond",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "MemorableLights",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "MouthOfPolaima",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "NewGift",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "NineStars",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "OldAnchor",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "OldCastle",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "OldVic",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "OlStones",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Orkmont",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "PebbleIsland",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Pebbleton",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "PinkMaiden",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Rock",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Runestone",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SaltCliff",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Saltworks",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Sarsfield",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SeaDragonCape",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Seagard",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "ServinCastle",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SheepsGate",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SilverHill",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Skagos",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SnakeForest",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Socks",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Springs",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "StoneBulwark",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "StoneCoast",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "StoneHill",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "StoneSepta",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Stonewood",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SummerCoast",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "SweetSister",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Tarbekhall",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TeaCity",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TenTowers",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TheLastHearth",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TheTop",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TinMines",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TorchensInheritance",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "TwilightTower",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Walmark",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "WanderersRefuge",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "WhiteHarbor",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Wicks",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "WidowsWatch",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "Windhall",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "WitchIsland",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: "WolfForest",
                column: "TurnOfDefeat",
                value: -2147483648);

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 5, 8, 4, 32, 5, 707, DateTimeKind.Utc).AddTicks(4685));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TurnOfDefeat",
                table: "Organizations");

            migrationBuilder.UpdateData(
                table: "Turns",
                keyColumn: "Id",
                keyValue: 1,
                column: "Started",
                value: new DateTime(2021, 5, 5, 15, 41, 13, 726, DateTimeKind.Utc).AddTicks(6737));
        }
    }
}
