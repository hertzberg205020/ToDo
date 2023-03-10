using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Migrations
{
    public partial class change_t_UploadFile_FK_policy_to_on_delete_cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadFile_ToDoId",
                table: "t_UploadFile");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadFile_ToDoId",
                table: "t_UploadFile",
                column: "ToDoId",
                principalTable: "t_ToDoItem",
                principalColumn: "ToDoId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadFile_ToDoId",
                table: "t_UploadFile");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadFile_ToDoId",
                table: "t_UploadFile",
                column: "ToDoId",
                principalTable: "t_ToDoItem",
                principalColumn: "ToDoId");
        }
    }
}
