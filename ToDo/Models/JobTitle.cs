namespace ToDo.Models;

public class JobTitle
{
    public Guid JobTitleId { get; set; }
    public string Name { get; set; }
    public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
}