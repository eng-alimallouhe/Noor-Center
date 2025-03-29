using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatetablesrealtionships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OtpCodes_UserId",
                table: "OtpCodes");

            migrationBuilder.RenameColumn(
                name: "OtpCodeValue",
                table: "OtpCodes",
                newName: "HashedValue");

            migrationBuilder.CreateIndex(
                name: "IX_OtpCodes_UserId",
                table: "OtpCodes",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OtpCodes_UserId",
                table: "OtpCodes");

            migrationBuilder.RenameColumn(
                name: "HashedValue",
                table: "OtpCodes",
                newName: "OtpCodeValue");

            migrationBuilder.CreateIndex(
                name: "IX_OtpCodes_UserId",
                table: "OtpCodes",
                column: "UserId");
        }
    }
}
