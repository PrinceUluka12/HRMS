using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Persistence.Configurations;

public class ApplicationConfiguration : IEntityTypeConfiguration<Domain.Aggregates.RecruitmentAggregates.Application>
{
    public void Configure(EntityTypeBuilder<Domain.Aggregates.RecruitmentAggregates.Application> builder)
    {
        builder.ToTable("Applications");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.JobVacancyId)
            .IsRequired();

        builder.OwnsOne(a => a.CandidateInfo, candidate =>
        {
            candidate.WithOwner();

            candidate.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);

            candidate.Property(c => c.PhoneNumber)
                .HasMaxLength(20);

            candidate.OwnsOne(c => c.Name, name =>
            {
                name.Property(n => n.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                name.Property(n => n.LastName)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        });

        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(a => a.AppliedDate)
            .IsRequired();

        builder.OwnsMany(a => a.Notes, note =>
        {
            note.WithOwner().HasForeignKey("ApplicationId");
            note.Property(n => n.Value).IsRequired();
            note.ToTable("ApplicationNotes");
        });

        builder.HasMany(a => a.Attachments)
            .WithOne()
            .HasForeignKey("ApplicationId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}