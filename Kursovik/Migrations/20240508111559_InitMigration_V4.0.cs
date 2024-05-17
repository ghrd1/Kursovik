using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kursovik.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration_V40 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position_Id",
                table: "Disciplines",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Disciplines_Position_Id",
                table: "Disciplines",
                column: "Position_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Disciplines_Positions_Position_Id",
                table: "Disciplines",
                column: "Position_Id",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disciplines_Positions_Position_Id",
                table: "Disciplines");

            migrationBuilder.DropIndex(
                name: "IX_Disciplines_Position_Id",
                table: "Disciplines");

            migrationBuilder.DropColumn(
                name: "Position_Id",
                table: "Disciplines");
        }
    }
}
