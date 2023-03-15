using Microsoft.AspNetCore.Mvc.Filters;

namespace ToDo.Filters;

public class LogActionFilter: IActionFilter
{
    private readonly IWebHostEnvironment _env;

    public LogActionFilter(IWebHostEnvironment env)
    {
        _env = env;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var projectRoot = _env.ContentRootPath;
        var logFolder = Path.Combine(projectRoot, "Log");
        if (!Directory.Exists(logFolder))
        {
            Directory.CreateDirectory(logFolder);
        }
        var employeeId = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "EmployeeId")?.Value;
        var path = context.HttpContext.Request.Path;
        var method = context.HttpContext.Request.Method;
        var startTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        var logContent = $@"StartTime: {startTime}, Path: {path}, Method: {method}, EmployeeId: {employeeId}";
        var logFileName = DateTime.Now.ToString("yyyyMMdd") + ".txt";
        System.IO.File.AppendAllText(Path.Combine(logFolder, logFileName), logContent + Environment.NewLine);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // throw new NotImplementedException();
    }
}