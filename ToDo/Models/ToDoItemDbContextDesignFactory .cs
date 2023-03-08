using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ToDo.Models;

public class ToDoItemDbContextDesignFactory: IDesignTimeDbContextFactory<ToDoDbContext>
{
    public ToDoDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<ToDoDbContext> builder = new DbContextOptionsBuilder<ToDoDbContext>();
        string connStr = Environment.GetEnvironmentVariable("ConnectionStrings:Demo2")!;
        builder.UseSqlServer(connStr!);
        return new ToDoDbContext(builder.Options);
    }
}