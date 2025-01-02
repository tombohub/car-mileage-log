using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarMileageLog.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNameToJobSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "job_sites",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "job_sites");
        }
    }
}
