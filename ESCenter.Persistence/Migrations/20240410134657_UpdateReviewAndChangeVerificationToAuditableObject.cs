using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESCenter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReviewAndChangeVerificationToAuditableObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Review",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Review",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Review",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifierId",
                table: "Review",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "ChangeVerificationRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "ChangeVerificationRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "ChangeVerificationRequest",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifierId",
                table: "ChangeVerificationRequest",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "ChangeVerificationRequest");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ChangeVerificationRequest");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "ChangeVerificationRequest");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "ChangeVerificationRequest");
        }
    }
}
