using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarMileageLog.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDriveStatusToDriveLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "drive_logs",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "drive_logs");
        }
    }
}
