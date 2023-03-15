using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ToDo.Dtos;

namespace ToDo.Filters;

/// <summary>
/// 統一回覆給前端的格式
/// </summary>
public class ToDoResultFilter: IResultFilter
{

    public void OnResultExecuting(ResultExecutingContext context)
    {
        var objectResult = context.Result as ObjectResult;
        if (context.ModelState.IsValid)
        {
            context.Result = new JsonResult(new ReturnJson()
            {
                Data = objectResult?.Value,
            });
        }
        else
        {
            context.Result = new JsonResult(new ReturnJson()
            {
                HttpStatusCode = 400,
                ErrorMessage = "資料驗證失敗",
                Error = context.ModelState
            });
        }
    }

    /// <summary>
    /// Web API不會用到這個
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnResultExecuted(ResultExecutedContext context)
    {
        // throw new NotImplementedException();
    }
}