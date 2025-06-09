using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration34 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Sponsorships_SponsorId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Sponsorships_GameStates_GameStateId",
                table: "Sponsorships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sponsorships",
                table: "Sponsorships");

            migrationBuilder.RenameTable(
                name: "Sponsorships",
                newName: "Sponsors");

            migrationBuilder.RenameIndex(
                name: "IX_Sponsorships_GameStateId",
                table: "Sponsors",
                newName: "IX_Sponsors_GameStateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sponsors",
                table: "Sponsors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Sponsors_SponsorId",
                table: "Contracts",
                column: "SponsorId",
                principalTable: "Sponsors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sponsors_GameStates_GameStateId",
                table: "Sponsors",
                column: "GameStateId",
                principalTable: "GameStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Sponsors_SponsorId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Sponsors_GameStates_GameStateId",
                table: "Sponsors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sponsors",
                table: "Sponsors");

            migrationBuilder.RenameTable(
                name: "Sponsors",
                newName: "Sponsorships");

            migrationBuilder.RenameIndex(
                name: "IX_Sponsors_GameStateId",
                table: "Sponsorships",
                newName: "IX_Sponsorships_GameStateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sponsorships",
                table: "Sponsorships",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Sponsorships_SponsorId",
                table: "Contracts",
                column: "SponsorId",
                principalTable: "Sponsorships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sponsorships_GameStates_GameStateId",
                table: "Sponsorships",
                column: "GameStateId",
                principalTable: "GameStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
