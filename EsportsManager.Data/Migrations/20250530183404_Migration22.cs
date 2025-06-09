using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "MarketValue",
                table: "Players",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "WeeklyHappiness",
                table: "Organizations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeeklyMotivation",
                table: "Organizations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeeklyPrestiegeDecay",
                table: "Organizations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Manager",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "MarketValue",
                table: "Manager",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Coaches",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "MarketValue",
                table: "Coaches",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "MarketValue",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "WeeklyHappiness",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "WeeklyMotivation",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "WeeklyPrestiegeDecay",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "MarketValue",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "MarketValue",
                table: "Coaches");
        }
    }
}
