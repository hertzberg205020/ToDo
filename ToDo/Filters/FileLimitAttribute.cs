using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ToDo.Dtos;

namespace ToDo.Filters;

/// <summary>
/// 判斷上傳檔案副檔名和上傳檔案大小的過濾器
/// </summary>
public class FileLimitAttribute: Attribute, IResourceFilter
{
    // 單位是MB
    public long Size { get; set; } = 10;
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        var files = context.HttpContext.Request.Form.Files;
        foreach (var file in files)
        {
            if (file.Length > 1024 * 1024 * Size)
            {
                // context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult("文件大小不能超過10M");
                context.Result = new JsonResult(new ReturnJson()
                {
                    Data = null,
                    ErrorMessage = $"文件大小不能超過{Size}M",
                    HttpStatusCode = 400
                });
            }

            if (Path.GetExtension(file.FileName) != ".jpg")
            {
                context.Result = new JsonResult(new ReturnJson()
                {
                    Data = null,
                    ErrorMessage = "文件格式不正確",
                    HttpStatusCode = 400
                });
            }
        }
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        // throw new NotImplementedException();
    }
}