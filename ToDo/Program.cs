using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using ToDo.Models;
using ToDo.Services;
using ToDo.Services.Impl;

namespace ToDo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ToDoDbContext>(opt =>
            {
                var connStr = builder.Configuration.GetConnectionString("Demo2");
                opt.UseSqlServer(connStr);
            });

            #region register Automapper

            // 註冊所有加入web api專案參考的程式集
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #endregion

            builder.Services.AddScoped<IToDoItemService, ToDoItemService>();

            #region 啟用Cookie驗證機制

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                //未登入時會自動導到這個網址
                option.LoginPath = new PathString("/api/Auth/NoLogin");
                // 沒有權限時會被自動導向這個網址
                option.AccessDeniedPath = new PathString("/api/Auth/NoLogin");
            });

            builder.Services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
            }); ;

            #endregion

            builder.Services.AddHttpContextAccessor();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            #region 允許訪問 wwwroot 資料夾下的內容

            app.UseStaticFiles();

            #endregion

            app.UseHttpsRedirection();

            // app.UseAuthorization();

            #region 身分驗證

            //順序要一樣
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            #endregion

            app.MapControllers();

            app.Run();
        }
    }
}