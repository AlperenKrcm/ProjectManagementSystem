using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Data.Migrations
{
    public partial class ModelUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    projectID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    projectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    projectDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    startTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    projectDeadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    client = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.projectID);
                });

            migrationBuilder.CreateTable(
                name: "projectTeams",
                columns: table => new
                {
                    projectTeamID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectRole = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projectTeams", x => x.projectTeamID);
                    table.ForeignKey(
                        name: "FK_projectTeams_projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "projects",
                        principalColumn: "projectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scrums",
                columns: table => new
                {
                    scrumID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    scrumMaster = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scrums", x => x.scrumID);
                    table.ForeignKey(
                        name: "FK_scrums_projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "projects",
                        principalColumn: "projectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tasksForUser",
                columns: table => new
                {
                    taskForUserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    userID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    taskDeadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasksForUser", x => x.taskForUserID);
                    table.ForeignKey(
                        name: "FK_tasksForUser_projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "projects",
                        principalColumn: "projectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dailyScrums",
                columns: table => new
                {
                    dailyScrumID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    scrumID = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dailyScrums", x => x.dailyScrumID);
                    table.ForeignKey(
                        name: "FK_dailyScrums_scrums_scrumID",
                        column: x => x.scrumID,
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
                    sprintNumber = table.Column<int>(type: "int", nullable: false),
                    ScrumID = table.Column<int>(type: "int", nullable: false),
                    springTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sprints", x => x.sprintID);
                    table.ForeignKey(
                        name: "FK_sprints_scrums_ScrumID",
                        column: x => x.ScrumID,
                        principalTable: "scrums",
                        principalColumn: "scrumID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "supports",
                columns: table => new
                {
                    supportID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    taskID = table.Column<int>(type: "int", nullable: false),
                    tasksForUsertaskForUserID = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    helpDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    helperID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supports", x => x.supportID);
                    table.ForeignKey(
                        name: "FK_supports_tasksForUser_tasksForUsertaskForUserID",
                        column: x => x.tasksForUsertaskForUserID,
                        principalTable: "tasksForUser",
                        principalColumn: "taskForUserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dailyScrums_scrumID",
                table: "dailyScrums",
                column: "scrumID");

            migrationBuilder.CreateIndex(
                name: "IX_projectTeams_ProjectID",
                table: "projectTeams",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_scrums_ProjectID",
                table: "scrums",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_sprints_ScrumID",
                table: "sprints",
                column: "ScrumID");

            migrationBuilder.CreateIndex(
                name: "IX_supports_tasksForUsertaskForUserID",
                table: "supports",
                column: "tasksForUsertaskForUserID");

            migrationBuilder.CreateIndex(
                name: "IX_tasksForUser_ProjectID",
                table: "tasksForUser",
                column: "ProjectID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dailyScrums");

            migrationBuilder.DropTable(
                name: "projectTeams");

            migrationBuilder.DropTable(
                name: "sprints");

            migrationBuilder.DropTable(
                name: "supports");

            migrationBuilder.DropTable(
                name: "scrums");

            migrationBuilder.DropTable(
                name: "tasksForUser");

            migrationBuilder.DropTable(
                name: "projects");
        }
    }
}
