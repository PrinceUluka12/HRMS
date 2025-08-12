using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HRMS.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HRMS.Domain.Aggregates.LeaveAggregate;

public class ApprovalHistoryConfiguration : IEntityTypeConfiguration<ApprovalHistory>
{
    public void Configure(EntityTypeBuilder<ApprovalHistory> builder)
    {
        builder.ToTable("ApprovalHistories");

        builder.HasKey(ah => ah.Id);

        builder.Property(ah => ah.RequestId).IsRequired();

        builder.Property(ah => ah.ApproverType).IsRequired();
        builder.Property(ah => ah.ApproverId).IsRequired();
        builder.Property(ah => ah.ApproverName).IsRequired().HasMaxLength(200);

        builder.Property(ah => ah.Action).IsRequired();
        builder.Property(ah => ah.PreviousStatus).IsRequired();
        builder.Property(ah => ah.NewStatus).IsRequired();

        builder.Property(ah => ah.Comments).HasMaxLength(1000).IsRequired(false);
        builder.Property(ah => ah.Timestamp).IsRequired();

       
    }
}
