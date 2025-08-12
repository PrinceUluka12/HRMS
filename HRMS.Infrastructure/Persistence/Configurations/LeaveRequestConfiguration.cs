using HRMS.Infrastructure.Persistence.Helpers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HRMS.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HRMS.Domain.Aggregates.LeaveAggregate;

public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder.ToTable("LeaveRequests");

        builder.HasKey(lr => lr.Id);

        builder.Property(lr => lr.EmployeeId).IsRequired();
        
        builder.Property(lr => lr.StartDate).IsRequired();
        builder.Property(lr => lr.EndDate).IsRequired();

        builder.Property(lr => lr.Type).IsRequired();
        builder.Property(lr => lr.Reason).IsRequired().HasMaxLength(1000);

        builder.Property(lr => lr.Status).IsRequired().HasConversion<string>().HasMaxLength(20).IsRequired();;
        builder.Property(lr => lr.RequestDate).IsRequired();

        builder.Property(lr => lr.ApprovedBy).HasMaxLength(100).IsRequired(false);
        builder.Property(lr => lr.ApprovedDate).IsRequired(false);

        builder.Property(lr => lr.ReviewedBy).HasMaxLength(100).IsRequired(false);
        builder.Property(lr => lr.ReviewedAt).IsRequired(false);
        builder.Property(lr => lr.ReviewComments).HasMaxLength(1000).IsRequired(false);

        builder.Property(lr => lr.RejectionReason).HasMaxLength(1000).IsRequired(false);

        
        var attachmentsConverter = new ValueConverter<string[], string>(
            v => JsonHelper.SerializeStringArray(v),
            v => JsonHelper.DeserializeStringArray(v) ?? Array.Empty<string>()
        );
        // Store Attachments as JSON or string - here assuming JSON serialization
        builder.Property(lr => lr.Attachments)
            .HasConversion(attachmentsConverter)
            .HasColumnType("nvarchar(max)")
            .IsRequired(false);

        builder.Property(lr => lr.IsHalfDay).IsRequired();

        // Relationships
        builder.HasOne(lr => lr.Employee)
               .WithMany()  // Assuming Employee has collection of LeaveRequests
               .HasForeignKey(lr => lr.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
