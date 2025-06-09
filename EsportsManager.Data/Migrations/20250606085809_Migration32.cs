using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration32 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Genre",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "DedicatedPlayers",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "DeveloperId",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Sponsorships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    LogoUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Product = table.Column<int>(type: "INTEGER", nullable: false),
                    Tier = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sponsorships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameStateId = table.Column<Guid>(type: "TEXT", nullable: false),
                    isActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    ExpirationDate = table.Column<string>(type: "TEXT", nullable: false),
                    ContractType = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TeamId = table.Column<Guid>(type: "TEXT", nullable: true),
                    WeeklySalary = table.Column<double>(type: "REAL", nullable: true),
                    SigningBonus = table.Column<double>(type: "REAL", nullable: true),
                    Buyout = table.Column<double>(type: "REAL", nullable: true),
                    BenchedSalary = table.Column<double>(type: "REAL", nullable: true),
                    canBeBenched = table.Column<bool>(type: "INTEGER", nullable: true),
                    canBeSold = table.Column<bool>(type: "INTEGER", nullable: true),
                    mustHaveConsent = table.Column<bool>(type: "INTEGER", nullable: true),
                    SponsorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ContractSponsor_TeamId = table.Column<Guid>(type: "TEXT", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PeriodPayment = table.Column<double>(type: "REAL", nullable: true),
                    PeriodType = table.Column<int>(type: "INTEGER", nullable: true),
                    SponsorType = table.Column<int>(type: "INTEGER", nullable: true),
                    SatisfactionSponsor = table.Column<int>(type: "INTEGER", nullable: true),
                    SatisfactionTeam = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_GameStates_GameStateId",
                        column: x => x.GameStateId,
                        principalTable: "GameStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contracts_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_Sponsorships_SponsorId",
                        column: x => x.SponsorId,
                        principalTable: "Sponsorships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_Teams_ContractSponsor_TeamId",
                        column: x => x.ContractSponsor_TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contracts_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_DeveloperId",
                table: "Games",
                column: "DeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractSponsor_TeamId",
                table: "Contracts",
                column: "ContractSponsor_TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_GameStateId",
                table: "Contracts",
                column: "GameStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_OrganizationId",
                table: "Contracts",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_PlayerId",
                table: "Contracts",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_SponsorId",
                table: "Contracts",
                column: "SponsorId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_TeamId",
                table: "Contracts",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Organizations_DeveloperId",
                table: "Games",
                column: "DeveloperId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Organizations_DeveloperId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Sponsorships");

            migrationBuilder.DropIndex(
                name: "IX_Games_DeveloperId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "DedicatedPlayers",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "DeveloperId",
                table: "Games");

            migrationBuilder.AlterColumn<string>(
                name: "Genre",
                table: "Games",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
