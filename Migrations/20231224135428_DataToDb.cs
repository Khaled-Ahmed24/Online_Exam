using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Exam.Migrations
{
    /// <inheritdoc />
    public partial class DataToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    U_Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    U_FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    U_PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    U_Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ConfirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.U_Email);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Exam_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adminstration_Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Exam_title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Start_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Exam_id);
                    table.ForeignKey(
                        name: "FK_Exams_Users_Adminstration_Email",
                        column: x => x.Adminstration_Email,
                        principalTable: "Users",
                        principalColumn: "U_Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Exam_id = table.Column<int>(type: "int", nullable: false),
                    Question_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question_title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Question_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => new { x.Exam_id, x.Question_id });
                    table.ForeignKey(
                        name: "FK_Questions_Exams_Exam_id",
                        column: x => x.Exam_id,
                        principalTable: "Exams",
                        principalColumn: "Exam_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Exam_id = table.Column<int>(type: "int", nullable: false),
                    Question_id = table.Column<int>(type: "int", nullable: false),
                    U_Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Answer_text = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Points_Earned = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => new { x.U_Email, x.Exam_id, x.Question_id, x.Answer_text });
                    table.ForeignKey(
                        name: "FK_Answers_Questions_Exam_id_Question_id",
                        columns: x => new { x.Exam_id, x.Question_id },
                        principalTable: "Questions",
                        principalColumns: new[] { "Exam_id", "Question_id" });
                    table.ForeignKey(
                        name: "FK_Answers_Users_U_Email",
                        column: x => x.U_Email,
                        principalTable: "Users",
                        principalColumn: "U_Email");
                });

            migrationBuilder.CreateTable(
                name: "Choices",
                columns: table => new
                {
                    Exam_id = table.Column<int>(type: "int", nullable: false),
                    Question_id = table.Column<int>(type: "int", nullable: false),
                    Choice_text = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Is_correct = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choices", x => new { x.Exam_id, x.Question_id, x.Choice_text });
                    table.ForeignKey(
                        name: "FK_Choices_Questions_Exam_id_Question_id",
                        columns: x => new { x.Exam_id, x.Question_id },
                        principalTable: "Questions",
                        principalColumns: new[] { "Exam_id", "Question_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_Exam_id_Question_id",
                table: "Answers",
                columns: new[] { "Exam_id", "Question_id" });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_Adminstration_Email",
                table: "Exams",
                column: "Adminstration_Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Choices");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
