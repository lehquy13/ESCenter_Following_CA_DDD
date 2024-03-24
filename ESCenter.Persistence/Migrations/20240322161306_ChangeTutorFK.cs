using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESCenter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTutorFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tutor_Customer_UserId",
                table: "Tutor");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Tutor",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Tutor_UserId",
                table: "Tutor",
                newName: "IX_Tutor_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tutor_Customer_CustomerId",
                table: "Tutor",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tutor_Customer_CustomerId",
                table: "Tutor");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Tutor",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tutor_CustomerId",
                table: "Tutor",
                newName: "IX_Tutor_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tutor_Customer_UserId",
                table: "Tutor",
                column: "UserId",
                principalTable: "Customer",
                principalColumn: "Id");
        }
    }
}
