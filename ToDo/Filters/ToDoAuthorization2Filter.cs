using Microsoft.AspNetCore.Mvc.Filters;
using ToDo.Models;

namespace ToDo.Filters;

public class ToDoAuthorization2Filter: Attribute, IAuthorizationFilter
{
    // 為了獲取特性上的屬性值 [Roles = "manager"]
    // 不能使用建構式注入 ToDoDbContext 物件
    public string Roles { get; set; }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var toDoDbContext = context.HttpContext.
            RequestServices.
            GetRequiredService<ToDoDbContext>();

        // 後續的判斷邏輯
    }
}