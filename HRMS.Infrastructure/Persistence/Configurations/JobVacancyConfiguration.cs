using HRMS.Domain.Aggregates.RecruitmentAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations;

public class JobVacancyConfiguration: IEntityTypeConfiguration<JobVacancy>
{
    public void Configure(EntityTypeBuilder<JobVacancy> builder)
    {
        builder.ToTable("JobVacancies");
            
        builder.HasKey(j => j.Id);
            
        builder.Property(j => j.Title)
            .IsRequired()
            .HasMaxLength(100);
                
        builder.Property(j => j.Description)
            .IsRequired()
            .HasMaxLength(2000);
                
        builder.Property(j => j.Location)
            .IsRequired()
            .HasMaxLength(100);
                
        builder.Property(j => j.EmploymentType)
            .IsRequired()
            .HasConversion<string>();
                
        builder.Property(j => j.PostedOn)
            .IsRequired(false);
                
        builder.Property(j => j.ClosingOn)
            .IsRequired();
                
        builder.Property(j => j.Status)
            .IsRequired()
            .HasConversion<string>();
                
        builder.HasMany(j => j.Applications)
            .WithOne()
            .HasForeignKey(a => a.JobVacancyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}