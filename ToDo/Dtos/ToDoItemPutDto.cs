using ToDo.Abstracts;
using ToDo.ValidationAttributes;

namespace ToDo.Dtos;

public class ToDoItemPutDto: ToDoItemEditDto
{
    public Guid ToDoId { get; set; }
    
}
