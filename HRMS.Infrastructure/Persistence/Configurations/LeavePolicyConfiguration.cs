using HRMS.Infrastructure.Persistence.Helpers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HRMS.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HRMS.Domain.Aggregates.LeaveAggregate;

public class LeavePolicyConfiguration : IEntityTypeConfiguration<LeavePolicy>
{
    public void Configure(EntityTypeBuilder<LeavePolicy> builder)
    {
        builder.ToTable("LeavePolicies");

        builder.HasKey(lp => lp.Id);

        builder.Property(lp => lp.LeaveType).IsRequired();

        builder.Property(lp => lp.DepartmentId).IsRequired(false);
        builder.Property(lp => lp.PositionId).IsRequired(false);

        builder.Property(lp => lp.AnnualAllocation).IsRequired();
        builder.Property(lp => lp.MaxCarryOver).IsRequired();
        builder.Property(lp => lp.AccrualMethod).IsRequired();
        builder.Property(lp => lp.AccrualRate).IsRequired().HasColumnType("decimal(10, 4)");

        builder.Property(lp => lp.MinRequestDays).IsRequired();
        builder.Property(lp => lp.MaxConsecutiveDays).IsRequired();

        builder.Property(lp => lp.RequiresApproval).IsRequired();
        builder.Property(lp => lp.RequiresHRApproval).IsRequired();
        

        builder.Property(lp => lp.IsActive).IsRequired();
        builder.Property(lp => lp.EffectiveDate).IsRequired();
        builder.Property(lp => lp.CreatedAt).IsRequired();
        builder.Property(lp => lp.UpdatedAt).IsRequired();
    }
}
