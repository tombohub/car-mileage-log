using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CarMileageLog.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateDriveLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "drive_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_kilometers = table.Column<int>(type: "integer", nullable: false),
                    end_kilometers = table.Column<int>(type: "integer", nullable: false),
                    job_site_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drive_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_drive_logs_job_sites_job_site_id",
                        column: x => x.job_site_id,
                        principalTable: "job_sites",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_drive_logs_job_site_id",
                table: "drive_logs",
                column: "job_site_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "drive_logs");
        }
    }
}
