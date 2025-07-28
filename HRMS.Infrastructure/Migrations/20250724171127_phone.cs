using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class phone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkPhone_Number",
                table: "Employees",
                newName: "WorkPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "WorkPhone_CountryCode",
                table: "Employees",
                newName: "WorkPhoneCountryCode");

            migrationBuilder.RenameColumn(
                name: "PersonalPhone_Number",
                table: "Employees",
                newName: "PersonalPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "PersonalPhone_CountryCode",
                table: "Employees",
                newName: "PersonalPhoneCountryCode");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "time_entries",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "time_entries",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "WorkPhoneNumber",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "WorkPhoneCountryCode",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<string>(
                name: "PersonalPhoneNumber",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "PersonalPhoneCountryCode",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkPhoneNumber",
                table: "Employees",
                newName: "WorkPhone_Number");

            migrationBuilder.RenameColumn(
                name: "WorkPhoneCountryCode",
                table: "Employees",
                newName: "WorkPhone_CountryCode");

            migrationBuilder.RenameColumn(
                name: "PersonalPhoneNumber",
                table: "Employees",
                newName: "PersonalPhone_Number");

            migrationBuilder.RenameColumn(
                name: "PersonalPhoneCountryCode",
                table: "Employees",
                newName: "PersonalPhone_CountryCode");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "time_entries",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "time_entries",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "WorkPhone_Number",
                table: "Employees",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "WorkPhone_CountryCode",
                table: "Employees",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PersonalPhone_Number",
                table: "Employees",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PersonalPhone_CountryCode",
                table: "Employees",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
