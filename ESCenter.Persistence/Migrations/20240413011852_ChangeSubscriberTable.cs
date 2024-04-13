using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESCenter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSubscriberTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriber_Customer_SubscriberId",
                table: "Subscriber");

            migrationBuilder.DropIndex(
                name: "IX_Subscriber_SubscriberId",
                table: "Subscriber");

            migrationBuilder.DropColumn(
                name: "SubscriberId",
                table: "Subscriber");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Subscriber",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Subscriber");

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriberId",
                table: "Subscriber",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Subscriber_SubscriberId",
                table: "Subscriber",
                column: "SubscriberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriber_Customer_SubscriberId",
                table: "Subscriber",
                column: "SubscriberId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
