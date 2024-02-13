using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESCenter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTutorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tutor_User_Id",
                table: "Tutor");

            migrationBuilder.RenameColumn(
                name: "TutorId",
                table: "Subscriber",
                newName: "SubscriberId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Tutor",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tutor_UserId",
                table: "Tutor",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriber_SubscriberId",
                table: "Subscriber",
                column: "SubscriberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriber_User_SubscriberId",
                table: "Subscriber",
                column: "SubscriberId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tutor_User_UserId",
                table: "Tutor",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriber_User_SubscriberId",
                table: "Subscriber");

            migrationBuilder.DropForeignKey(
                name: "FK_Tutor_User_UserId",
                table: "Tutor");

            migrationBuilder.DropIndex(
                name: "IX_Tutor_UserId",
                table: "Tutor");

            migrationBuilder.DropIndex(
                name: "IX_Subscriber_SubscriberId",
                table: "Subscriber");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tutor");

            migrationBuilder.RenameColumn(
                name: "SubscriberId",
                table: "Subscriber",
                newName: "TutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tutor_User_Id",
                table: "Tutor",
                column: "Id",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
