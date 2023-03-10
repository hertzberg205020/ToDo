using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ToDo.Dtos;
using ToDo.Models;
using ToDo.Parameters;

namespace ToDo.Services.Impl;

public class ToDoItemService: IToDoItemService
{
    private readonly ToDoDbContext _context;
    private readonly IMapper _mapper;

    public ToDoItemService(ToDoDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ToDoItemSelectDto>> GetToDoItemsAsync(TodoParameter parameters)
    {
        var result = _context
            .ToDoList
            .Include(a => a.UpdateEmployee)
            .Include(a => a.InsertEmployee)
            .Include(a => a.UploadFiles)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(parameters.Name))
        {
            // result = result.Where(a => a.Name.IndexOf(name) > -1);
            result = result.Where(a => a.Name.Contains(parameters.Name));
        }

        if (parameters.Enable != null)
        {
            result = result.Where(a => a.Enable == parameters.Enable);
        }

        if (parameters.InsertTime != null)
        {
            result = result.Where(a => a.InsertTime == parameters.InsertTime);
        }

        if (parameters.MinOrder != null && parameters.MaxOrder != null)
        {
            result = result.Where(a => a.Orders >= parameters.MinOrder && a.Orders <= parameters.MaxOrder);
        }

        var res = await result.ToListAsync();
        return _mapper.Map<List<ToDoItemSelectDto>>(res);
    }

    public async Task<ToDoItemSelectDto?> GetOneAsync(Guid id)
    {
        var res =  await  _context
            .ToDoList
            .Include(a => a.UpdateEmployee)
            .Include(a => a.InsertEmployee)
            .Include(a => a.UploadFiles)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.ToDoId == id);
        return res == null ? null : _mapper.Map<ToDoItemSelectDto>(res);
    }

    public async Task<Guid> AddAsync(ToDoItemPostDto dto)
    {
        var item = _mapper.Map<ToDoItem>(dto);
        item.ToDoId = Guid.NewGuid();

        item.InsertTime = DateTime.Now;
        item.UpdateTime = DateTime.Now;

        item.InsertEmployeeId = Guid.Parse("cc5fab39-0ee8-4615-b437-73ff89c81019");
        item.UpdateEmployeeId = Guid.Parse("f3c7567a-085b-40e9-8ad7-e0822bc7156a");

        await _context.ToDoList.AddAsync(item);
        await _context.SaveChangesAsync();
        return item.ToDoId;
    }

    public async Task<int> UpdateAsync(Guid toDoItemId, ToDoItemPutDto value)
    {
        // 不要直接將傳過的值上傳到資料庫中
        // Find(): 參數只能放Primary key
        var target = await _context.ToDoList.FindAsync(toDoItemId);

        // 此處邏輯有待加強
        if (target == null)
        {
            return 0;
        }

        #region 系統賦值

        target.UpdateTime = DateTime.Now;
        target.UpdateEmployeeId = Guid.Parse("cc5fab39-0ee8-4615-b437-73ff89c81019");

        #endregion

        #region 上傳的數值

        target.Name = value.Name;
        target.Orders = value.Orders;
        target.Enable = value.Enable;

        #endregion

        return await _context.SaveChangesAsync();  // 回傳受影響的資料筆數
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var target = _context.ToDoList
            .Include(e => e.UploadFiles)
            .SingleOrDefault(e => e.ToDoId == id);
        if (target == null)
        {
            return 0;
        }

        _context.ToDoList.Remove(target);
        return await _context.SaveChangesAsync();
    }
}