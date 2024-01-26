using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MoveUsrGameFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Games_AsPlayerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AsPlayerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AsPlayerId",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "Player2Id",
                table: "Games",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_Player2Id",
                table: "Games",
                column: "Player2Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_Player2Id",
                table: "Games",
                column: "Player2Id",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_Player2Id",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_Player2Id",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Player2Id",
                table: "Games");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Games_AsPlayerId",
                table: "Users",
                column: "AsPlayerId",
                principalTable: "Games",
                principalColumn: "Id");
        }
    }
}
