namespace HRMS.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HRMS.Domain.Aggregates.LeaveAggregate;

public class TimeOffBalanceConfiguration : IEntityTypeConfiguration<TimeOffBalance>
{
    public void Configure(EntityTypeBuilder<TimeOffBalance> builder)
    {
        builder.ToTable("TimeOffBalances");

        builder.HasKey(tb => tb.Id);

        builder.Property(tb => tb.EmployeeId).IsRequired();
        builder.Property(tb => tb.LeaveType).IsRequired();

        builder.Property(tb => tb.TotalAllowed).IsRequired().HasColumnType("decimal(10, 2)");
        builder.Property(tb => tb.Used).IsRequired().HasColumnType("decimal(10, 2)");
        builder.Property(tb => tb.Pending).IsRequired().HasColumnType("decimal(10, 2)");
        builder.Property(tb => tb.CarryOver).IsRequired().HasColumnType("decimal(10, 2)");
        builder.Property(tb => tb.AccrualRate).IsRequired().HasColumnType("decimal(10, 4)");

        builder.Property(tb => tb.LastAccrualDate).IsRequired();
        builder.Property(tb => tb.PolicyYear).IsRequired();

        builder.Property(tb => tb.CreatedAt).IsRequired();
        builder.Property(tb => tb.UpdatedAt).IsRequired();
    }
}
