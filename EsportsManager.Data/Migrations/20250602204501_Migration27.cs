using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContractExpiration",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<int>(
                name: "TeamAScore",
                table: "Matches",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamBScore",
                table: "Matches",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContractExpiration",
                table: "Manager",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "ContractExpiration",
                table: "Coaches",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractExpiration",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TeamAScore",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TeamBScore",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "ContractExpiration",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "ContractExpiration",
                table: "Coaches");
        }
    }
}
