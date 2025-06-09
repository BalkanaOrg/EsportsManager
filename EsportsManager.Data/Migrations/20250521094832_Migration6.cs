using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "WeeklyExpense",
                table: "Teams",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<double>(
                name: "Budget",
                table: "Teams",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<Guid>(
                name: "GameStateId",
                table: "Teams",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "Teams",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Buyout",
                table: "Players",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "GameStateId",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Happiness",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Money",
                table: "Players",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WeeklySalary",
                table: "Players",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "GameStateId",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "Buyout",
                table: "Coaches",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "GameStateId",
                table: "Coaches",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Happiness",
                table: "Coaches",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Money",
                table: "Coaches",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WeeklySalary",
                table: "Coaches",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "GameState",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CurrentYear = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentWeek = table.Column<int>(type: "INTEGER", nullable: false),
                    UserTeamId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserTeamId1 = table.Column<Guid>(type: "TEXT", nullable: false),
                    Budget = table.Column<decimal>(type: "TEXT", nullable: false),
                    WeeklyExpense = table.Column<decimal>(type: "TEXT", nullable: false),
                    PlayedYears = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameState_Teams_UserTeamId1",
                        column: x => x.UserTeamId1,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Map",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameStateId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Map", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Map_GameState_GameStateId",
                        column: x => x.GameStateId,
                        principalTable: "GameState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Map_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    FoundingYear = table.Column<int>(type: "INTEGER", nullable: false),
                    GameStateId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organization_GameState_GameStateId",
                        column: x => x.GameStateId,
                        principalTable: "GameState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Nickname = table.Column<string>(type: "TEXT", nullable: false),
                    Reputation = table.Column<int>(type: "INTEGER", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    Motivation = table.Column<int>(type: "INTEGER", nullable: false),
                    Happiness = table.Column<int>(type: "INTEGER", nullable: false),
                    Money = table.Column<double>(type: "REAL", nullable: false),
                    WeeklySalary = table.Column<double>(type: "REAL", nullable: false),
                    GameStateId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manager_GameState_GameStateId",
                        column: x => x.GameStateId,
                        principalTable: "GameState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Manager_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Tournament",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    GameStateId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WinnerId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PrizePool = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    StartingWeek = table.Column<int>(type: "INTEGER", nullable: false),
                    DurationWeeks = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tournament_GameState_GameStateId",
                        column: x => x.GameStateId,
                        principalTable: "GameState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tournament_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tournament_Teams_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TournamentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TeamAId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TeamBId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Format = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamAWins = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamBWins = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Series_Teams_TeamAId",
                        column: x => x.TeamAId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Series_Teams_TeamBId",
                        column: x => x.TeamBId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Series_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SeriesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MatchNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    WinnerTeamId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MatchType = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    MapId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    KillsTeamA = table.Column<int>(type: "INTEGER", nullable: true),
                    DeathsTeamA = table.Column<int>(type: "INTEGER", nullable: true),
                    AssistsTeamA = table.Column<int>(type: "INTEGER", nullable: true),
                    TowersDestroyedTeamA = table.Column<int>(type: "INTEGER", nullable: true),
                    DragonsSlayedTeamA = table.Column<int>(type: "INTEGER", nullable: true),
                    BaronsSlayedTeamA = table.Column<int>(type: "INTEGER", nullable: true),
                    KillsTeamB = table.Column<int>(type: "INTEGER", nullable: true),
                    DeathsTeamB = table.Column<int>(type: "INTEGER", nullable: true),
                    AssistsTeamB = table.Column<int>(type: "INTEGER", nullable: true),
                    TowersDestroyedTeamB = table.Column<int>(type: "INTEGER", nullable: true),
                    DragonsSlayedTeamB = table.Column<int>(type: "INTEGER", nullable: true),
                    BaronsSlayedTeamB = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Match_Map_MapId",
                        column: x => x.MapId,
                        principalTable: "Map",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Match_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Match_Teams_WinnerTeamId",
                        column: x => x.WinnerTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerStats_CS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MatchId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Kills = table.Column<int>(type: "INTEGER", nullable: false),
                    Deaths = table.Column<int>(type: "INTEGER", nullable: false),
                    Assists = table.Column<int>(type: "INTEGER", nullable: false),
                    DamageDealt = table.Column<int>(type: "INTEGER", nullable: false),
                    HeadShotKills = table.Column<int>(type: "INTEGER", nullable: false),
                    RoundsPlayed = table.Column<int>(type: "INTEGER", nullable: false),
                    RoundsWithKillsOrAssists = table.Column<int>(type: "INTEGER", nullable: false),
                    RoundsTraded = table.Column<int>(type: "INTEGER", nullable: false),
                    RoundsSurvived = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TeamId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerStats_CS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerStats_CS_Match_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Match",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerStats_CS_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerStats_CS_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_GameStateId",
                table: "Teams",
                column: "GameStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_OrganizationId",
                table: "Teams",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_GameStateId",
                table: "Players",
                column: "GameStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameStateId",
                table: "Games",
                column: "GameStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_GameStateId",
                table: "Coaches",
                column: "GameStateId");

            migrationBuilder.CreateIndex(
                name: "IX_GameState_UserTeamId1",
                table: "GameState",
                column: "UserTeamId1");

            migrationBuilder.CreateIndex(
                name: "IX_Manager_GameStateId",
                table: "Manager",
                column: "GameStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Manager_OrganizationId",
                table: "Manager",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Map_GameId",
                table: "Map",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Map_GameStateId",
                table: "Map",
                column: "GameStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_MapId",
                table: "Match",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_SeriesId",
                table: "Match",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_WinnerTeamId",
                table: "Match",
                column: "WinnerTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_GameStateId",
                table: "Organization",
                column: "GameStateId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_CS_MatchId",
                table: "PlayerStats_CS",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_CS_PlayerId",
                table: "PlayerStats_CS",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_CS_TeamId",
                table: "PlayerStats_CS",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Series_TeamAId",
                table: "Series",
                column: "TeamAId");

            migrationBuilder.CreateIndex(
                name: "IX_Series_TeamBId",
                table: "Series",
                column: "TeamBId");

            migrationBuilder.CreateIndex(
                name: "IX_Series_TournamentId",
                table: "Series",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_GameStateId",
                table: "Tournament",
                column: "GameStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_OrganizationId",
                table: "Tournament",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_WinnerId",
                table: "Tournament",
                column: "WinnerId");

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
                name: "FK_Players_GameState_GameStateId",
                table: "Players",
                column: "GameStateId",
                principalTable: "GameState",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coaches_GameState_GameStateId",
                table: "Coaches");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameState_GameStateId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_GameState_GameStateId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_GameState_GameStateId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Organization_OrganizationId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "Manager");

            migrationBuilder.DropTable(
                name: "PlayerStats_CS");

            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.DropTable(
                name: "Map");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropTable(
                name: "Tournament");

            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.DropTable(
                name: "GameState");

            migrationBuilder.DropIndex(
                name: "IX_Teams_GameStateId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_OrganizationId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Players_GameStateId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Games_GameStateId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Coaches_GameStateId",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "GameStateId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Buyout",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "GameStateId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Happiness",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Money",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "WeeklySalary",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "GameStateId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Buyout",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "GameStateId",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "Happiness",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "Money",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "WeeklySalary",
                table: "Coaches");

            migrationBuilder.AlterColumn<int>(
                name: "WeeklyExpense",
                table: "Teams",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<int>(
                name: "Budget",
                table: "Teams",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");
        }
    }
}
