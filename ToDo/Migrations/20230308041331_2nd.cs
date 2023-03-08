using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Migrations
{
    public partial class _2nd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_t_TodoList",
                table: "t_TodoList");

            migrationBuilder.RenameTable(
                name: "t_TodoList",
                newName: "t_ToDoItem");

            migrationBuilder.RenameIndex(
                name: "IX_t_TodoList_UpdateEmployeeId",
                table: "t_ToDoItem",
                newName: "IX_t_ToDoItem_UpdateEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_t_TodoList_InsertEmployeeId",
                table: "t_ToDoItem",
                newName: "IX_t_ToDoItem_InsertEmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_t_ToDoItem",
                table: "t_ToDoItem",
                column: "TodoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_t_ToDoItem",
                table: "t_ToDoItem");

            migrationBuilder.RenameTable(
                name: "t_ToDoItem",
                newName: "t_TodoList");

            migrationBuilder.RenameIndex(
                name: "IX_t_ToDoItem_UpdateEmployeeId",
                table: "t_TodoList",
                newName: "IX_t_TodoList_UpdateEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_t_ToDoItem_InsertEmployeeId",
                table: "t_TodoList",
                newName: "IX_t_TodoList_InsertEmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_t_TodoList",
                table: "t_TodoList",
                column: "TodoId");
        }
    }
}
