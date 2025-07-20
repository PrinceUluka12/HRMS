using HRMS.Domain.Aggregates.PerformanceAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations;

public class PerformanceReviewConfiguration : IEntityTypeConfiguration<PerformanceReview>
{
    public void Configure(EntityTypeBuilder<PerformanceReview> builder)
    {
        builder.ToTable("PerformanceReviews");
        
        builder.HasKey(pr => pr.Id);
        
        builder.Property(pr => pr.ReviewDate)
            .IsRequired();
            
        builder.Property(pr => pr.NextReviewDate);
            
        builder.Property(pr => pr.Status)
            .HasConversion<string>()
            .IsRequired();
            
        builder.Property(pr => pr.OverallComments)
            .HasColumnType("nvarchar(2000)")
            .IsRequired();
            
        builder.Property(p => p.OverallRating).HasPrecision(5, 2); ;
            
        // Relationships
        builder.HasMany(pr => pr.Metrics)
            .WithOne()
            .HasForeignKey(m => m.PerformanceReviewId);
            
        builder.HasMany(pr => pr.Goals)
            .WithOne()
            .HasForeignKey(g => g.PerformanceReviewId);
            
        builder.HasMany(pr => pr.Feedback)
            .WithOne()
            .HasForeignKey(f => f.PerformanceReviewId);
    }
}