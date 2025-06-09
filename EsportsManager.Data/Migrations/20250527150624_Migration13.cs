using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Teams_UserTeamId1",
                table: "GameStates");

            migrationBuilder.RenameColumn(
                name: "UserTeamId1",
                table: "GameStates",
                newName: "UserOrgId");

            migrationBuilder.RenameIndex(
                name: "IX_GameStates_UserTeamId1",
                table: "GameStates",
                newName: "IX_GameStates_UserOrgId");

            migrationBuilder.AddColumn<int>(
                name: "FoundingYear",
                table: "Teams",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TeamId1",
                table: "Players",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isBenched",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId1",
                table: "Players",
                column: "TeamId1");

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_UserTeamId",
                table: "GameStates",
                column: "UserTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Organizations_UserOrgId",
                table: "GameStates",
                column: "UserOrgId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Teams_UserTeamId",
                table: "GameStates",
                column: "UserTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Teams_TeamId1",
                table: "Players",
                column: "TeamId1",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Organizations_UserOrgId",
                table: "GameStates");

            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Teams_UserTeamId",
                table: "GameStates");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Teams_TeamId1",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_TeamId1",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_GameStates_UserTeamId",
                table: "GameStates");

            migrationBuilder.DropColumn(
                name: "FoundingYear",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamId1",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "isBenched",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "UserOrgId",
                table: "GameStates",
                newName: "UserTeamId1");

            migrationBuilder.RenameIndex(
                name: "IX_GameStates_UserOrgId",
                table: "GameStates",
                newName: "IX_GameStates_UserTeamId1");

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Teams_UserTeamId1",
                table: "GameStates",
                column: "UserTeamId1",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
