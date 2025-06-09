using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Organizations_UserOrgId",
                table: "GameStates");

            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Teams_UserTeamId",
                table: "GameStates");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserTeamId",
                table: "GameStates",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserOrgId",
                table: "GameStates",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

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

            migrationBuilder.AlterColumn<Guid>(
                name: "UserTeamId",
                table: "GameStates",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserOrgId",
                table: "GameStates",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Organizations_UserOrgId",
                table: "GameStates",
                column: "UserOrgId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Teams_UserTeamId",
                table: "GameStates",
                column: "UserTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
