using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Dtos;

namespace ToDo.Filters;

public class ToDoAuthorizationFilter: Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        bool hasToke = context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
        
        var isIgnore = context.ActionDescriptor.EndpointMetadata.Any(a => a.GetType() == typeof(AllowAnonymousAttribute));

        // 沒有令牌且不可匿名訪問，才返回 沒有登入
        if (!(hasToke || isIgnore))
        {
            // context.Result只要被賦值，就會直接將response返回給client
            // 不會繼續往後走
            // context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
            context.Result = new JsonResult(new ReturnJson()
            {
                Data = "test1",
                ErrorMessage = "沒有登入",
                HttpStatusCode = 401
            });
        }
    }
}