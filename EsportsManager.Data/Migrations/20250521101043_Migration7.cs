using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coaches_GameState_GameStateId",
                table: "Coaches");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameState_GameStateId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_GameState_Teams_UserTeamId1",
                table: "GameState");

            migrationBuilder.DropForeignKey(
                name: "FK_Manager_GameState_GameStateId",
                table: "Manager");

            migrationBuilder.DropForeignKey(
                name: "FK_Manager_Organization_OrganizationId",
                table: "Manager");

            migrationBuilder.DropForeignKey(
                name: "FK_Map_GameState_GameStateId",
                table: "Map");

            migrationBuilder.DropForeignKey(
                name: "FK_Map_Games_GameId",
                table: "Map");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Map_MapId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Series_SeriesId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Teams_WinnerTeamId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Organization_GameState_GameStateId",
                table: "Organization");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_GameState_GameStateId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_CS_Match_MatchId",
                table: "PlayerStats_CS");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_CS_Players_PlayerId",
                table: "PlayerStats_CS");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_CS_Teams_TeamId",
                table: "PlayerStats_CS");

            migrationBuilder.DropForeignKey(
                name: "FK_Series_Tournament_TournamentId",
                table: "Series");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_GameState_GameStateId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Organization_OrganizationId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournament_GameState_GameStateId",
                table: "Tournament");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournament_Organization_OrganizationId",
                table: "Tournament");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournament_Teams_WinnerId",
                table: "Tournament");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tournament",
                table: "Tournament");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organization",
                table: "Organization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Match",
                table: "Match");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Map",
                table: "Map");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameState",
                table: "GameState");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerStats_CS",
                table: "PlayerStats_CS");

            migrationBuilder.RenameTable(
                name: "Tournament",
                newName: "Tournaments");

            migrationBuilder.RenameTable(
                name: "Organization",
                newName: "Organizations");

            migrationBuilder.RenameTable(
                name: "Match",
                newName: "Matches");

            migrationBuilder.RenameTable(
                name: "Map",
                newName: "Maps");

            migrationBuilder.RenameTable(
                name: "GameState",
                newName: "GameStates");

            migrationBuilder.RenameTable(
                name: "PlayerStats_CS",
                newName: "PlayerStats");

            migrationBuilder.RenameIndex(
                name: "IX_Tournament_WinnerId",
                table: "Tournaments",
                newName: "IX_Tournaments_WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Tournament_OrganizationId",
                table: "Tournaments",
                newName: "IX_Tournaments_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Tournament_GameStateId",
                table: "Tournaments",
                newName: "IX_Tournaments_GameStateId");

            migrationBuilder.RenameIndex(
                name: "IX_Organization_GameStateId",
                table: "Organizations",
                newName: "IX_Organizations_GameStateId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_WinnerTeamId",
                table: "Matches",
                newName: "IX_Matches_WinnerTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_SeriesId",
                table: "Matches",
                newName: "IX_Matches_SeriesId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_MapId",
                table: "Matches",
                newName: "IX_Matches_MapId");

            migrationBuilder.RenameIndex(
                name: "IX_Map_GameStateId",
                table: "Maps",
                newName: "IX_Maps_GameStateId");

            migrationBuilder.RenameIndex(
                name: "IX_Map_GameId",
                table: "Maps",
                newName: "IX_Maps_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameState_UserTeamId1",
                table: "GameStates",
                newName: "IX_GameStates_UserTeamId1");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerStats_CS_TeamId",
                table: "PlayerStats",
                newName: "IX_PlayerStats_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerStats_CS_PlayerId",
                table: "PlayerStats",
                newName: "IX_PlayerStats_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerStats_CS_MatchId",
                table: "PlayerStats",
                newName: "IX_PlayerStats_MatchId");

            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                table: "Tournaments",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "Budget",
                table: "Organizations",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Prestige",
                table: "Organizations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "WeeklyExpense",
                table: "Organizations",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "RoundsWithKillsOrAssists",
                table: "PlayerStats",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "RoundsTraded",
                table: "PlayerStats",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "RoundsSurvived",
                table: "PlayerStats",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "RoundsPlayed",
                table: "PlayerStats",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "MatchId",
                table: "PlayerStats",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Kills",
                table: "PlayerStats",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "HeadShotKills",
                table: "PlayerStats",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "Deaths",
                table: "PlayerStats",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "DamageDealt",
                table: "PlayerStats",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "Assists",
                table: "PlayerStats",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "PlayerStats",
                type: "TEXT",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tournaments",
                table: "Tournaments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matches",
                table: "Matches",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Maps",
                table: "Maps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameStates",
                table: "GameStates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerStats",
                table: "PlayerStats",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TeamTournament",
                columns: table => new
                {
                    ParticipatingTeamsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TournamentsParticipatedId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamTournament", x => new { x.ParticipatingTeamsId, x.TournamentsParticipatedId });
                    table.ForeignKey(
                        name: "FK_TeamTournament_Teams_ParticipatingTeamsId",
                        column: x => x.ParticipatingTeamsId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamTournament_Tournaments_TournamentsParticipatedId",
                        column: x => x.TournamentsParticipatedId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_GameId",
                table: "Tournaments",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTournament_TournamentsParticipatedId",
                table: "TeamTournament",
                column: "TournamentsParticipatedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coaches_GameStates_GameStateId",
                table: "Coaches",
                column: "GameStateId",
                principalTable: "GameStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameStates_GameStateId",
                table: "Games",
                column: "GameStateId",
                principalTable: "GameStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Teams_UserTeamId1",
                table: "GameStates",
                column: "UserTeamId1",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Manager_GameStates_GameStateId",
                table: "Manager",
                column: "GameStateId",
                principalTable: "GameStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Manager_Organizations_OrganizationId",
                table: "Manager",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_GameStates_GameStateId",
                table: "Maps",
                column: "GameStateId",
                principalTable: "GameStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_Games_GameId",
                table: "Maps",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Maps_MapId",
                table: "Matches",
                column: "MapId",
                principalTable: "Maps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Series_SeriesId",
                table: "Matches",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Teams_WinnerTeamId",
                table: "Matches",
                column: "WinnerTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_GameStates_GameStateId",
                table: "Organizations",
                column: "GameStateId",
                principalTable: "GameStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_GameStates_GameStateId",
                table: "Players",
                column: "GameStateId",
                principalTable: "GameStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_Matches_MatchId",
                table: "PlayerStats",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_Players_PlayerId",
                table: "PlayerStats",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_Teams_TeamId",
                table: "PlayerStats",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Series_Tournaments_TournamentId",
                table: "Series",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_GameStates_GameStateId",
                table: "Teams",
                column: "GameStateId",
                principalTable: "GameStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Organizations_OrganizationId",
                table: "Teams",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_GameStates_GameStateId",
                table: "Tournaments",
                column: "GameStateId",
                principalTable: "GameStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Games_GameId",
                table: "Tournaments",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Organizations_OrganizationId",
                table: "Tournaments",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Teams_WinnerId",
                table: "Tournaments",
                column: "WinnerId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coaches_GameStates_GameStateId",
                table: "Coaches");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameStates_GameStateId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Teams_UserTeamId1",
                table: "GameStates");

            migrationBuilder.DropForeignKey(
                name: "FK_Manager_GameStates_GameStateId",
                table: "Manager");

            migrationBuilder.DropForeignKey(
                name: "FK_Manager_Organizations_OrganizationId",
                table: "Manager");

            migrationBuilder.DropForeignKey(
                name: "FK_Maps_GameStates_GameStateId",
                table: "Maps");

            migrationBuilder.DropForeignKey(
                name: "FK_Maps_Games_GameId",
                table: "Maps");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Maps_MapId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Series_SeriesId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Teams_WinnerTeamId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_GameStates_GameStateId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_GameStates_GameStateId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_Matches_MatchId",
                table: "PlayerStats");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_Players_PlayerId",
                table: "PlayerStats");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_Teams_TeamId",
                table: "PlayerStats");

            migrationBuilder.DropForeignKey(
                name: "FK_Series_Tournaments_TournamentId",
                table: "Series");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_GameStates_GameStateId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Organizations_OrganizationId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_GameStates_GameStateId",
                table: "Tournaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Games_GameId",
                table: "Tournaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Organizations_OrganizationId",
                table: "Tournaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Teams_WinnerId",
                table: "Tournaments");

            migrationBuilder.DropTable(
                name: "TeamTournament");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tournaments",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_GameId",
                table: "Tournaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matches",
                table: "Matches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Maps",
                table: "Maps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameStates",
                table: "GameStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerStats",
                table: "PlayerStats");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Budget",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Prestige",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "WeeklyExpense",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "PlayerStats");

            migrationBuilder.RenameTable(
                name: "Tournaments",
                newName: "Tournament");

            migrationBuilder.RenameTable(
                name: "Organizations",
                newName: "Organization");

            migrationBuilder.RenameTable(
                name: "Matches",
                newName: "Match");

            migrationBuilder.RenameTable(
                name: "Maps",
                newName: "Map");

            migrationBuilder.RenameTable(
                name: "GameStates",
                newName: "GameState");

            migrationBuilder.RenameTable(
                name: "PlayerStats",
                newName: "PlayerStats_CS");

            migrationBuilder.RenameIndex(
                name: "IX_Tournaments_WinnerId",
                table: "Tournament",
                newName: "IX_Tournament_WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Tournaments_OrganizationId",
                table: "Tournament",
                newName: "IX_Tournament_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Tournaments_GameStateId",
                table: "Tournament",
                newName: "IX_Tournament_GameStateId");

            migrationBuilder.RenameIndex(
                name: "IX_Organizations_GameStateId",
                table: "Organization",
                newName: "IX_Organization_GameStateId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_WinnerTeamId",
                table: "Match",
                newName: "IX_Match_WinnerTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_SeriesId",
                table: "Match",
                newName: "IX_Match_SeriesId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_MapId",
                table: "Match",
                newName: "IX_Match_MapId");

            migrationBuilder.RenameIndex(
                name: "IX_Maps_GameStateId",
                table: "Map",
                newName: "IX_Map_GameStateId");

            migrationBuilder.RenameIndex(
                name: "IX_Maps_GameId",
                table: "Map",
                newName: "IX_Map_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameStates_UserTeamId1",
                table: "GameState",
                newName: "IX_GameState_UserTeamId1");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerStats_TeamId",
                table: "PlayerStats_CS",
                newName: "IX_PlayerStats_CS_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerStats_PlayerId",
                table: "PlayerStats_CS",
                newName: "IX_PlayerStats_CS_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerStats_MatchId",
                table: "PlayerStats_CS",
                newName: "IX_PlayerStats_CS_MatchId");

            migrationBuilder.AlterColumn<int>(
                name: "RoundsWithKillsOrAssists",
                table: "PlayerStats_CS",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoundsTraded",
                table: "PlayerStats_CS",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoundsSurvived",
                table: "PlayerStats_CS",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoundsPlayed",
                table: "PlayerStats_CS",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MatchId",
                table: "PlayerStats_CS",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Kills",
                table: "PlayerStats_CS",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HeadShotKills",
                table: "PlayerStats_CS",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Deaths",
                table: "PlayerStats_CS",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DamageDealt",
                table: "PlayerStats_CS",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Assists",
                table: "PlayerStats_CS",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tournament",
                table: "Tournament",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organization",
                table: "Organization",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Match",
                table: "Match",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Map",
                table: "Map",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameState",
                table: "GameState",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerStats_CS",
                table: "PlayerStats_CS",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Coaches_GameState_GameStateId",
                table: "Coaches",
                column: "GameStateId",
                principalTable: "GameState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameState_GameStateId",
                table: "Games",
                column: "GameStateId",
                principalTable: "GameState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameState_Teams_UserTeamId1",
                table: "GameState",
                column: "UserTeamId1",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Manager_GameState_GameStateId",
                table: "Manager",
                column: "GameStateId",
                principalTable: "GameState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Manager_Organization_OrganizationId",
                table: "Manager",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Map_GameState_GameStateId",
                table: "Map",
                column: "GameStateId",
                principalTable: "GameState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Map_Games_GameId",
                table: "Map",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Map_MapId",
                table: "Match",
                column: "MapId",
                principalTable: "Map",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Series_SeriesId",
                table: "Match",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Teams_WinnerTeamId",
                table: "Match",
                column: "WinnerTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organization_GameState_GameStateId",
                table: "Organization",
                column: "GameStateId",
                principalTable: "GameState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_GameState_GameStateId",
                table: "Players",
                column: "GameStateId",
                principalTable: "GameState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_CS_Match_MatchId",
                table: "PlayerStats_CS",
                column: "MatchId",
                principalTable: "Match",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_CS_Players_PlayerId",
                table: "PlayerStats_CS",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_CS_Teams_TeamId",
                table: "PlayerStats_CS",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Series_Tournament_TournamentId",
                table: "Series",
                column: "TournamentId",
                principalTable: "Tournament",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_GameState_GameStateId",
                table: "Teams",
                column: "GameStateId",
                principalTable: "GameState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Organization_OrganizationId",
                table: "Teams",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournament_GameState_GameStateId",
                table: "Tournament",
                column: "GameStateId",
                principalTable: "GameState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournament_Organization_OrganizationId",
                table: "Tournament",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournament_Teams_WinnerId",
                table: "Tournament",
                column: "WinnerId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
