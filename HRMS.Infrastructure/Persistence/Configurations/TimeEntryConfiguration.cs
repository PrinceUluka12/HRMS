using HRMS.Domain.Aggregates.TimeTrackingAggregate;
using HRMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations;

public class TimeEntryConfiguration : IEntityTypeConfiguration<TimeEntry>
{
    public void Configure(EntityTypeBuilder<TimeEntry> builder)
    {
        builder.ToTable("time_entries");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.EmployeeId)
            .IsRequired();

        builder.Property(e => e.Date)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(e => e.ClockIn)
            .HasColumnType("time")
            .IsRequired();

        builder.Property(e => e.ClockOut)
            .HasColumnType("time")
            .IsRequired(false);

        builder.Property(e => e.BreakTimeMinutes);

        builder.Property(e => e.Description)
            .HasColumnType("text");

        builder.Property(e => e.Project)
            .HasMaxLength(255);

        builder.Property(e => e.TotalHours)
            .HasPrecision(4, 2)
            .IsRequired();

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasDefaultValue(TimeEntryStatus.Active)
            .IsRequired();

        builder.Property(e => e.CreatedAt);


        builder.Property(e => e.UpdatedAt);


        builder.HasIndex(e => new { e.EmployeeId, e.Date }).HasDatabaseName("idx_employee_date");
        builder.HasIndex(e => e.Status).HasDatabaseName("idx_status");
        builder.HasIndex(e => e.Date).HasDatabaseName("idx_date_range");

        builder.HasMany(e => e.Locations)
            .WithOne()
            .HasForeignKey(l => l.TimeEntryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}