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

            // ���U�Ҧ��[�Jweb api�M�װѦҪ��{����
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #endregion

            builder.Services.AddScoped<IToDoItemService, ToDoItemService>();

            #region �ҥ�Cookie���Ҿ���

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                //���n�J�ɷ|�۰ʾɨ�o�Ӻ��}
                option.LoginPath = new PathString("/api/Auth/NoLogin");
                // �S���v���ɷ|�Q�۰ʾɦV�o�Ӻ��}
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

            #region ���\�X�� wwwroot ��Ƨ��U�����e

            app.UseStaticFiles();

            #endregion

            app.UseHttpsRedirection();

            // app.UseAuthorization();

            #region ��������

            //���ǭn�@��
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            #endregion

            app.MapControllers();

            app.Run();
        }
    }
}