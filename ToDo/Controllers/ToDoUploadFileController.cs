using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo.Dtos;
using ToDo.Models;

namespace ToDo.Controllers;


[Route("api/ToDo/{toDoId}/uploadFile")]
[ApiController]
public class ToDoUploadFileController: ControllerBase
{
    private readonly ToDoDbContext _context;
    private readonly IMapper _mapper;

    public ToDoUploadFileController(ToDoDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<UploadFileDto>> Get(Guid toDoId)
    {
        if (!_context.ToDoList.Any(a => a.ToDoId == toDoId))
        {
            return NotFound("找不到該事項");
        }
        var result = _context
            .UploadFiles
            .Include(a => a.ToDoItem)
            .Where(a => a.ToDoId == toDoId)
            .AsNoTracking();

        if (!result.Any())
        {
            return NotFound("找不到檔案");
        }

        return Ok(_mapper.Map<IEnumerable<UploadFileDto>>(result));
    }

    [HttpGet("{uploadFileId}")]
    public async Task<ActionResult<UploadFileDto>> Get(Guid toDoId, Guid uploadFileId)
    {
        // 檢查事項是否存在
        if (!_context.ToDoList.Any(a => a.ToDoId == toDoId))
        {
            return NotFound("找不到該事項");
        }

        var result = await _context
            .UploadFiles
            .Include(a => a.ToDoItem)
            .Where(a => a.ToDoId == toDoId && a.UploadFileId == uploadFileId)
            .AsNoTracking()
            .SingleOrDefaultAsync();

        if (result == null)
        {
            return NotFound("找不到檔案");
        }

        return Ok(_mapper.Map<UploadFileDto>(result));
    }

    [HttpPost]
    public async Task<ActionResult<UploadFileDto>> Post(Guid toDoId, [FromBody] UploadFilePostDto item)
    {
        // 檢查事項是否存在
        if (!_context.ToDoList.Any(a => a.ToDoId == toDoId))
        {
            return NotFound("找不到該事項");
        }
        // 絕對不要用entity物件接資料後，直接進行資料新增
        // 要守護資料的安全性，請先使用DTO來接收傳入的資料
        var uploadFile = new UploadFile
        {
            Name = item.Name,
            Src = item.Src,
            ToDoId = toDoId  // 這裡要設定由路由參數所獲取的ToDoItem的Id
        };
        _context.UploadFiles.Add(uploadFile);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { toDoId = toDoId, uploadFileId = uploadFile.UploadFileId }, _mapper.Map<UploadFileDto>(uploadFile));
    }

    [HttpPost("autoMapper")]
    public async Task<ActionResult<UploadFileDto>> PostAutoMapper(Guid toDoId, [FromBody] UploadFilePostDto uploadFilePostDto)
    {
        // 檢查事項是否存在
        if (!_context.ToDoList.Any(a => a.ToDoId == toDoId))
        {
            return NotFound("找不到該事項");
        }
        
        var uploadFile = _mapper.Map<UploadFile>(uploadFilePostDto);

        // 這裡要手動設定ToDoItem的Id
        uploadFile.ToDoId = toDoId;

        _context.UploadFiles.Add(uploadFile);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { toDoId = toDoId, uploadFileId = uploadFile.UploadFileId }, _mapper.Map<UploadFileDto>(uploadFile));
    }
}