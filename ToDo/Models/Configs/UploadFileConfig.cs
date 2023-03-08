using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ToDo.Models.Configs;

public class UploadFileConfig: IEntityTypeConfiguration<UploadFile>
{

    public void Configure(EntityTypeBuilder<UploadFile> builder)
    {
        builder.ToTable("t_UploadFile");

        builder.Property(e => e.UploadFileId).HasDefaultValueSql("(newid())");

        builder.Property(e => e.Name).IsRequired();

        builder.Property(e => e.Src).IsRequired();

        builder.HasOne(d => d.ToDoItem)
            .WithMany(p => p.UploadFiles)
            .HasForeignKey(d => d.ToDoId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_UploadFile_ToDoId");
    }
}