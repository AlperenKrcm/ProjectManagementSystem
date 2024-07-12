using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Data.Migrations
{
    public partial class modelchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dailyScrums_scrums_scrumID",
                table: "dailyScrums");

            migrationBuilder.DropForeignKey(
                name: "FK_sprints_scrums_ScrumID",
                table: "sprints");

            migrationBuilder.DropColumn(
                name: "dailyScrumNumber",
                table: "sprints");

            migrationBuilder.DropColumn(
                name: "springTime",
                table: "sprints");

            migrationBuilder.RenameColumn(
                name: "ScrumID",
                table: "sprints",
                newName: "scrumID");

            migrationBuilder.RenameIndex(
                name: "IX_sprints_ScrumID",
                table: "sprints",
                newName: "IX_sprints_scrumID");

            migrationBuilder.RenameColumn(
                name: "scrumID",
                table: "dailyScrums",
                newName: "ScrumID");

            migrationBuilder.RenameIndex(
                name: "IX_dailyScrums_scrumID",
                table: "dailyScrums",
                newName: "IX_dailyScrums_ScrumID");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "sprints",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "dailyScrums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "dailyScrumNumber",
                table: "dailyScrums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "dailyScrumTime",
                table: "dailyScrums",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_dailyScrums_scrums_ScrumID",
                table: "dailyScrums",
                column: "ScrumID",
                principalTable: "scrums",
                principalColumn: "scrumID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sprints_scrums_scrumID",
                table: "sprints",
                column: "scrumID",
                principalTable: "scrums",
                principalColumn: "scrumID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dailyScrums_scrums_ScrumID",
                table: "dailyScrums");

            migrationBuilder.DropForeignKey(
                name: "FK_sprints_scrums_scrumID",
                table: "sprints");

            migrationBuilder.DropColumn(
                name: "dailyScrumNumber",
                table: "dailyScrums");

            migrationBuilder.DropColumn(
                name: "dailyScrumTime",
                table: "dailyScrums");

            migrationBuilder.RenameColumn(
                name: "scrumID",
                table: "sprints",
                newName: "ScrumID");

            migrationBuilder.RenameIndex(
                name: "IX_sprints_scrumID",
                table: "sprints",
                newName: "IX_sprints_ScrumID");

            migrationBuilder.RenameColumn(
                name: "ScrumID",
                table: "dailyScrums",
                newName: "scrumID");

            migrationBuilder.RenameIndex(
                name: "IX_dailyScrums_ScrumID",
                table: "dailyScrums",
                newName: "IX_dailyScrums_scrumID");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "sprints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "dailyScrumNumber",
                table: "sprints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "springTime",
                table: "sprints",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "dailyScrums",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_dailyScrums_scrums_scrumID",
                table: "dailyScrums",
                column: "scrumID",
                principalTable: "scrums",
                principalColumn: "scrumID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sprints_scrums_ScrumID",
                table: "sprints",
                column: "ScrumID",
                principalTable: "scrums",
                principalColumn: "scrumID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
