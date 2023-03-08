namespace ToDo.Models;

public class Employee
{
    public Guid EmployeeId { get; set; }
    public string Name { get; set; }
    public string Account { get; set; }
    public string Password { get; set; }
    public Guid JobTitleId { get; set; }
    public Guid DivisionId { get; set; }

    public Division Division { get; set; }
    public JobTitle JobTitle { get; set; }
    public ICollection<ToDoItem> TodoListInsertEmployees { get; set; } = new HashSet<ToDoItem>();
    public ICollection<ToDoItem> TodoListUpdateEmployees { get; set; } = new HashSet<ToDoItem>();
}