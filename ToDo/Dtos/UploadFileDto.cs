namespace ToDo.Dtos;

public class UploadFileDto
{
    public Guid UploadFileId { get; set; }
    public string Name { get; set; }
    public string Src { get; set; }
    public Guid ToDoId { get; set; }
}