using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "GameStates",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "GameStates",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_OrganizationId",
                table: "GameStates",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_TeamId",
                table: "GameStates",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Organizations_OrganizationId",
                table: "GameStates",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Teams_TeamId",
                table: "GameStates",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Organizations_OrganizationId",
                table: "GameStates");

            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Teams_TeamId",
                table: "GameStates");

            migrationBuilder.DropIndex(
                name: "IX_GameStates_OrganizationId",
                table: "GameStates");

            migrationBuilder.DropIndex(
                name: "IX_GameStates_TeamId",
                table: "GameStates");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "GameStates");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "GameStates");
        }
    }
}
