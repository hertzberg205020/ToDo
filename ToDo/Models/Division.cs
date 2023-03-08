namespace ToDo.Models;

public class Division
{
    public Guid DivisionId { get; set; }
    public string Name { get; set; }

    public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
}