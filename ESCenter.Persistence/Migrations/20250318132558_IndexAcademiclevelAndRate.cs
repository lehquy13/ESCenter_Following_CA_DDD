using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESCenter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IndexAcademiclevelAndRat1e : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tutor_AcademicLevel",
                table: "Tutor",
                column: "AcademicLevel");

            migrationBuilder.CreateIndex(
                name: "IX_Tutor_Rate",
                table: "Tutor",
                column: "Rate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tutor_AcademicLevel",
                table: "Tutor");

            migrationBuilder.DropIndex(
                name: "IX_Tutor_Rate",
                table: "Tutor");
        }
    }
}
