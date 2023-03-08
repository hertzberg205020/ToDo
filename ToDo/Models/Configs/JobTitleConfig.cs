using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ToDo.Models.Configs;

public class JobTitleConfig: IEntityTypeConfiguration<JobTitle>
{
    public void Configure(EntityTypeBuilder<JobTitle> builder)
    {
        builder.ToTable("t_JobTitle");

        builder.Property(e => e.JobTitleId).HasDefaultValueSql("(newid())");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}