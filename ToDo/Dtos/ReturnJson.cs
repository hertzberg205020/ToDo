namespace ToDo.Dtos;

public class ReturnJson
{
    public dynamic? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public int HttpStatusCode { get; set; }
    public dynamic? Error { get; set; }
}