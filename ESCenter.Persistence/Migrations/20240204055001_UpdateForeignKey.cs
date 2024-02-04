using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESCenter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangeVerificationRequest_Tutors_TutorId",
                table: "ChangeVerificationRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseRequest_Courses_CourseId",
                table: "CourseRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscoverySubject_Discoveries_DiscoveryId",
                table: "DiscoverySubject");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUsers_IdentityRoles_IdentityRoleId",
                table: "IdentityUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Courses_CourseId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_TutorMajor_Tutors_TutorId",
                table: "TutorMajor");

            migrationBuilder.DropForeignKey(
                name: "FK_TutorVerificationInfo_Tutors_TutorId",
                table: "TutorVerificationInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tutors",
                table: "Tutors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TutorRequests",
                table: "TutorRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscribers",
                table: "Subscribers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUsers",
                table: "IdentityUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityRoles",
                table: "IdentityRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscoveryUsers",
                table: "DiscoveryUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discoveries",
                table: "Discoveries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tutors");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Tutors",
                newName: "Tutor");

            migrationBuilder.RenameTable(
                name: "TutorRequests",
                newName: "TutorRequest");

            migrationBuilder.RenameTable(
                name: "Subscribers",
                newName: "Subscriber");

            migrationBuilder.RenameTable(
                name: "Subjects",
                newName: "Subject");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameTable(
                name: "IdentityUsers",
                newName: "IdentityUser");

            migrationBuilder.RenameTable(
                name: "IdentityRoles",
                newName: "IdentityRole");

            migrationBuilder.RenameTable(
                name: "DiscoveryUsers",
                newName: "DiscoveryUser");

            migrationBuilder.RenameTable(
                name: "Discoveries",
                newName: "Discovery");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "Course");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "User",
                newName: "IX_User_Email");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityUsers_IdentityRoleId",
                table: "IdentityUser",
                newName: "IX_IdentityUser_IdentityRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityUsers_Email",
                table: "IdentityUser",
                newName: "IX_IdentityUser_Email");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Discovery",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "LearnerId",
                table: "Course",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tutor",
                table: "Tutor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TutorRequest",
                table: "TutorRequest",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscriber",
                table: "Subscriber",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subject",
                table: "Subject",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUser",
                table: "IdentityUser",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityRole",
                table: "IdentityRole",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscoveryUser",
                table: "DiscoveryUser",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discovery",
                table: "Discovery",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TutorMajor_SubjectId",
                table: "TutorMajor",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRequest_TutorId",
                table: "CourseRequest",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorRequest_LearnerId",
                table: "TutorRequest",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorRequest_TutorId",
                table: "TutorRequest",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscoveryUser_DiscoveryId",
                table: "DiscoveryUser",
                column: "DiscoveryId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscoveryUser_UserId",
                table: "DiscoveryUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Discovery_SubjectId",
                table: "Discovery",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_LearnerId",
                table: "Course",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_SubjectId",
                table: "Course",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeVerificationRequest_Tutor_TutorId",
                table: "ChangeVerificationRequest",
                column: "TutorId",
                principalTable: "Tutor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Subject_SubjectId",
                table: "Course",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_User_LearnerId",
                table: "Course",
                column: "LearnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseRequest_Course_CourseId",
                table: "CourseRequest",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseRequest_Tutor_TutorId",
                table: "CourseRequest",
                column: "TutorId",
                principalTable: "Tutor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Discovery_Subject_SubjectId",
                table: "Discovery",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscoverySubject_Discovery_DiscoveryId",
                table: "DiscoverySubject",
                column: "DiscoveryId",
                principalTable: "Discovery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscoveryUser_Discovery_DiscoveryId",
                table: "DiscoveryUser",
                column: "DiscoveryId",
                principalTable: "Discovery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscoveryUser_User_UserId",
                table: "DiscoveryUser",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUser_IdentityRole_IdentityRoleId",
                table: "IdentityUser",
                column: "IdentityRoleId",
                principalTable: "IdentityRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Course_CourseId",
                table: "Review",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TutorMajor_Subject_SubjectId",
                table: "TutorMajor",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TutorMajor_Tutor_TutorId",
                table: "TutorMajor",
                column: "TutorId",
                principalTable: "Tutor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TutorRequest_Tutor_TutorId",
                table: "TutorRequest",
                column: "TutorId",
                principalTable: "Tutor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TutorRequest_User_LearnerId",
                table: "TutorRequest",
                column: "LearnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TutorVerificationInfo_Tutor_TutorId",
                table: "TutorVerificationInfo",
                column: "TutorId",
                principalTable: "Tutor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangeVerificationRequest_Tutor_TutorId",
                table: "ChangeVerificationRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_Subject_SubjectId",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_User_LearnerId",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseRequest_Course_CourseId",
                table: "CourseRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseRequest_Tutor_TutorId",
                table: "CourseRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_Discovery_Subject_SubjectId",
                table: "Discovery");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscoverySubject_Discovery_DiscoveryId",
                table: "DiscoverySubject");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscoveryUser_Discovery_DiscoveryId",
                table: "DiscoveryUser");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscoveryUser_User_UserId",
                table: "DiscoveryUser");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUser_IdentityRole_IdentityRoleId",
                table: "IdentityUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Course_CourseId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_TutorMajor_Subject_SubjectId",
                table: "TutorMajor");

            migrationBuilder.DropForeignKey(
                name: "FK_TutorMajor_Tutor_TutorId",
                table: "TutorMajor");

            migrationBuilder.DropForeignKey(
                name: "FK_TutorRequest_Tutor_TutorId",
                table: "TutorRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_TutorRequest_User_LearnerId",
                table: "TutorRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_TutorVerificationInfo_Tutor_TutorId",
                table: "TutorVerificationInfo");

            migrationBuilder.DropIndex(
                name: "IX_TutorMajor_SubjectId",
                table: "TutorMajor");

            migrationBuilder.DropIndex(
                name: "IX_CourseRequest_TutorId",
                table: "CourseRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TutorRequest",
                table: "TutorRequest");

            migrationBuilder.DropIndex(
                name: "IX_TutorRequest_LearnerId",
                table: "TutorRequest");

            migrationBuilder.DropIndex(
                name: "IX_TutorRequest_TutorId",
                table: "TutorRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tutor",
                table: "Tutor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscriber",
                table: "Subscriber");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subject",
                table: "Subject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUser",
                table: "IdentityUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityRole",
                table: "IdentityRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscoveryUser",
                table: "DiscoveryUser");

            migrationBuilder.DropIndex(
                name: "IX_DiscoveryUser_DiscoveryId",
                table: "DiscoveryUser");

            migrationBuilder.DropIndex(
                name: "IX_DiscoveryUser_UserId",
                table: "DiscoveryUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discovery",
                table: "Discovery");

            migrationBuilder.DropIndex(
                name: "IX_Discovery_SubjectId",
                table: "Discovery");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_LearnerId",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_SubjectId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Discovery");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "TutorRequest",
                newName: "TutorRequests");

            migrationBuilder.RenameTable(
                name: "Tutor",
                newName: "Tutors");

            migrationBuilder.RenameTable(
                name: "Subscriber",
                newName: "Subscribers");

            migrationBuilder.RenameTable(
                name: "Subject",
                newName: "Subjects");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "IdentityUser",
                newName: "IdentityUsers");

            migrationBuilder.RenameTable(
                name: "IdentityRole",
                newName: "IdentityRoles");

            migrationBuilder.RenameTable(
                name: "DiscoveryUser",
                newName: "DiscoveryUsers");

            migrationBuilder.RenameTable(
                name: "Discovery",
                newName: "Discoveries");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "Courses");

            migrationBuilder.RenameIndex(
                name: "IX_User_Email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityUser_IdentityRoleId",
                table: "IdentityUsers",
                newName: "IX_IdentityUsers_IdentityRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityUser_Email",
                table: "IdentityUsers",
                newName: "IX_IdentityUsers_Email");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Tutors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "LearnerId",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TutorRequests",
                table: "TutorRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tutors",
                table: "Tutors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscribers",
                table: "Subscribers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUsers",
                table: "IdentityUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityRoles",
                table: "IdentityRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscoveryUsers",
                table: "DiscoveryUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discoveries",
                table: "Discoveries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeVerificationRequest_Tutors_TutorId",
                table: "ChangeVerificationRequest",
                column: "TutorId",
                principalTable: "Tutors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseRequest_Courses_CourseId",
                table: "CourseRequest",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscoverySubject_Discoveries_DiscoveryId",
                table: "DiscoverySubject",
                column: "DiscoveryId",
                principalTable: "Discoveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUsers_IdentityRoles_IdentityRoleId",
                table: "IdentityUsers",
                column: "IdentityRoleId",
                principalTable: "IdentityRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Courses_CourseId",
                table: "Review",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TutorMajor_Tutors_TutorId",
                table: "TutorMajor",
                column: "TutorId",
                principalTable: "Tutors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TutorVerificationInfo_Tutors_TutorId",
                table: "TutorVerificationInfo",
                column: "TutorId",
                principalTable: "Tutors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
