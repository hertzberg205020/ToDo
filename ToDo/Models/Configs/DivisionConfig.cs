using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ToDo.Models.Configs;

public class DivisionConfig: IEntityTypeConfiguration<Division>
{
    public void Configure(EntityTypeBuilder<Division> builder)
    {
        builder.ToTable("t_Division");

        builder.Property(e => e.DivisionId).HasDefaultValueSql("(newid())");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}