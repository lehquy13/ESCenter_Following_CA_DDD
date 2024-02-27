using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESCenter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseFee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Course",
                newName: "SectionFee");

            migrationBuilder.AddColumn<float>(
                name: "ChargeFee",
                table: "Course",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChargeFee",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "SectionFee",
                table: "Course",
                newName: "Amount");
        }
    }
}
