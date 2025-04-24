using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Battle_Spells.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Heroes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    BaseHP = table.Column<int>(type: "INTEGER", nullable: false),
                    BaseOrbs = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Heroes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    MMR = table.Column<int>(type: "INTEGER", nullable: false),
                    MatchId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Flavor = table.Column<string>(type: "TEXT", nullable: false),
                    MaxHealth = table.Column<int>(type: "INTEGER", nullable: false),
                    ManaCost = table.Column<int>(type: "INTEGER", nullable: false),
                    Attack = table.Column<int>(type: "INTEGER", nullable: false),
                    Cost = table.Column<int>(type: "INTEGER", nullable: false),
                    EffectDescription = table.Column<string>(type: "TEXT", nullable: false),
                    ActivationEffects = table.Column<string>(type: "TEXT", nullable: false),
                    ActivationEffectsData = table.Column<string>(type: "TEXT", nullable: false),
                    Rarity = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Type = table.Column<ushort>(type: "INTEGER", nullable: false),
                    HeroId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Heroes_HeroId",
                        column: x => x.HeroId,
                        principalTable: "Heroes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EffectDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EffectType = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    SerializedParameters = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ParentEffectId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ConditionalEffectId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Condition = table.Column<ushort>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EffectDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EffectDefinitions_Cards_Id",
                        column: x => x.Id,
                        principalTable: "Cards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EffectDefinitions_EffectDefinitions_ConditionalEffectId",
                        column: x => x.ConditionalEffectId,
                        principalTable: "EffectDefinitions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EffectDefinitions_EffectDefinitions_ParentEffectId",
                        column: x => x.ParentEffectId,
                        principalTable: "EffectDefinitions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlayerCards",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CardId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCards", x => new { x.PlayerId, x.CardId });
                    table.ForeignKey(
                        name: "FK_PlayerCards_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerCards_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MatchId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ActionType = table.Column<string>(type: "TEXT", nullable: false),
                    SourceCardId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TargetCardId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsProcessed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchActions_Cards_SourceCardId",
                        column: x => x.SourceCardId,
                        principalTable: "Cards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MatchActions_Cards_TargetCardId",
                        column: x => x.TargetCardId,
                        principalTable: "Cards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MatchActions_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CurrentPlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TurnNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    LastActionTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Player1Id = table.Column<Guid>(type: "TEXT", nullable: true),
                    Player2Id = table.Column<Guid>(type: "TEXT", nullable: true),
                    Player1MatchStateId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Player2MatchStateId = table.Column<Guid>(type: "TEXT", nullable: true),
                    State = table.Column<ushort>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Players_CurrentPlayerId",
                        column: x => x.CurrentPlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Matches_Players_Player1Id",
                        column: x => x.Player1Id,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Matches_Players_Player2Id",
                        column: x => x.Player2Id,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MatchStateChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MatchId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ActionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StateChangeType = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SerializedData = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Sequence = table.Column<int>(type: "INTEGER", nullable: false),
                    MatchActionId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchStateChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchStateChanges_MatchActions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "MatchActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchStateChanges_MatchActions_MatchActionId",
                        column: x => x.MatchActionId,
                        principalTable: "MatchActions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MatchStateChanges_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerMatchStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    HeroId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MatchId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMatchStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerMatchStates_Heroes_HeroId",
                        column: x => x.HeroId,
                        principalTable: "Heroes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerMatchStates_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerMatchStates_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchPlayerCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CardId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CurrentHealt = table.Column<int>(type: "INTEGER", nullable: false),
                    Location = table.Column<ushort>(type: "INTEGER", nullable: false),
                    PlayerMatchStateHandId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PlayerMatchStateGraveyardId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PlayerMatchStateDeckId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PlayerMatchStateShopId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchPlayerCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchPlayerCards_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateDeckId",
                        column: x => x.PlayerMatchStateDeckId,
                        principalTable: "PlayerMatchStates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateGraveyardId",
                        column: x => x.PlayerMatchStateGraveyardId,
                        principalTable: "PlayerMatchStates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateHandId",
                        column: x => x.PlayerMatchStateHandId,
                        principalTable: "PlayerMatchStates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateShopId",
                        column: x => x.PlayerMatchStateShopId,
                        principalTable: "PlayerMatchStates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_HeroId",
                table: "Cards",
                column: "HeroId");

            migrationBuilder.CreateIndex(
                name: "IX_EffectDefinitions_ConditionalEffectId",
                table: "EffectDefinitions",
                column: "ConditionalEffectId");

            migrationBuilder.CreateIndex(
                name: "IX_EffectDefinitions_ParentEffectId",
                table: "EffectDefinitions",
                column: "ParentEffectId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchActions_MatchId",
                table: "MatchActions",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchActions_PlayerId",
                table: "MatchActions",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchActions_SourceCardId",
                table: "MatchActions",
                column: "SourceCardId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchActions_TargetCardId",
                table: "MatchActions",
                column: "TargetCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_CurrentPlayerId",
                table: "Matches",
                column: "CurrentPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player1Id",
                table: "Matches",
                column: "Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player1MatchStateId",
                table: "Matches",
                column: "Player1MatchStateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player2Id",
                table: "Matches",
                column: "Player2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Player2MatchStateId",
                table: "Matches",
                column: "Player2MatchStateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayerCards_CardId",
                table: "MatchPlayerCards",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayerCards_PlayerMatchStateDeckId",
                table: "MatchPlayerCards",
                column: "PlayerMatchStateDeckId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayerCards_PlayerMatchStateGraveyardId",
                table: "MatchPlayerCards",
                column: "PlayerMatchStateGraveyardId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayerCards_PlayerMatchStateHandId",
                table: "MatchPlayerCards",
                column: "PlayerMatchStateHandId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayerCards_PlayerMatchStateShopId",
                table: "MatchPlayerCards",
                column: "PlayerMatchStateShopId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchStateChanges_ActionId",
                table: "MatchStateChanges",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchStateChanges_MatchActionId",
                table: "MatchStateChanges",
                column: "MatchActionId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchStateChanges_MatchId",
                table: "MatchStateChanges",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCards_CardId",
                table: "PlayerCards",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMatchStates_HeroId",
                table: "PlayerMatchStates",
                column: "HeroId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMatchStates_MatchId",
                table: "PlayerMatchStates",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMatchStates_PlayerId",
                table: "PlayerMatchStates",
                column: "PlayerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchActions_Matches_MatchId",
                table: "MatchActions",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_PlayerMatchStates_Player1MatchStateId",
                table: "Matches",
                column: "Player1MatchStateId",
                principalTable: "PlayerMatchStates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_PlayerMatchStates_Player2MatchStateId",
                table: "Matches",
                column: "Player2MatchStateId",
                principalTable: "PlayerMatchStates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerMatchStates_Heroes_HeroId",
                table: "PlayerMatchStates");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerMatchStates_Matches_MatchId",
                table: "PlayerMatchStates");

            migrationBuilder.DropTable(
                name: "EffectDefinitions");

            migrationBuilder.DropTable(
                name: "MatchPlayerCards");

            migrationBuilder.DropTable(
                name: "MatchStateChanges");

            migrationBuilder.DropTable(
                name: "PlayerCards");

            migrationBuilder.DropTable(
                name: "MatchActions");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Heroes");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "PlayerMatchStates");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
