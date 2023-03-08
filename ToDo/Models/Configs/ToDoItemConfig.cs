using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ToDo.Models.Configs;

public class ToDoItemConfig: IEntityTypeConfiguration<ToDoItem>
{
    public void Configure(EntityTypeBuilder<ToDoItem> builder)
    {
        builder.ToTable("t_ToDoItem");
        builder.HasKey(e => e.TodoId);

        builder.Property(e => e.TodoId).HasDefaultValueSql("(newid())");
        builder.Property(e => e.InsertTime)
            .HasColumnType("datetime")
            .HasDefaultValueSql("(getdate())");

        builder.Property(e => e.Name).IsRequired();

        builder.Property(e => e.UpdateTime)
            .HasColumnType("datetime")
            .HasDefaultValueSql("(getdate())");

        builder.HasOne<Employee>(d => d.InsertEmployee)
            .WithMany(p => p.TodoListInsertEmployees)
            .HasForeignKey(d => d.InsertEmployeeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Todo_InsertEmpId");

        builder.HasOne<Employee>(d => d.UpdateEmployee)
            .WithMany(p => p.TodoListUpdateEmployees)
            .HasForeignKey(d => d.UpdateEmployeeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Todo_UpdateEmpId");
    }
}