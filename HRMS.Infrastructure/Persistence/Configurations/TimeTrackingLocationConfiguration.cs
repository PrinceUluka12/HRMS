using HRMS.Domain.Aggregates.TimeTrackingAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations;

public class TimeTrackingLocationConfiguration : IEntityTypeConfiguration<TimeTrackingLocation>
{
    public void Configure(EntityTypeBuilder<TimeTrackingLocation> builder)
    {
        builder.ToTable("time_tracking_locations");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.TimeEntryId).IsRequired();

        builder.Property(l => l.ActionType)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(l => l.Latitude)
            .HasPrecision(10, 8);

        builder.Property(l => l.Longitude)
            .HasPrecision(11, 8);

        builder.Property(l => l.Timestamp)
            .IsRequired();
    }
}