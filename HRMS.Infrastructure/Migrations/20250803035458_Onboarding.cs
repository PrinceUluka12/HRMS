using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Onboarding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnboardingTaskDocuments");

            migrationBuilder.DropIndex(
                name: "IX_OnboardingTasks_DueDate",
                table: "OnboardingTasks");

            migrationBuilder.DropIndex(
                name: "IX_OnboardingTasks_Status",
                table: "OnboardingTasks");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "OnboardingTasks");

            migrationBuilder.DropColumn(
                name: "CompletedBy",
                table: "OnboardingTasks");

            migrationBuilder.DropColumn(
                name: "CompletionNotes",
                table: "OnboardingTasks");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "OnboardingTasks");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "OnboardingTasks");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "OnboardingTasks");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "OnboardingTasks");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "OnboardingTasks");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "OnboardingTasks",
                newName: "TaskName");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "OnboardingTasks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "OnboardingTasks",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "OnboardingTasks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");

            migrationBuilder.AddColumn<string>(
                name: "AssignedTo",
                table: "OnboardingTasks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "OnboardingTasks",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StageId",
                table: "OnboardingTasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Onboardings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OverallProgress = table.Column<int>(type: "int", nullable: false),
                    DaysRemaining = table.Column<int>(type: "int", nullable: false),
                    LastActivity = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Onboardings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OnboardingDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OnboardingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardingDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnboardingDocuments_Onboardings_OnboardingId",
                        column: x => x.OnboardingId,
                        principalTable: "Onboardings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnboardingStages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Progress = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OnboardingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardingStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnboardingStages_Onboardings_OnboardingId",
                        column: x => x.OnboardingId,
                        principalTable: "Onboardings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingTasks_StageId",
                table: "OnboardingTasks",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingDocuments_OnboardingId",
                table: "OnboardingDocuments",
                column: "OnboardingId");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingStages_OnboardingId",
                table: "OnboardingStages",
                column: "OnboardingId");

            migrationBuilder.AddForeignKey(
                name: "FK_OnboardingTasks_OnboardingStages_StageId",
                table: "OnboardingTasks",
                column: "StageId",
                principalTable: "OnboardingStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnboardingTasks_OnboardingStages_StageId",
                table: "OnboardingTasks");

            migrationBuilder.DropTable(
                name: "OnboardingDocuments");

            migrationBuilder.DropTable(
                name: "OnboardingStages");

            migrationBuilder.DropTable(
                name: "Onboardings");

            migrationBuilder.DropIndex(
                name: "IX_OnboardingTasks_StageId",
                table: "OnboardingTasks");

            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "OnboardingTasks");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "OnboardingTasks");

            migrationBuilder.DropColumn(
                name: "StageId",
                table: "OnboardingTasks");

            migrationBuilder.RenameColumn(
                name: "TaskName",
                table: "OnboardingTasks",
                newName: "CreatedBy");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "OnboardingTasks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "OnboardingTasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "OnboardingTasks",
                type: "nvarchar(1000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "OnboardingTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompletedBy",
                table: "OnboardingTasks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompletionNotes",
                table: "OnboardingTasks",
                type: "nvarchar(2000)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "OnboardingTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "OnboardingTasks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "OnboardingTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "OnboardingTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "OnboardingTasks",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OnboardingTaskDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OnboardingTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardingTaskDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnboardingTaskDocuments_OnboardingTasks_OnboardingTaskId",
                        column: x => x.OnboardingTaskId,
                        principalTable: "OnboardingTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingTasks_DueDate",
                table: "OnboardingTasks",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingTasks_Status",
                table: "OnboardingTasks",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingTaskDocuments_OnboardingTaskId",
                table: "OnboardingTaskDocuments",
                column: "OnboardingTaskId");
        }
    }
}
