using HRMS.Domain.Aggregates.RecruitmentAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations;

public class AttachmentConfiguration: IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.ToTable("Attachments");
            
        builder.HasKey(a => a.Id);
            
        builder.Property(a => a.FileName)
            .IsRequired()
            .HasMaxLength(255);
                
        builder.Property(a => a.FileType)
            .IsRequired()
            .HasMaxLength(100);
                
        builder.Property(a => a.FileUrl)
            .IsRequired();
    }
}