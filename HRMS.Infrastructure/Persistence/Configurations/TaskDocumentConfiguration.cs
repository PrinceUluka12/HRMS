using HRMS.Domain.Aggregates.OnboardingAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations;

public class TaskDocumentConfiguration : IEntityTypeConfiguration<TaskDocument>
{
    public void Configure(EntityTypeBuilder<TaskDocument> builder)
    {
        builder.ToTable("OnboardingTaskDocuments");
        
        builder.HasKey(td => td.Id);
        
        builder.Property(td => td.Name)
            .HasMaxLength(200)
            .IsRequired();
            
        builder.Property(td => td.FilePath)
            .HasMaxLength(500)
            .IsRequired();
            
        builder.Property(td => td.FileType)
            .HasMaxLength(50)
            .IsRequired();
            
        builder.Property(td => td.FileSize)
            .IsRequired();
            
        builder.Property(td => td.UploadDate)
            .IsRequired();
            
        builder.Property(td => td.UploadedBy)
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(td => td.IsRequired)
            .IsRequired();
            
        builder.Property(td => td.Status)
            .HasConversion<string>()
            .IsRequired();
    }
}