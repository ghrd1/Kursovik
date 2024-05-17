using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kursovik.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration_V31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Busy",
                table: "Disciplines",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Busy",
                table: "Disciplines");
        }
    }
}
