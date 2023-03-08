using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_Division",
                columns: table => new
                {
                    DivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_Division", x => x.DivisionId);
                });

            migrationBuilder.CreateTable(
                name: "t_JobTitle",
                columns: table => new
                {
                    JobTitleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_JobTitle", x => x.JobTitleId);
                });

            migrationBuilder.CreateTable(
                name: "t_Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobTitleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_Employee", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employee_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "t_Division",
                        principalColumn: "DivisionId");
                    table.ForeignKey(
                        name: "FK_Employee_JobTitleId",
                        column: x => x.JobTitleId,
                        principalTable: "t_JobTitle",
                        principalColumn: "JobTitleId");
                });

            migrationBuilder.CreateTable(
                name: "t_TodoList",
                columns: table => new
                {
                    TodoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Enable = table.Column<bool>(type: "bit", nullable: false),
                    Orders = table.Column<int>(type: "int", nullable: false),
                    InsertEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_TodoList", x => x.TodoId);
                    table.ForeignKey(
                        name: "FK_Todo_InsertEmpId",
                        column: x => x.InsertEmployeeId,
                        principalTable: "t_Employee",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_Todo_UpdateEmpId",
                        column: x => x.UpdateEmployeeId,
                        principalTable: "t_Employee",
                        principalColumn: "EmployeeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_Employee_DivisionId",
                table: "t_Employee",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_t_Employee_JobTitleId",
                table: "t_Employee",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_t_TodoList_InsertEmployeeId",
                table: "t_TodoList",
                column: "InsertEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_t_TodoList_UpdateEmployeeId",
                table: "t_TodoList",
                column: "UpdateEmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_TodoList");

            migrationBuilder.DropTable(
                name: "t_Employee");

            migrationBuilder.DropTable(
                name: "t_Division");

            migrationBuilder.DropTable(
                name: "t_JobTitle");
        }
    }
}
