using HRMS.Domain.Aggregates.OnboardingAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations;

public class OnboardingStageConfiguration: IEntityTypeConfiguration<OnboardingStage>
{
    public void Configure(EntityTypeBuilder<OnboardingStage> builder)
    {
        builder.ToTable("OnboardingStages");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.StageName).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Status).HasConversion<string>();
        builder.Property(s => s.Progress);
        builder.Property(s => s.DueDate);

        builder.Navigation(s => s.Tasks).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(s => s.Tasks)
            .WithOne()
            .HasForeignKey("StageId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}