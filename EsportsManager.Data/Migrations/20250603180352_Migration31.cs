using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isUserControlled",
                table: "Teams",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isUserControlled",
                table: "Organizations",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isUserControlled",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "isUserControlled",
                table: "Organizations");
        }
    }
}
