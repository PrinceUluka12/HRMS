using HRMS.Domain.Aggregates.DepartmentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(d => d.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(d => d.ManagerId)
            .IsRequired(false);

        builder.HasOne(d => d.Manager)
            .WithMany() // No reverse navigation from Employee to Department
            .HasForeignKey(d => d.ManagerId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

        
       
        
    }
}