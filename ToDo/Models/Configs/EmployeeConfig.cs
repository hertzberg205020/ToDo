using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ToDo.Models.Configs;

public class EmployeeConfig: IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("t_Employee");

        builder.Property(e => e.EmployeeId).HasDefaultValueSql("(newid())");

        builder.Property(e => e.Account).IsRequired();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Password).IsRequired();

        builder.HasOne<Division>(d => d.Division)
            .WithMany(p => p.Employees)
            .HasForeignKey(d => d.DivisionId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Employee_DivisionId");

        builder.HasOne<JobTitle>(d => d.JobTitle)
            .WithMany(p => p.Employees)
            .HasForeignKey(d => d.JobTitleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Employee_JobTitleId");
    }
}