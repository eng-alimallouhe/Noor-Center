using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateEmployeeDepcolimns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeDepartments_Departments_DepartmentId1",
                table: "EmployeeDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeDepartments_Employees_EmployeeUserId",
                table: "EmployeeDepartments");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeDepartments_DepartmentId1",
                table: "EmployeeDepartments");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeDepartments_EmployeeUserId",
                table: "EmployeeDepartments");

            migrationBuilder.DropColumn(
                name: "DepartmentId1",
                table: "EmployeeDepartments");

            migrationBuilder.DropColumn(
                name: "EmployeeUserId",
                table: "EmployeeDepartments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId1",
                table: "EmployeeDepartments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeUserId",
                table: "EmployeeDepartments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDepartments_DepartmentId1",
                table: "EmployeeDepartments",
                column: "DepartmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDepartments_EmployeeUserId",
                table: "EmployeeDepartments",
                column: "EmployeeUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeDepartments_Departments_DepartmentId1",
                table: "EmployeeDepartments",
                column: "DepartmentId1",
                principalTable: "Departments",
                principalColumn: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeDepartments_Employees_EmployeeUserId",
                table: "EmployeeDepartments",
                column: "EmployeeUserId",
                principalTable: "Employees",
                principalColumn: "UserId");
        }
    }
}
