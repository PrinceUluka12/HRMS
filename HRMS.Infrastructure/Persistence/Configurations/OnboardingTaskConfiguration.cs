using HRMS.Domain.Aggregates.OnboardingAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations;

public class OnboardingTaskConfiguration : IEntityTypeConfiguration<OnboardingTask>
{
    public void Configure(EntityTypeBuilder<OnboardingTask> builder)
    {
        builder.ToTable("OnboardingTasks");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.TaskName).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(500);
        builder.Property(t => t.AssignedTo).HasMaxLength(50);
        builder.Property(t => t.DueDate);
        builder.Property(t => t.CompletedDate);
        builder.Property(t => t.Notes).HasMaxLength(1000);
        builder.Property(t => t.Status).HasConversion<string>();
    }
}