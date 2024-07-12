using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Data.Migrations
{
    public partial class change12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dailyScrums_scrums_ScrumID",
                table: "dailyScrums");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dailyScrums",
                table: "dailyScrums");

            migrationBuilder.RenameTable(
                name: "dailyScrums",
                newName: "dailyScrumsTable");

            migrationBuilder.RenameIndex(
                name: "IX_dailyScrums_ScrumID",
                table: "dailyScrumsTable",
                newName: "IX_dailyScrumsTable_ScrumID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dailyScrumsTable",
                table: "dailyScrumsTable",
                column: "dailyScrumID");

            migrationBuilder.AddForeignKey(
                name: "FK_dailyScrumsTable_scrums_ScrumID",
                table: "dailyScrumsTable",
                column: "ScrumID",
                principalTable: "scrums",
                principalColumn: "scrumID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dailyScrumsTable_scrums_ScrumID",
                table: "dailyScrumsTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dailyScrumsTable",
                table: "dailyScrumsTable");

            migrationBuilder.RenameTable(
                name: "dailyScrumsTable",
                newName: "dailyScrums");

            migrationBuilder.RenameIndex(
                name: "IX_dailyScrumsTable_ScrumID",
                table: "dailyScrums",
                newName: "IX_dailyScrums_ScrumID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dailyScrums",
                table: "dailyScrums",
                column: "dailyScrumID");

            migrationBuilder.AddForeignKey(
                name: "FK_dailyScrums_scrums_ScrumID",
                table: "dailyScrums",
                column: "ScrumID",
                principalTable: "scrums",
                principalColumn: "scrumID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
