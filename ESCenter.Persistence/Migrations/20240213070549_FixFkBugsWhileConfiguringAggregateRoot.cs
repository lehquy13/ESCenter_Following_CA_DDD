using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESCenter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixFkBugsWhileConfiguringAggregateRoot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discovery_Subject_SubjectId",
                table: "Discovery");

            migrationBuilder.DropIndex(
                name: "IX_Discovery_SubjectId",
                table: "Discovery");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Discovery");

            migrationBuilder.CreateIndex(
                name: "IX_DiscoverySubject_SubjectId",
                table: "DiscoverySubject",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscoverySubject_Subject_SubjectId",
                table: "DiscoverySubject",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscoverySubject_Subject_SubjectId",
                table: "DiscoverySubject");

            migrationBuilder.DropIndex(
                name: "IX_DiscoverySubject_SubjectId",
                table: "DiscoverySubject");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Discovery",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Discovery_SubjectId",
                table: "Discovery",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Discovery_Subject_SubjectId",
                table: "Discovery",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
