using HRMS.Domain.Aggregates.DepartmentAggregate;
using HRMS.Domain.Aggregates.PositionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("Positions");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Title)
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(p => p.Code)
            .HasMaxLength(20)
            .IsRequired();
            
        builder.Property(p => p.BaseSalary)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
            
        builder.Property(p => p.Description)
            .HasMaxLength(500);
            
        builder.HasOne<Department>()
            .WithMany()
            .HasForeignKey(p => p.DepartmentId);
            
        builder.HasMany(p => p.Employees)
            .WithOne()
            .HasForeignKey(e => e.PositionId);
    }
}