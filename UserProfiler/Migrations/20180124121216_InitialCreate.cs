using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace UserProfiler.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnonymousUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AppVersion = table.Column<string>(nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(nullable: false),
                    UserAgent = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnonymousUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentPages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserActivities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AnonymousUserId = table.Column<Guid>(nullable: false),
                    ContentPageId = table.Column<int>(nullable: false),
                    Coordinates = table.Column<string>(nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    Kind = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserActivities_AnonymousUsers_AnonymousUserId",
                        column: x => x.AnonymousUserId,
                        principalTable: "AnonymousUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserActivities_ContentPages_ContentPageId",
                        column: x => x.ContentPageId,
                        principalTable: "ContentPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_AnonymousUserId",
                table: "UserActivities",
                column: "AnonymousUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_ContentPageId",
                table: "UserActivities",
                column: "ContentPageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserActivities");

            migrationBuilder.DropTable(
                name: "AnonymousUsers");

            migrationBuilder.DropTable(
                name: "ContentPages");
        }
    }
}
