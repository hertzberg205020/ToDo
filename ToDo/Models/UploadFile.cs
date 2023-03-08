namespace ToDo.Models;

public class UploadFile
{
    public Guid UploadFileId { get; set; }
    public string Name { get; set; }
    public string Src { get; set; }
    public Guid ToDoId { get; set; }

    public ToDoItem ToDoItem { get; set; }
}