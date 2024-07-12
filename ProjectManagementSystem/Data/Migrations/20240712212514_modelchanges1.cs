using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Data.Migrations
{
    public partial class modelchanges1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dailyScrums");

            migrationBuilder.DropTable(
                name: "sprints");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dailyScrums",
                columns: table => new
                {
                    dailyScrumID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScrumID = table.Column<int>(type: "int", nullable: false),
                    dailyScrumNumber = table.Column<int>(type: "int", nullable: false),
                    dailyScrumTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dailyScrums", x => x.dailyScrumID);
                    table.ForeignKey(
                        name: "FK_dailyScrums_scrums_ScrumID",
                        column: x => x.ScrumID,
                        principalTable: "scrums",
                        principalColumn: "scrumID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sprints",
                columns: table => new
                {
                    sprintID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    scrumID = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sprints", x => x.sprintID);
                    table.ForeignKey(
                        name: "FK_sprints_scrums_scrumID",
                        column: x => x.scrumID,
                        principalTable: "scrums",
                        principalColumn: "scrumID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dailyScrums_ScrumID",
                table: "dailyScrums",
                column: "ScrumID");

            migrationBuilder.CreateIndex(
                name: "IX_sprints_scrumID",
                table: "sprints",
                column: "scrumID");
        }
    }
}
