using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameProfiles_Teams_TeamId",
                table: "GameProfiles");

            migrationBuilder.DropIndex(
                name: "IX_GameProfiles_TeamId",
                table: "GameProfiles");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "GameProfiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "GameProfiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameProfiles_TeamId",
                table: "GameProfiles",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameProfiles_Teams_TeamId",
                table: "GameProfiles",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
