using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Data.Migrations
{
    public partial class ModelUpdateFKSupport1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_supports_tasksForUser_tasksForUsertaskForUserID",
                table: "supports");

            migrationBuilder.DropIndex(
                name: "IX_supports_tasksForUsertaskForUserID",
                table: "supports");

            migrationBuilder.DropColumn(
                name: "tasksForUsertaskForUserID",
                table: "supports");

            migrationBuilder.CreateIndex(
                name: "IX_supports_taskForUserID",
                table: "supports",
                column: "taskForUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_supports_tasksForUser_taskForUserID",
                table: "supports",
                column: "taskForUserID",
                principalTable: "tasksForUser",
                principalColumn: "taskForUserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_supports_tasksForUser_taskForUserID",
                table: "supports");

            migrationBuilder.DropIndex(
                name: "IX_supports_taskForUserID",
                table: "supports");

            migrationBuilder.AddColumn<int>(
                name: "tasksForUsertaskForUserID",
                table: "supports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_supports_tasksForUsertaskForUserID",
                table: "supports",
                column: "tasksForUsertaskForUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_supports_tasksForUser_tasksForUsertaskForUserID",
                table: "supports",
                column: "tasksForUsertaskForUserID",
                principalTable: "tasksForUser",
                principalColumn: "taskForUserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
