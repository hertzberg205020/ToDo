using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Dtos;
using ToDo.Models;

namespace ToDo.Controllers;

/// <summary>
/// 登入相關的controller記得要設定任何身分都能訪問
/// </summary>
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController: ControllerBase
{
    private readonly ToDoDbContext _context;

    public AuthController(ToDoDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public string Login(LoginPost value)
    {
        var user = (from a in _context.Employees
            where a.Account == value.Account
                  && a.Password == value.Password
            select a).SingleOrDefault();

        if (user == null)
        {
            return "帳號密碼錯誤";
        }

        //這邊等等寫驗證
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Account),
            new Claim("FullName", user.Name),
            new Claim("EmployeeId", user.EmployeeId.ToString()),
        };

        #region 設置角色

        // var roles = _context.Roles.Where(e => e.EmployeeId == user.EmployeeId);
        // foreach (var role in roles)
        // {
        //     claims.Add(new Claim(ClaimTypes.Role, role.Name));
        // }

        #endregion

        #region 權限憑證的設置

        var authProperties = new AuthenticationProperties
        {
            // 設定Cookie的有效期限
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
            // 設定Cookie是否可以被前端存取
            IsPersistent = true,
            // 設定Cookie的有效路徑
            // Path = new PathString("/api/Auth/Login")
        };

        #endregion


        #region 使用Cookie作為驗證方式
        // 製作認證的信物
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // 將信物存入Cookie
        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        #endregion

        return "登入成功";
    }

    [HttpDelete]
    public void Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpGet("NoLogin")]
    public string NoLogin()
    {
        return "未登入";
    }

    [HttpGet("NoAccess")]
    public string NoAccess()
    {
        return "權限不足";
    }
}