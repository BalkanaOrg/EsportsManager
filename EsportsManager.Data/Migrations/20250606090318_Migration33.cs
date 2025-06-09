using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GameStateId",
                table: "Sponsorships",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Sponsorships_GameStateId",
                table: "Sponsorships",
                column: "GameStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sponsorships_GameStates_GameStateId",
                table: "Sponsorships",
                column: "GameStateId",
                principalTable: "GameStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sponsorships_GameStates_GameStateId",
                table: "Sponsorships");

            migrationBuilder.DropIndex(
                name: "IX_Sponsorships_GameStateId",
                table: "Sponsorships");

            migrationBuilder.DropColumn(
                name: "GameStateId",
                table: "Sponsorships");
        }
    }
}
