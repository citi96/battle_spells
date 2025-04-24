using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Battle_Spells.Api.Migrations
{
    /// <inheritdoc />
    public partial class Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EffectDefinitions_Cards_Id",
                table: "EffectDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_PlayerMatchStates_Player1MatchStateId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_PlayerMatchStates_Player2MatchStateId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateDeckId",
                table: "MatchPlayerCards");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateGraveyardId",
                table: "MatchPlayerCards");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateHandId",
                table: "MatchPlayerCards");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateShopId",
                table: "MatchPlayerCards");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCards_Cards_CardId",
                table: "PlayerCards");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCards_Players_PlayerId",
                table: "PlayerCards");

            migrationBuilder.AddColumn<Guid>(
                name: "CardId",
                table: "EffectDefinitions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EffectDefinitions_CardId",
                table: "EffectDefinitions",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_EffectDefinitions_Cards_CardId",
                table: "EffectDefinitions",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_PlayerMatchStates_Player1MatchStateId",
                table: "Matches",
                column: "Player1MatchStateId",
                principalTable: "PlayerMatchStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_PlayerMatchStates_Player2MatchStateId",
                table: "Matches",
                column: "Player2MatchStateId",
                principalTable: "PlayerMatchStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateDeckId",
                table: "MatchPlayerCards",
                column: "PlayerMatchStateDeckId",
                principalTable: "PlayerMatchStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateGraveyardId",
                table: "MatchPlayerCards",
                column: "PlayerMatchStateGraveyardId",
                principalTable: "PlayerMatchStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateHandId",
                table: "MatchPlayerCards",
                column: "PlayerMatchStateHandId",
                principalTable: "PlayerMatchStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateShopId",
                table: "MatchPlayerCards",
                column: "PlayerMatchStateShopId",
                principalTable: "PlayerMatchStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCards_Cards_CardId",
                table: "PlayerCards",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCards_Players_PlayerId",
                table: "PlayerCards",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EffectDefinitions_Cards_CardId",
                table: "EffectDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_PlayerMatchStates_Player1MatchStateId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_PlayerMatchStates_Player2MatchStateId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateDeckId",
                table: "MatchPlayerCards");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateGraveyardId",
                table: "MatchPlayerCards");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateHandId",
                table: "MatchPlayerCards");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateShopId",
                table: "MatchPlayerCards");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCards_Cards_CardId",
                table: "PlayerCards");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCards_Players_PlayerId",
                table: "PlayerCards");

            migrationBuilder.DropIndex(
                name: "IX_EffectDefinitions_CardId",
                table: "EffectDefinitions");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "EffectDefinitions");

            migrationBuilder.AddForeignKey(
                name: "FK_EffectDefinitions_Cards_Id",
                table: "EffectDefinitions",
                column: "Id",
                principalTable: "Cards",
                principalColumn: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateDeckId",
                table: "MatchPlayerCards",
                column: "PlayerMatchStateDeckId",
                principalTable: "PlayerMatchStates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateGraveyardId",
                table: "MatchPlayerCards",
                column: "PlayerMatchStateGraveyardId",
                principalTable: "PlayerMatchStates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateHandId",
                table: "MatchPlayerCards",
                column: "PlayerMatchStateHandId",
                principalTable: "PlayerMatchStates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchPlayerCards_PlayerMatchStates_PlayerMatchStateShopId",
                table: "MatchPlayerCards",
                column: "PlayerMatchStateShopId",
                principalTable: "PlayerMatchStates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCards_Cards_CardId",
                table: "PlayerCards",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCards_Players_PlayerId",
                table: "PlayerCards",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
