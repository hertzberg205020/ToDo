using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Migrations
{
    public partial class add_t_UploadFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_UploadFile",
                columns: table => new
                {
                    UploadFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Src = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TodoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_UploadFile", x => x.UploadFileId);
                    table.ForeignKey(
                        name: "FK_UploadFile_ToDoId",
                        column: x => x.TodoId,
                        principalTable: "t_ToDoItem",
                        principalColumn: "TodoId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_UploadFile_TodoId",
                table: "t_UploadFile",
                column: "TodoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_UploadFile");
        }
    }
}
