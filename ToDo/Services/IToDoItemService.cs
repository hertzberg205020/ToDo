using ToDo.Dtos;
using ToDo.Parameters;

namespace ToDo.Services;

public interface IToDoItemService
{
    Task<List<ToDoItemSelectDto>> GetToDoItemsAsync(TodoParameter parameters);
    Task<ToDoItemSelectDto?> GetOneAsync(Guid id);
    Task<Guid> AddAsync(ToDoItemPostDto dto);
    Task<int> UpdateAsync(Guid id, ToDoItemPutDto dto);
    Task<int> DeleteAsync(Guid id);
}