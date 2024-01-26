using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class GameReferencesForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_Player2Id",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Games_GameId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Games_Player1Id",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_Player2Id",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Player2Id",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "Users",
                newName: "AsWatcherId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_GameId",
                table: "Users",
                newName: "IX_Users_AsWatcherId");

            migrationBuilder.AddColumn<int>(
                name: "AsPlayerId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AsPlayerId",
                table: "Users",
                column: "AsPlayerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_Player1Id",
                table: "Games",
                column: "Player1Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Games_AsPlayerId",
                table: "Users",
                column: "AsPlayerId",
                principalTable: "Games",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Games_AsWatcherId",
                table: "Users",
                column: "AsWatcherId",
                principalTable: "Games",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Games_AsPlayerId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Games_AsWatcherId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AsPlayerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Games_Player1Id",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "AsPlayerId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "AsWatcherId",
                table: "Users",
                newName: "GameId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_AsWatcherId",
                table: "Users",
                newName: "IX_Users_GameId");

            migrationBuilder.AddColumn<Guid>(
                name: "Player2Id",
                table: "Games",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_Player1Id",
                table: "Games",
                column: "Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Player2Id",
                table: "Games",
                column: "Player2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_Player2Id",
                table: "Games",
                column: "Player2Id",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Games_GameId",
                table: "Users",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
        }
    }
}
