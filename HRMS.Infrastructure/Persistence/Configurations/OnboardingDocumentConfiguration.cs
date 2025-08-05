using HRMS.Domain.Aggregates.OnboardingAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations;

public class OnboardingDocumentConfiguration : IEntityTypeConfiguration<OnboardingDocument>
{
    public void Configure(EntityTypeBuilder<OnboardingDocument> builder)
    {
        builder.ToTable("OnboardingDocuments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.DocumentName).IsRequired().HasMaxLength(100);
        builder.Property(d => d.DocumentType).HasMaxLength(50);
        builder.Property(d => d.Status).HasConversion<string>();
        builder.Property(d => d.UploadedDate);
        builder.Property(d => d.ReviewedDate);
    }
}