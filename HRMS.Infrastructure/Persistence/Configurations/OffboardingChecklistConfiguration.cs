using HRMS.Domain.Aggregates.OffboardingAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations;

public class OffboardingChecklistConfiguration : IEntityTypeConfiguration<OffboardingChecklist>
{
    public void Configure(EntityTypeBuilder<OffboardingChecklist> builder)
    {
        builder.ToTable("OffboardingChecklists");
        
        builder.HasKey(oc => oc.Id);
        
        // Core properties
        builder.Property(oc => oc.InitiationDate)
            .IsRequired();
            
        builder.Property(oc => oc.CompletionDate);
            
        builder.Property(oc => oc.Status)
            .HasConversion<string>()
            .IsRequired();
            
        builder.Property(oc => oc.Type)
            .HasConversion<string>()
            .IsRequired();
            
        builder.Property(oc => oc.InitiatedBy)
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(oc => oc.CompletedBy)
            .HasMaxLength(100);

        // Exit details
        builder.Property(oc => oc.LastWorkingDate)
            .IsRequired();
            
        builder.Property(oc => oc.ExitReason)
            .HasMaxLength(500);
            
        builder.Property(oc => oc.ExitInterviewNotes)
            .HasColumnType("nvarchar(2000)");
            
        builder.Property(oc => oc.ExitInterviewConducted)
            .IsRequired();

        // Audit fields
        builder.Property(oc => oc.CreatedDate)
            .IsRequired();
            
        builder.Property(oc => oc.ModifiedDate);

        // Relationships
        builder.HasMany(oc => oc.Tasks)
            .WithOne()
            .HasForeignKey(ot => ot.ChecklistId);
            
        builder.HasMany(oc => oc.ReturnedAssets)
            .WithOne()
            .HasForeignKey(ar => ar.ChecklistId);
            
        builder.HasMany(oc => oc.AccessRevocations)
            .WithOne()
            .HasForeignKey(ac => ac.ChecklistId);

        // Indexes
        builder.HasIndex(oc => oc.EmployeeId);
        builder.HasIndex(oc => oc.Status);
        builder.HasIndex(oc => oc.LastWorkingDate);
    }
}