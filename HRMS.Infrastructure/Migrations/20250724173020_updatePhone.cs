using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalPhoneCountryCode",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalPhoneNumber",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "WorkPhoneCountryCode",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "WorkPhoneNumber",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PhoneNumber_CountryCode",
                table: "EmployeeEmergencyContacts");

            migrationBuilder.DropColumn(
                name: "PhoneNumber_Number",
                table: "EmployeeEmergencyContacts");

            migrationBuilder.AddColumn<string>(
                name: "PersonalPhone",
                table: "Employees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkPhone",
                table: "Employees",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "EmployeeEmergencyContacts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalPhone",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "WorkPhone",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "EmployeeEmergencyContacts");

            migrationBuilder.AddColumn<string>(
                name: "PersonalPhoneCountryCode",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PersonalPhoneNumber",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkPhoneCountryCode",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkPhoneNumber",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber_CountryCode",
                table: "EmployeeEmergencyContacts",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber_Number",
                table: "EmployeeEmergencyContacts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
