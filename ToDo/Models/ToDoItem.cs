namespace ToDo.Models;

public class ToDoItem
{
    public Guid TodoId { get; set; }
    public string Name { get; set; }
    public DateTime InsertTime { get; set; }
    public DateTime UpdateTime { get; set; }
    public bool Enable { get; set; }
    public int Orders { get; set; }
    public Guid InsertEmployeeId { get; set; }
    public Guid UpdateEmployeeId { get; set; }

    public Employee InsertEmployee { get; set; }
    public Employee UpdateEmployee { get; set; }
    public List<UploadFile> UploadFiles { get; set; } = new List<UploadFile>();
}