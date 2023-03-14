using System.Text;
using Microsoft.AspNetCore.Mvc;
using ToDo.Helper;
using ToDo.Models;

namespace ToDo.Controllers;

/// <summary>
/// 檔案上傳
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class FileUploadController: ControllerBase
{
    // 不用額外註冊，直接注入即可
    private readonly IWebHostEnvironment _env;
    private readonly ToDoDbContext _context;

    public FileUploadController(IWebHostEnvironment env, ToDoDbContext context)
    {
        _env = env;
        _context = context;
    }

    /// <summary>
    /// 接收前端以form-data形式上傳的檔案
    /// </summary>
    /// <param name="file1">使用IFormFile取得上傳的檔案</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post(IFormFile file1)
    {
        if (file1.Length <= 0)
        {
            return BadRequest();
        }
        // var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file1.FileName);
        var filePath = Path.Combine(_env.WebRootPath, "UploadFiles", file1.FileName);

        // var fileName = file1.FileName;
        using (var stream = System.IO.File.Create(filePath))
        {
            await file1.CopyToAsync(stream);
        }

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> UploadFiles(List<IFormFile> files)
    {
        
        foreach (var file in files)
        {
            await UploadFile(file);
        }
        return Ok();
    }

    [HttpPost("{toDoItemId:guid}")]
    public async Task<IActionResult> UploadFiles(Guid toDoItemId, IFormFileCollection files)
    {
        foreach (var file in files)
        {
            // _env.WebRootPath: 直接就定位到 wwwroot 資料夾的絕對路徑
            // _env.ContentRootPath: 直接就定位到專案的根目錄的絕對路徑
            var folderPath = Path.Combine(_env.WebRootPath, "UploadFiles", toDoItemId.ToString());
            var success = await UploadFileHelper.UploadFileAsync(file, folderPath);
            
            if (!success)
            {
                continue;
            }

            var insert = new UploadFile()
            {
                Name = file.FileName,
                Src = UploadFileHelper.GetFileSrc(file, toDoItemId),
                ToDoId = toDoItemId
            };
            _context.UploadFiles.Add(insert);
        }
        
        await _context.SaveChangesAsync();
        return Ok();
    }


    private async Task UploadFile(IFormFile file)
    {

        if (file.Length <= 0)
        {
            return;
        }

        string filePath = Path.Combine(_env.WebRootPath, "UploadFiles", file.FileName);
        await using var stream = System.IO.File.Create(filePath);
        await file.CopyToAsync(stream);
    }

    
}