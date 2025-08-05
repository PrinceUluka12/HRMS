using HRMS.Domain.Aggregates.OnboardingAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations;

public class OnboardingConfiguration: IEntityTypeConfiguration<Onboarding>
{
    public void Configure(EntityTypeBuilder<Onboarding> builder)
    {
        builder.ToTable("Onboardings");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.EmployeeId).IsRequired();
        builder.Property(o => o.StartDate).IsRequired();
        builder.Property(o => o.OverallProgress);
        builder.Property(o => o.Status).HasConversion<string>();
        builder.Property(o => o.DaysRemaining);
        builder.Property(o => o.LastActivity);
        builder.Property(o => o.CreatedDate);

        builder.Navigation(o => o.Stages).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(o => o.Documents).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(o => o.Stages)
            .WithOne()
            .HasForeignKey("OnboardingId")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(o => o.Documents)
            .WithOne()
            .HasForeignKey("OnboardingId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}