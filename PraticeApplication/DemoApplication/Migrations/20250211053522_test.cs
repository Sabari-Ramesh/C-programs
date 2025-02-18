using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoApplication.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_Courses_CoursescourseId",
                table: "StudentCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_Students_StudentsstudentId",
                table: "StudentCourses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentCourses",
                table: "StudentCourses");

            migrationBuilder.RenameTable(
                name: "StudentCourses",
                newName: "CourseStudent");

            migrationBuilder.RenameColumn(
                name: "studentId",
                table: "Students",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "StudentsstudentId",
                table: "CourseStudent",
                newName: "stdId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourses_StudentsstudentId",
                table: "CourseStudent",
                newName: "IX_CourseStudent_stdId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseStudent",
                table: "CourseStudent",
                columns: new[] { "CoursescourseId", "stdId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseStudent_Courses_CoursescourseId",
                table: "CourseStudent",
                column: "CoursescourseId",
                principalTable: "Courses",
                principalColumn: "courseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseStudent_Students_stdId",
                table: "CourseStudent",
                column: "stdId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseStudent_Courses_CoursescourseId",
                table: "CourseStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseStudent_Students_stdId",
                table: "CourseStudent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseStudent",
                table: "CourseStudent");

            migrationBuilder.RenameTable(
                name: "CourseStudent",
                newName: "StudentCourses");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Students",
                newName: "studentId");

            migrationBuilder.RenameColumn(
                name: "stdId",
                table: "StudentCourses",
                newName: "StudentsstudentId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseStudent_stdId",
                table: "StudentCourses",
                newName: "IX_StudentCourses_StudentsstudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentCourses",
                table: "StudentCourses",
                columns: new[] { "CoursescourseId", "StudentsstudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_Courses_CoursescourseId",
                table: "StudentCourses",
                column: "CoursescourseId",
                principalTable: "Courses",
                principalColumn: "courseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_Students_StudentsstudentId",
                table: "StudentCourses",
                column: "StudentsstudentId",
                principalTable: "Students",
                principalColumn: "studentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
