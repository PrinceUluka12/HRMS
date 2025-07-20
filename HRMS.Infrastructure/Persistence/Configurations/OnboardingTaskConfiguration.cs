using HRMS.Domain.Aggregates.OnboardingAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations;

public class OnboardingTaskConfiguration : IEntityTypeConfiguration<OnboardingTask>
{
    public void Configure(EntityTypeBuilder<OnboardingTask> builder)
    {
        builder.ToTable("OnboardingTasks");
        
        builder.HasKey(ot => ot.Id);
        
        // Core properties
        builder.Property(ot => ot.Title)
            .HasMaxLength(200)
            .IsRequired();
            
        builder.Property(ot => ot.Description)
            .HasColumnType("nvarchar(1000)")
            .IsRequired();
            
        builder.Property(ot => ot.DueDate)
            .IsRequired();
            
        builder.Property(ot => ot.Status)
            .HasConversion<string>()
            .IsRequired();
            
        builder.Property(ot => ot.Category)
            .HasConversion<string>()
            .IsRequired();
            
        builder.Property(ot => ot.Priority)
            .IsRequired();

        // Completion tracking
        builder.Property(ot => ot.CompletedDate);
        builder.Property(ot => ot.CompletedBy)
            .HasMaxLength(100);
        builder.Property(ot => ot.CompletionNotes)
            .HasColumnType("nvarchar(2000)");

        // Audit fields
        builder.Property(ot => ot.CreatedDate)
            .IsRequired();
        builder.Property(ot => ot.CreatedBy)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(ot => ot.ModifiedDate);
        builder.Property(ot => ot.ModifiedBy)
            .HasMaxLength(100);

        // Relationships
        builder.HasMany(ot => ot.Documents)
            .WithOne()
            .HasForeignKey(d => d.OnboardingTaskId);

        // Indexes
        builder.HasIndex(ot => ot.EmployeeId);
        builder.HasIndex(ot => ot.Status);
        builder.HasIndex(ot => ot.DueDate);
    }
}