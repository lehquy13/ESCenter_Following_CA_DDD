using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESCenter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFKForTutor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Tutor_User_Id",
                table: "Tutor",
                column: "Id",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tutor_User_Id",
                table: "Tutor");
        }
    }
}
