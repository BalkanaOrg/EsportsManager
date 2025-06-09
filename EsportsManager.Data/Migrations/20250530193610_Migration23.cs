using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ExpectedSalary",
                table: "Players",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ExpectedSalary",
                table: "Manager",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ExpectedSalary",
                table: "Coaches",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedSalary",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "ExpectedSalary",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "ExpectedSalary",
                table: "Coaches");
        }
    }
}
