using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "PrizePool",
                table: "Tournaments",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Tournaments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GroupStageDuration",
                table: "Tournaments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GroupStageFormat",
                table: "Tournaments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentTournamentId",
                table: "Tournaments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayoffsDuration",
                table: "Tournaments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlayoffsFormat",
                table: "Tournaments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Tournaments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Tier",
                table: "Tournaments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Tournaments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isRetired",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Organizations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Maps",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_ParentTournamentId",
                table: "Tournaments",
                column: "ParentTournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Tournaments_ParentTournamentId",
                table: "Tournaments",
                column: "ParentTournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Tournaments_ParentTournamentId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_ParentTournamentId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "GroupStageDuration",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "GroupStageFormat",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "ParentTournamentId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "PlayoffsDuration",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "PlayoffsFormat",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Tier",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "isRetired",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Maps");

            migrationBuilder.AlterColumn<int>(
                name: "PrizePool",
                table: "Tournaments",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "REAL");
        }
    }
}
