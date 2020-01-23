using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Database.Migrations
{
    public partial class InitialThing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(maxLength: 32, nullable: false),
                    Email = table.Column<string>(maxLength: 254, nullable: true),
                    EncodedHash = table.Column<string>(nullable: false),
                    TimeOfCreation = table.Column<DateTime>(nullable: false),
                    SessionId = table.Column<Guid>(nullable: false),
                    SessionIdExpiryDate = table.Column<DateTime>(nullable: false),
                    PreferedLanguage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerAccountId = table.Column<long>(nullable: true),
                    Authority = table.Column<byte>(nullable: false),
                    LastTimePlayed = table.Column<DateTime>(nullable: false),
                    Nickname = table.Column<string>(maxLength: 24, nullable: false),
                    MapId = table.Column<long>(nullable: false),
                    MapX = table.Column<float>(nullable: false),
                    MapY = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_OwnerAccountId",
                table: "Characters",
                column: "OwnerAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
