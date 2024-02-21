using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESCenter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIdentityUserToAuditable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "IdentityUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "IdentityUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleterId",
                table: "IdentityUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "IdentityUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "IdentityUser",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "IdentityUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifierId",
                table: "IdentityUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUser_User_Id",
                table: "IdentityUser",
                column: "Id",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUser_User_Id",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "IdentityUser");
        }
    }
}
