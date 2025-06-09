using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GameId1",
                table: "Teams",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "isAlive",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isMale",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Manager",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Manager",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "isAlive",
                table: "Manager",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isMale",
                table: "Manager",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Consistency",
                table: "GameProfiles",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GameProfile_LoL_Consistency",
                table: "GameProfiles",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GameProfile_LoL_Potential",
                table: "GameProfiles",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Potential",
                table: "GameProfiles",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Coaches",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Coaches",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "isAlive",
                table: "Coaches",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isMale",
                table: "Coaches",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_GameId1",
                table: "Teams",
                column: "GameId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Games_GameId1",
                table: "Teams",
                column: "GameId1",
                principalTable: "Games",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Games_GameId1",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_GameId1",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "GameId1",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "isAlive",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "isMale",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "isAlive",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "isMale",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "Consistency",
                table: "GameProfiles");

            migrationBuilder.DropColumn(
                name: "GameProfile_LoL_Consistency",
                table: "GameProfiles");

            migrationBuilder.DropColumn(
                name: "GameProfile_LoL_Potential",
                table: "GameProfiles");

            migrationBuilder.DropColumn(
                name: "Potential",
                table: "GameProfiles");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "isAlive",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "isMale",
                table: "Coaches");
        }
    }
}
