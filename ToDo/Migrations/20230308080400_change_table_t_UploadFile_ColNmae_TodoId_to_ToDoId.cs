using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Migrations
{
    public partial class change_table_t_UploadFile_ColNmae_TodoId_to_ToDoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TodoId",
                table: "t_UploadFile",
                newName: "ToDoId");

            migrationBuilder.RenameIndex(
                name: "IX_t_UploadFile_TodoId",
                table: "t_UploadFile",
                newName: "IX_t_UploadFile_ToDoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToDoId",
                table: "t_UploadFile",
                newName: "TodoId");

            migrationBuilder.RenameIndex(
                name: "IX_t_UploadFile_ToDoId",
                table: "t_UploadFile",
                newName: "IX_t_UploadFile_TodoId");
        }
    }
}
