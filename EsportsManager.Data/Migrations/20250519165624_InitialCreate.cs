using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ShortName = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    IconURL = table.Column<string>(type: "TEXT", nullable: false),
                    Genre = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Budget = table.Column<int>(type: "INTEGER", nullable: false),
                    WeeklyExpense = table.Column<int>(type: "INTEGER", nullable: false),
                    Prestige = table.Column<int>(type: "INTEGER", nullable: false),
                    DivisionalLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Coaches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Prestiege = table.Column<int>(type: "INTEGER", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    Communication = table.Column<double>(type: "REAL", nullable: false),
                    Strategy = table.Column<double>(type: "REAL", nullable: false),
                    GameSense = table.Column<double>(type: "REAL", nullable: false),
                    Adaptability = table.Column<double>(type: "REAL", nullable: false),
                    Mentorship = table.Column<double>(type: "REAL", nullable: false),
                    TeamId = table.Column<Guid>(type: "TEXT", nullable: true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Nickname = table.Column<string>(type: "TEXT", nullable: false),
                    Reputation = table.Column<int>(type: "INTEGER", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    Motivation = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coaches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coaches_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Prestiege = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating = table.Column<double>(type: "REAL", nullable: false),
                    TeamId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TeamId1 = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Nickname = table.Column<string>(type: "TEXT", nullable: false),
                    Reputation = table.Column<int>(type: "INTEGER", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    Motivation = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamId1",
                        column: x => x.TeamId1,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TeamId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ProfileType = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: true),
                    isBenched = table.Column<bool>(type: "INTEGER", nullable: true),
                    Tactics = table.Column<double>(type: "REAL", nullable: true),
                    Mechanics = table.Column<double>(type: "REAL", nullable: true),
                    Reflexes = table.Column<double>(type: "REAL", nullable: true),
                    Communication = table.Column<double>(type: "REAL", nullable: true),
                    Teamwork = table.Column<double>(type: "REAL", nullable: true),
                    GameSense = table.Column<double>(type: "REAL", nullable: true),
                    Adaptability = table.Column<double>(type: "REAL", nullable: true),
                    Kills = table.Column<int>(type: "INTEGER", nullable: true),
                    Deaths = table.Column<int>(type: "INTEGER", nullable: true),
                    Assists = table.Column<int>(type: "INTEGER", nullable: true),
                    DamageDealt = table.Column<int>(type: "INTEGER", nullable: true),
                    HeadShotKills = table.Column<int>(type: "INTEGER", nullable: true),
                    MapsPlayed = table.Column<int>(type: "INTEGER", nullable: true),
                    RoundsPlayed = table.Column<int>(type: "INTEGER", nullable: true),
                    RoundsWithKillsOrAssists = table.Column<int>(type: "INTEGER", nullable: true),
                    RoundsTraded = table.Column<int>(type: "INTEGER", nullable: true),
                    RoundsSurvived = table.Column<int>(type: "INTEGER", nullable: true),
                    GameProfile_LoL_Role = table.Column<int>(type: "INTEGER", nullable: true),
                    GameProfile_LoL_isBenched = table.Column<bool>(type: "INTEGER", nullable: true),
                    GameProfile_LoL_Tactics = table.Column<double>(type: "REAL", nullable: true),
                    GameProfile_LoL_Mechanics = table.Column<double>(type: "REAL", nullable: true),
                    GameProfile_LoL_Reflexes = table.Column<double>(type: "REAL", nullable: true),
                    GameProfile_LoL_Communication = table.Column<double>(type: "REAL", nullable: true),
                    GameProfile_LoL_Teamwork = table.Column<double>(type: "REAL", nullable: true),
                    GameProfile_LoL_GameSense = table.Column<double>(type: "REAL", nullable: true),
                    GameProfile_LoL_Adaptability = table.Column<double>(type: "REAL", nullable: true),
                    GameProfile_LoL_Kills = table.Column<int>(type: "INTEGER", nullable: true),
                    GameProfile_LoL_Deaths = table.Column<int>(type: "INTEGER", nullable: true),
                    GameProfile_LoL_Assists = table.Column<int>(type: "INTEGER", nullable: true),
                    CS = table.Column<int>(type: "INTEGER", nullable: true),
                    GamesPlayed = table.Column<int>(type: "INTEGER", nullable: true),
                    MinutesPlayed = table.Column<int>(type: "INTEGER", nullable: true),
                    WardsPlaced = table.Column<int>(type: "INTEGER", nullable: true),
                    WardsCleared = table.Column<int>(type: "INTEGER", nullable: true),
                    ObjectivesTaken = table.Column<int>(type: "INTEGER", nullable: true),
                    GameProfile_SC_Role = table.Column<int>(type: "INTEGER", nullable: true),
                    GameProfile_SC_isBenched = table.Column<bool>(type: "INTEGER", nullable: true),
                    GameProfile_SC_Tactics = table.Column<double>(type: "REAL", nullable: true),
                    GameProfile_SC_Mechanics = table.Column<double>(type: "REAL", nullable: true),
                    Micro = table.Column<double>(type: "REAL", nullable: true),
                    Macro = table.Column<double>(type: "REAL", nullable: true),
                    GameProfile_SC_GameSense = table.Column<double>(type: "REAL", nullable: true),
                    GameProfile_SC_Adaptability = table.Column<double>(type: "REAL", nullable: true),
                    APM = table.Column<int>(type: "INTEGER", nullable: true),
                    GameProfile_SC_MapsPlayed = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameProfiles_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameProfiles_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameProfiles_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_TeamId",
                table: "Coaches",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_GameProfiles_GameId",
                table: "GameProfiles",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameProfiles_PlayerId",
                table: "GameProfiles",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GameProfiles_TeamId",
                table: "GameProfiles",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId1",
                table: "Players",
                column: "TeamId1");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_GameId",
                table: "Teams",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coaches");

            migrationBuilder.DropTable(
                name: "GameProfiles");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
