using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trait_Manager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IconURL = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trait_Manager", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Traits_Organization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IconURL = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traits_Organization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Traits_Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IconURL = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traits_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Traits_Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IconURL = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traits_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManagerTrait",
                columns: table => new
                {
                    ManagerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TraitId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AcquiredDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerTrait", x => new { x.ManagerId, x.TraitId });
                    table.ForeignKey(
                        name: "FK_ManagerTrait_Manager_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Manager",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManagerTrait_Trait_Manager_TraitId",
                        column: x => x.TraitId,
                        principalTable: "Trait_Manager",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationTraits",
                columns: table => new
                {
                    OrganizationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TraitId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AcquiredDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationTraits", x => new { x.OrganizationId, x.TraitId });
                    table.ForeignKey(
                        name: "FK_OrganizationTraits_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationTraits_Traits_Organization_TraitId",
                        column: x => x.TraitId,
                        principalTable: "Traits_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerTraits",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TraitId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AcquiredDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTraits", x => new { x.PlayerId, x.TraitId });
                    table.ForeignKey(
                        name: "FK_PlayerTraits_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerTraits_Traits_Players_TraitId",
                        column: x => x.TraitId,
                        principalTable: "Traits_Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamTraits",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TraitId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AcquiredDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamTraits", x => new { x.TeamId, x.TraitId });
                    table.ForeignKey(
                        name: "FK_TeamTraits_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamTraits_Traits_Teams_TraitId",
                        column: x => x.TraitId,
                        principalTable: "Traits_Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManagerTrait_TraitId",
                table: "ManagerTrait",
                column: "TraitId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationTraits_TraitId",
                table: "OrganizationTraits",
                column: "TraitId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTraits_TraitId",
                table: "PlayerTraits",
                column: "TraitId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTraits_TraitId",
                table: "TeamTraits",
                column: "TraitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManagerTrait");

            migrationBuilder.DropTable(
                name: "OrganizationTraits");

            migrationBuilder.DropTable(
                name: "PlayerTraits");

            migrationBuilder.DropTable(
                name: "TeamTraits");

            migrationBuilder.DropTable(
                name: "Trait_Manager");

            migrationBuilder.DropTable(
                name: "Traits_Organization");

            migrationBuilder.DropTable(
                name: "Traits_Players");

            migrationBuilder.DropTable(
                name: "Traits_Teams");
        }
    }
}
