using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ToDo.Models;

public class ToDoDbContext: DbContext
{
    public DbSet<ToDoItem> ToDoList { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Division> Divisions { get; set; } = null!;
    public DbSet<JobTitle> JobTitles { get; set; } = null!;
    public DbSet<UploadFile> UploadFiles { get; set; } = null!;

    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 從當前運行的Assembly加載所有的IEntityTypeConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}